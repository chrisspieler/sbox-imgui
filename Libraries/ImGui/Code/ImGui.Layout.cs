using static System.Net.Mime.MediaTypeNames;

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
		return new Vector2( text.Length * GetTextLineHeight() * ( Screen.Height / Screen.Width ), GetTextLineHeight() );
	}
}
