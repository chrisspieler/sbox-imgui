using Sandbox.UI;

namespace Duccsoft.ImGui;

internal class PassthroughPanel : RootPanel
{
	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime )
			return;

		AcceptsFocus = true;
		Focus();
	}

	public override void Tick()
	{
		if ( !Game.IsPlaying )
			Delete();
	}

	public delegate void ClickEvent( bool pressed );
	public ClickEvent LeftClick { get; set; }
	public ClickEvent RightClick { get; set; }
	public ClickEvent MiddleClick { get; set; }

	public delegate void KeystrokeEvent( string button, KeyboardModifiers modifiers );
	public KeystrokeEvent Keystroke { get; set; }

	public override void OnButtonEvent( ButtonEvent e )
	{
		switch ( e.Button )
		{
			case "mouseleft":
				LeftClick?.Invoke( e.Pressed );
				break;
			case "mouseright":
				RightClick?.Invoke( e.Pressed );
				break;
			case "mousemiddle":
				MiddleClick?.Invoke( e.Pressed );
				break;
			default:
				break;
		}
		// TODO: Implement io.WantCaptureMouse, somehow.
	}

	public override void OnButtonTyped( ButtonEvent e )
	{
		Keystroke?.Invoke( e.Button, e.KeyboardModifiers );
	}
}
