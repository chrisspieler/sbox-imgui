using Duccsoft.ImGui.Rendering;
using System.Numerics;

namespace Duccsoft.ImGui.Elements;

public class Slider<T> : Element where T : INumber<T>
{
	public Slider( Element parent, string label, ref T[] components, T min, T max, string format ) 
		: base( parent )
	{
		Label = label;
		ComponentCount = components.Length;

		OnBegin();
		var sliderWidth = BarAreaWidth / (components.Length);
		for ( int i = 0; i < components.Length; i++ )
		{
			ImGui.PushID( i );
			_ = new SliderBar( this, ref components[i], sliderWidth, min, max, format );
			ImGui.PopID();
			ImGui.SameLine( sliderWidth, Style.ItemInnerSpacing.x );
		}
		OnEnd();
	}

	public string Label { get; set; }
	public int ComponentCount { get; set; }
	private float BarAreaWidth => ImGui.GetFontSize() * 15f;
	public override Vector2 Size => new Vector2( BarAreaWidth, ImGui.GetFrameHeightWithSpacing() )
		+ ComponentCount * new Vector2( Style.ItemInnerSpacing.x, 0f );

	private class SliderBar : Element
	{
		public SliderBar( Element parent, ref T value, float width, T min, T max, string format )
			: base( parent )
		{
			Value = value;
			Min = min;
			Max = max;
			Format = format;

			Size = new Vector2( width, ImGui.GetFrameHeightWithSpacing() );

			OnBegin();
			OnEnd();

			value = Value;
		}

		public T Value { get; set; }
		public T Min { get; init; }
		public T Max { get; init; }
		public string Format { get; init; }

		protected float ValueProgress
		{
			get => LerpInverse( Value, Min, Max );
			set
			{
				Value = Lerp( Min, Max, value );
			}
		}

		private static T Lerp( T from, T to, float frac, bool clamp = true )
		{
			if ( clamp )
			{
				frac = frac.Clamp( 0f, 1f );
			}

			var fromF = float.CreateTruncating( from );
			var toF = float.CreateTruncating( to );
			return T.CreateTruncating( fromF + frac * (toF - fromF) );
		}

		private static float LerpInverse( T value, T from, T to )
		{
			var valueF = float.CreateTruncating( value );
			var fromF = float.CreateTruncating( from );
			var toF = float.CreateTruncating( to );
			valueF -= fromF;
			toF -= fromF;
			return valueF / toF;
		}

		protected Color32 GrabColor
		{
			get
			{
				return IsActive
					? ImGui.GetColorU32( ImGuiCol.SliderGrabActive )
					: ImGui.GetColorU32( ImGuiCol.SliderGrab );
			}
		}

		public override void OnUpdateInput()
		{
			base.OnUpdateInput();

			if ( IsActive )
			{
				var xPosMin = ScreenPosition.x;
				var xPosMax = ScreenPosition.x + Size.x;
				ValueProgress = MathX.LerpInverse( ImGui.GetMousePos().x, xPosMin, xPosMax );
			}
		}

		protected override void OnDrawSelf( ImDrawList drawList )
		{
			var bgRect = new Rect( ScreenPosition, Size );
			drawList.AddRectFilled( ScreenPosition, ScreenPosition + Size, FrameColor );

			var grabSize = new Vector2( Style.GrabMinSize, ImGui.GetFrameHeight() );
			var xGrabPos = MathX.Lerp( 0f, bgRect.Size.x - grabSize.x, ValueProgress );
			var grabPos = ScreenPosition + new Vector2( xGrabPos, Style.FramePadding.y * 0.5f );
			drawList.AddRectFilled( grabPos, grabPos + grabSize, GrabColor );

			var text = string.Format( "{0:" + Format + "}", Value );
			var xOffsetText = bgRect.Size.x * 0.5f;
			var textPos = ScreenPosition + new Vector2( xOffsetText, Style.FramePadding.y );
			drawList.AddText( textPos, ImGui.GetColorU32( ImGuiCol.Text ), text, TextFlag.CenterTop );
		}
	}
}
