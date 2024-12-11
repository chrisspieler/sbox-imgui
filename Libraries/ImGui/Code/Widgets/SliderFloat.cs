using Duccsoft.ImGui.Rendering;
using System;

namespace Duccsoft.ImGui;

internal class SliderFloat : Widget
{
	public SliderFloat( Window parent, string label, Func<float> getter, Action<float> setter, 
		float valueMin, float valueMax, string format ) 
		: base( parent )
	{
		Label = label;
		ValueGetter = getter;
		ValueSetter = setter;
		MinValue = valueMin;
		MaxValue = valueMax;
		Format = format;
		Show();
	}

	public string Label { get; set; }
	public float Value
	{
		get => ValueGetter.Invoke();
		set => ValueSetter.Invoke( value );
	}
	public Func<float> ValueGetter { get; set; }
	public Action<float> ValueSetter { get; set; }
	public float MinValue { get; set; } = -128f;
	public float MaxValue { get; set; } = 256f;
	public string Format { get; set; } = null;

	public override Vector2 Size => new Vector2( 250 * ImGuiStyle.UIScale, ImGui.GetFrameHeightWithSpacing() );

	public override void UpdateInput()
	{
		base.UpdateInput();

		if ( IsActive )
		{
			var xPosMin = ScreenPosition.x;
			var xPosMax = ScreenPosition.x + Size.x;
			var dragProgress = MathX.LerpInverse( ImGui.GetMousePos().x, xPosMin, xPosMax );
			var targetValue = MathX.Lerp( MinValue, MaxValue, dragProgress );
			ValueSetter( targetValue );
		}
	}

	public override void Draw( ImDrawList drawList )
	{
		// Paint background
		var bgRect = new Rect( ScreenPosition, Size );
		var bgColor = ImGui.GetColorU32( ImGuiCol.FrameBg );
		if ( IsActive )
		{
			bgColor = ImGui.GetColorU32( ImGuiCol.FrameBgActive );
		}
		else if ( IsHovered )
		{
			bgColor = ImGui.GetColorU32( ImGuiCol.FrameBgHovered );
		}
		drawList.AddRectFilled( ScreenPosition, ScreenPosition + Size, bgColor );

		// Paint grab
		var grabSize = new Vector2( Style.GrabMinSize, ImGui.GetFrameHeight() );
		var xGrabPos = Value.Remap( MinValue, MaxValue, 0f, bgRect.Size.x - grabSize.x );
		var grabPos = ScreenPosition + new Vector2( xGrabPos, Style.FramePadding.y * 0.5f );
		var grabColor = ImGui.GetColorU32( ImGuiCol.SliderGrab );
		if ( IsActive )
		{
			grabColor = ImGui.GetColorU32( ImGuiCol.SliderGrabActive );
		}
		drawList.AddRectFilled( grabPos, grabPos + grabSize, grabColor );

		// Paint value
		var text = Value.ToString( Format );
		var xOffsetText = bgRect.Size.x * 0.5f;
		var textPos = ScreenPosition + new Vector2( xOffsetText, Style.FramePadding.y );
		drawList.AddText( textPos, ImGui.GetColorU32( ImGuiCol.Text ), text );
	}
}
