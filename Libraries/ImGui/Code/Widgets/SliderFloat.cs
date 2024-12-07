﻿using System;

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

	public override Vector2 GetSize()
	{
		return new Vector2( 250 * ImGuiStyle.UIScale, ImGui.GetFrameHeightWithSpacing() );
	}

	public override void Paint( ImGuiPainter painter )
	{
		var bgRect = new Rect( ScreenPosition, GetSize() );
		var bgColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_FrameBg );
		if ( IsActive )
		{
			bgColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_FrameBgActive );
		}
		else if ( IsHovered )
		{
			bgColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_FrameBgHovered );
		}
		painter.DrawRect( bgRect, bgColor );

		var grabSize = new Vector2( Style.GrabMinSize, ImGui.GetFrameHeight() ) ;
		var xGrabPos = Value.Remap( MinValue, MaxValue, 0f, bgRect.Size.x );
		var grabPos = ScreenPosition + new Vector2( xGrabPos, Style.FramePadding.y * 0.5f );
		var grabColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_SliderGrab );
		if ( IsActive )
		{
			grabColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_SliderGrabActive );
		}
		painter.DrawRect( new Rect( grabPos, grabSize ), grabColor );

		var text = Value.ToString( Format );
		var textSize = ImGui.CalcTextSize( text );
		var xOffsetText = bgRect.Size.x * 0.5f;
		var textPos = ScreenPosition + new Vector2( xOffsetText, Style.FramePadding.y );
		painter.DrawText( text, new Rect( textPos, textSize ) );
	}
}
