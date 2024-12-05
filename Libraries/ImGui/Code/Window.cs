using System;

namespace Duccsoft;

internal class Window
{
	public string Name { get; set; }
	public Vector2 Position { get; set; } = new Vector2( 250, 50 );
	public Vector2 Pivot { get; set; }
	public Vector2 Size { get; set; } = new Vector2( 175, 100 );
	public Action OnClose { get; set; }

	public void Paint( ImGuiPainter painter )
	{
		// Paint window
		var position = Position;
		var pivotPx = Size * Pivot;
		position -= pivotPx;
		var screenRect = new Rect( position, Size );
		var windowBgColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_WindowBg );
		var borderColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_Border );
		painter.DrawRect( screenRect, windowBgColor, default, Vector4.One, borderColor );

		// Paint title background
		var textPanelSize = new Vector2( screenRect.Width, screenRect.Height * 0.15f );
		var textPanelRect = new Rect( screenRect.Position, textPanelSize );
		var titleBgActiveColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_TitleBgActive );
		painter.DrawRect( textPanelRect, titleBgActiveColor );

		// Paint title
		var textRect = new Rect( textPanelRect.Position, textPanelSize * 0.95f );
		painter.DrawText( Name, textRect );
	}
}
