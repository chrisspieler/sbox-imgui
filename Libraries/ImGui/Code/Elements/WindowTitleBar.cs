﻿using Duccsoft.ImGui.Rendering;

namespace Duccsoft.ImGui.Elements;

internal class WindowTitleBar : Element
{
	public WindowTitleBar( Window parent ) : base( parent )
	{
		TitleText = parent.Name;

		TitleTextSize = ImGui.CalcTextSize( TitleText ) + ImGui.GetStyle().FramePadding * 2;

		OnBegin();
	}
	public static Color32 TitleActiveColor => ImGui.GetColorU32( ImGuiCol.TitleBgActive );
	public static Color32 TitleInactiveColor => ImGui.GetColorU32( ImGuiCol.TitleBg );

	public override Vector2 Size => new( Parent.Size.x, ImGui.GetFrameHeightWithSpacing() );

	public string TitleText { get; set; }
	private Rect TitleBarRect => new( Parent.ScreenPosition, Size );
	private Vector2 TitleTextSize { get; set; }

	public override void UpdateInput()
	{
		base.UpdateInput();
		Parent.IsDragged = IsActive;
		if ( IsActive )
		{
			if ( MouseState.LeftClickPressed || MouseState.LeftClickReleased )
			{
				ImGuiSystem.Current.CustomWindowPositions[Parent.Id] = Parent.ScreenRect.Position;
			}
		}
	}

	protected override void DrawSelf( ImDrawList drawList )
	{
		var titleBarRect = TitleBarRect;

		// Paint title background
		var titleBarColor = Parent.IsFocused
			? TitleActiveColor
			: TitleInactiveColor;
		drawList.AddRectFilled( titleBarRect.Position, titleBarRect.Position + titleBarRect.Size, titleBarColor );

		// Paint title
		var textPanelSize = TitleTextSize;
		var xTextOffset = titleBarRect.Size.x * 0.5f - textPanelSize.x * 0.5f;
		var yTextOffset = textPanelSize.y * 0.25f;
		var textPanelPos = titleBarRect.Position + new Vector2( xTextOffset, yTextOffset );
		drawList.AddText( textPanelPos, ImGui.GetColorU32( ImGuiCol.Text ), TitleText );
	}
}