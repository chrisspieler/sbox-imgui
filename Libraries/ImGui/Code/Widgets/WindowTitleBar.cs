namespace Duccsoft.ImGui;

internal class WindowTitleBar : Widget
{
	public WindowTitleBar( Window parent ) : base( parent )
	{
		Show();
	}

	public static Color32 TitleActiveColor => ImGui.GetColorU32( ImGuiCol.TitleBgActive );
	public static Color32 TitleInactiveColor => ImGui.GetColorU32( ImGuiCol.TitleBg );

	public override Vector2 GetSize() => GetTitleTextSize() * new Vector2( 2, 1 );

	private Rect GetTitleBarRect()
	{
		var textPanelSize = new Vector2( Parent.ScreenRect.Width, ImGui.GetFrameHeightWithSpacing() );
		return new Rect( Parent.ScreenRect.Position, textPanelSize );
	}

	private Vector2 GetTitleTextSize() => ImGui.CalcTextSize( Parent.Name ) + ImGui.GetStyle().FramePadding * 2;

	public override void Paint( ImGuiPainter painter )
	{
		var titleBarRect = GetTitleBarRect();

		// Paint title background
		var titleBarColor = Parent.IsFocused
			? TitleActiveColor
			: TitleInactiveColor;
		painter.DrawRect( titleBarRect, titleBarColor );

		// Paint title
		var textPanelSize = GetTitleTextSize();
		var xTextOffset = titleBarRect.Size.x * 0.5f - textPanelSize.x * 0.5f;
		var yTextOffset = textPanelSize.y * 0.25f;
		var textPanelPos = titleBarRect.Position + new Vector2( xTextOffset, yTextOffset );
		var textRect = new Rect( textPanelPos, textPanelSize );
		painter.DrawText( Parent.Name, textRect );
	}
}
