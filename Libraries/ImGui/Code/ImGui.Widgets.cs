namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static void Text( string formatString, params object[] args )
	{
		var text = string.Format( formatString, args );
		_ = new TextWidget( text );
	}

	public static bool Button( string label, Vector2 size = default )
	{
		var widget = new ButtonWidget( label );
		widget.UpdateInput( Mouse );
		return widget.IsReleased;
	}
}
