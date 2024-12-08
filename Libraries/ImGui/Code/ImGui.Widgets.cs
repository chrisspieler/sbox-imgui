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
}
