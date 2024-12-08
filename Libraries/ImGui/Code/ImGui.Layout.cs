using System;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static float GetTextLineHeight() => GetFontSize();
	public static float GetTextLineHeightWithSpacing() => GetFontSize() + GetStyle().ItemSpacing.y;
	public static float GetFrameHeight() => GetFontSize() + GetStyle().FramePadding.y * 2;
	public static float GetFrameHeightWithSpacing() => GetFrameHeight() + GetStyle().ItemSpacing.y;
	public static Vector2 CalcTextSize( string text, string textEnd = null, bool hideTextAfterDoubleHash = false, float wrapWidth = 1.0f )
	{
		// Assume each character is square. Maybe a bad assumption!
		return new Vector2( text.Length * GetTextLineHeight() * (Screen.Height / Screen.Width), GetTextLineHeight() );
	}

	public static Vector2 GetCursorScreenPos() => GetCursorPos() + GetWindowPos();
	public static void SetCursorScreenPos( Vector2 position )
	{
		if ( CurrentWindow is null )
			return;

		CurrentWindow.CursorPosition = position - GetWindowPos();
	}

	public static Vector2 GetContentRegionAvail()
	{
		// TODO: Calculate available space in window/group based on max size.
		throw new NotImplementedException();
	}
	public static Vector2 GetCursorPos()
	{
		if ( CurrentWindow is null )
			return default;

		return CurrentWindow.CursorPosition;
	}
	public static float GetCursorPosX() => GetCursorPos().x;
	public static float GetCursorPosY() => GetCursorPos().y;

	public static Vector2 GetCursorStartPos()
	{
		// TODO: Store start position separately from screen position to account for vertical layouts and padding.
		throw new NotImplementedException();
	}

	public static void NewLine()
	{
		if ( CurrentWidget is null )
			return;

		var widgetRect = CurrentWidget.ScreenRect;
		var cursorOffset = new Vector2( 0f, widgetRect.Size.y + GetStyle().ItemSpacing.y );
		SetCursorScreenPos( widgetRect.Position + cursorOffset );
	}

	public static void SameLine( float offsetFromStartX = 0f, float spacing = -1f )
	{
		// TODO: Calculate the next cursor position on the same line.
		throw new NotImplementedException();
	}
}
