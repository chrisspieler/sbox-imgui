using Duccsoft.ImGui.Rendering;
using System;
using System.Numerics;

namespace Duccsoft.ImGui.Elements;

public class Slider<T> : Element where T : INumber<T>
{
	public Slider( Element parent, string label, T value, Action<T> setter, T min, T max, string format, int componentCount ) 
		: base( parent )
	{
		OnBegin();
		for ( int i = 0; i < componentCount; i++ )
		{
			ImGui.PushID( i );
			_ = new SliderBar<T>( this, i, value, setter, min, max, format );
			ImGui.SameLine();
			ImGui.PopID();
		}
		OnEnd();
	}
}

public class SliderBar<T> : Element where T : INumber<T>
{
	public SliderBar( Element parent, int index, T value, Action<T> setter, T min, T max, string format ) 
		: base( parent )
	{
		Index = index;
		Value = value;
		Setter = setter;
		Min = min;
		Max = max;
		Format = format;

		Size = new Vector2( 250 * ImGuiStyle.UIScale, ImGui.GetFrameHeightWithSpacing() );

		OnBegin();
		OnEnd();
	}

	public int Index { get; init; }
	public T Value { get; set; }
	public Action<T> Setter { get; init; }
	public T Min { get; init; }
	public T Max { get; init; }
	public string Format { get; init; }

	protected float ValueProgress
	{
		get => LerpInverse( Value, Min, Max );
		set
		{
			Value = Lerp( Min, Max, value );
			Setter?.Invoke( Value );
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
		drawList.AddText( textPos, ImGui.GetColorU32( ImGuiCol.Text ), text );
	}
}
