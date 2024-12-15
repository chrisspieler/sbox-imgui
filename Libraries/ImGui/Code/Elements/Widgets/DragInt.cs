using Duccsoft.ImGui.Rendering;
using System;

namespace Duccsoft.ImGui.Elements;

internal class DragInt : Element
{
	public DragInt( Window parent, string label, ref int value, float speed, 
		int minValue, int maxValue, string format, ImGuiSliderFlags flags ) 
		: base( parent )
	{
		Label = label;
		Value = value;
		Speed = speed;
		MinValue = minValue;
		MaxValue = maxValue;
		Format = format;
		Flags = flags;

		Size = new Vector2( 250 * ImGuiStyle.UIScale, ImGui.GetFrameHeightWithSpacing() );

		OnBegin();
		OnEnd();

		value = Value;
	}

	public string Label { get; set; }
	public int Value { get; set; }
	public Func<int> ValueGetter { get; set; }
	public Action<int> ValueSetter { get; set; }
	public float Speed { get; set; }
	public int MinValue { get; set; }
	public int MaxValue { get; set; }
	public string Format { get; set; }
	public ImGuiSliderFlags Flags { get; set; }

	public override void OnUpdateInput()
	{
		base.OnUpdateInput();

		if ( IsActive )
		{
			var delta = MouseState.LeftClickDragFrameDelta.x * Speed;
			Value = Value += (int)delta;
		}
	}

	protected override void OnDrawSelf( ImDrawList drawList )
	{
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

		// Paint value
		var text = Value.ToString( Format );
		var xOffsetText = bgRect.Size.x * 0.5f;
		var textPos = ScreenPosition + new Vector2( xOffsetText, ImGui.GetStyle().FramePadding.y );
		drawList.AddText( textPos, ImGui.GetColorU32( ImGuiCol.Text ), text );
	}
}
