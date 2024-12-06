using Sandbox.UI;
using System;

namespace Duccsoft.ImGui;

internal class PassthroughPanel : RootPanel
{
	public override void Tick()
	{
		if ( !Game.IsPlaying )
			Delete();
	}

	public Action<bool> LeftClick { get; set; }
	public Action<bool> RightClick { get; set; }
	public Action<bool> MiddleClick { get; set; }

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
	}
}
