namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static void Text( string formatString, params object[] args )
	{
		var text = string.Format( formatString, args );
		var widget = new TextWidget( text );
		ImGuiSystem.Current.PushWidget( widget );
	}
}
