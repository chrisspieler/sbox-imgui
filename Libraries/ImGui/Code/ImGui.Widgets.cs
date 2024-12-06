namespace Duccsoft.ImGui;

public static partial class ImGui
{
	private static Window CurrentWindow => ImGuiSystem.Current.CurrentWindow;

	public static void Text( string formatString, params object[] args )
	{
		var text = string.Format( formatString, args );
		_ = new TextWidget( CurrentWindow, text );
	}

	public static bool Button( string label, Vector2 size = default )
	{
		var widget = new ButtonWidget( CurrentWindow, label );
		widget.UpdateInput( Mouse );
		return widget.IsReleased;
	}
}
