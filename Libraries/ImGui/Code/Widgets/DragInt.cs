﻿using Duccsoft.ImGui.Rendering;
using System;

namespace Duccsoft.ImGui;

internal class DragInt : Widget
{
	public DragInt( Window parent, string label, Func<int> getter, Action<int> setter,
		float speed, int minValue, int maxValue, string format, ImGuiSliderFlags flags ) 
		: base( parent )
	{
		Label = label;
		ValueGetter = getter;
		ValueSetter = setter;
		Speed = speed;
		MinValue = minValue;
		MaxValue = maxValue;
		Format = format;
		Flags = flags;
		Show();
	}

	public string Label { get; set; }
	public int Value
	{
		get => ValueGetter.Invoke();
		set => ValueSetter.Invoke( value );
	}
	public Func<int> ValueGetter { get; set; }
	public Action<int> ValueSetter { get; set; }
	public float Speed { get; set; }
	public int MinValue { get; set; }
	public int MaxValue { get; set; }
	public string Format { get; set; }
	public ImGuiSliderFlags Flags { get; set; }

	public override Vector2 Size => new Vector2( 250 * ImGuiStyle.UIScale, ImGui.GetFrameHeightWithSpacing() );

	public override void UpdateInput()
	{
		base.UpdateInput();


		if ( IsActive )
		{
			var delta = MouseState.LeftClickDragTotalDelta.x * Speed;
			Value = Value += (int)delta;
		}
	}

	public override void Draw( ImDrawList drawList )
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
		var textPos = ScreenPosition + new Vector2( xOffsetText, Style.FramePadding.y );
		drawList.AddText( textPos, ImGui.GetColorU32( ImGuiCol.Text ), text );
	}
}