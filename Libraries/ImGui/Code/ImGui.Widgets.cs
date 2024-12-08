using System;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	private static Widget CurrentWidget => CurrentWindow?.CurrentWidget;

	public static bool IsItemClicked( ImGuiMouseButton button = ImGuiMouseButton.Left )
	{
		return CurrentWidget.IsClicked( button );
	}

	public static void Text( string formatString, params object[] args )
	{
		var text = string.Format( formatString, args );
		_ = new TextWidget( CurrentWindow, text );
	}

	public static bool Button( string label, Vector2 size = default )
	{
		var button = new ButtonWidget( CurrentWindow, label );
		return button.IsReleased;
	}

	public static bool SliderFloat( string label, Func<float> getter, Action<float> setter, float min, float max, string format = null, ImGuiSliderFlags flags = default )
	{
		_ = new SliderFloat( CurrentWindow, label, getter, setter, min, max, format );
		return true;
	}


	public static void Image( Texture texture, Vector2 size, Color tintColor, Color borderColor )
	{
		Image( texture, size, Vector2.Zero, Vector2.One, tintColor, borderColor );
	}

	public static void Image( Texture texture, Vector2 size, Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor )
	{
		_ = new ImageWidget( CurrentWindow, texture, size, uv0, uv1, tintColor, borderColor );
	}
}
