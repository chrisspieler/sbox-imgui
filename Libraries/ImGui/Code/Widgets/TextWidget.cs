﻿namespace Duccsoft.ImGui;

internal class TextWidget : Widget
{
	public TextWidget( Window parent, string text ) : base( parent )
	{
		Text = text;
		Show();
	}

	public string Text { get; set; }

	public override Vector2 GetSize()
	{
		return ImGui.CalcTextSize( Text );
	}

	public override void Paint( ImGuiPainter painter )
	{
		painter.DrawText( Text, new Rect( ScreenPosition, GetSize() ) );
	}
}