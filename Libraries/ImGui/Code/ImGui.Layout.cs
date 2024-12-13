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

	public static void SetCursorPos( Vector2 localPos )
	{
		if ( CurrentWindow is null )
			return;

		CurrentWindow.CursorPosition = localPos;
	}

	public static void SetCursorPosX( float localX )
	{
		if ( CurrentWindow is null )
			return;

		CurrentWindow.CursorPosition = CurrentWindow.CursorPosition.WithX( localX );
	}

	public static void SetCursorPosY( float localY )
	{
		if ( CurrentWindow is null )
			return;

		CurrentWindow.CursorPosition = CurrentWindow.CursorPosition.WithY( localY );
	}

	public static Vector2 GetCursorStartPos()
	{
		if ( CurrentWindow is null )
			return default;

		return CurrentWindow.CursorStartPosition;
	}

	public static void NewLine()
	{
		if ( CurrentItem is null )
			return;

		var yOffset = CurrentItem.Position.y + CurrentItem.Size.y + GetStyle().ItemSpacing.y;
		SetCursorPos( new Vector2( GetCursorStartPos().x, yOffset ) );
	}

	public static void SameLine( float offsetFromStartX = 0f, float spacing = -1f )
	{
		if ( CurrentItem is null )
			return;


		if ( offsetFromStartX != 0 )
		{
			if ( spacing < 0f )
				spacing = 0f;

			var xPos = offsetFromStartX + spacing;
			var yPos = CurrentItem.Position.y;
			SetCursorPos( new Vector2( xPos, yPos ) );
		}
		else
		{
			if ( spacing < 0f )
				spacing = GetStyle().ItemSpacing.x;

			var cursorOffset = new Vector2( CurrentItem.Size.x + spacing, 0f );
			SetCursorPos( CurrentItem.Position + cursorOffset );
		}
	}
}
