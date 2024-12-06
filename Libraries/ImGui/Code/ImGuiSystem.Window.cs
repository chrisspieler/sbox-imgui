using System;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	public List<Window> WindowDrawList { get; init; } = new();
	private Stack<Window> WindowStack { get; init; } = new();

	public Window CurrentWindow
	{
		get
		{
			if ( WindowStack.Count < 1 )
				return null;

			return WindowStack.Peek();
		}
	}

	public Window NextWindow { get; private set; } = new();
	public string FocusedWindow { get; private set; }
	public int ClickedWidget { get; set; }

	public void Focus( Window window )
	{
		FocusedWindow = window?.Name;
	}

	public void ClearWindows()
	{
		WindowDrawList.Clear();
		WindowStack.Clear();
	}

	public void PushWindow( string name, Action onClose, ImGuiWindowFlags flags )
	{
		NextWindow.Name = name;
		WindowStack.Push( NextWindow );
		FocusedWindow ??= NextWindow.Name;
		CursorScreenPosition = NextWindow.GetContentScreenPosition();
		NextWindow = new();
	}

	public void PushWidget( Widget widget )
	{
		CursorScreenPosition = CurrentWindow.AddChild( widget );
	}

	public void PopWindow()
	{
		var popped = WindowStack.Pop();
		if ( WindowStack.Count > 0 && popped.Name == FocusedWindow )
		{
			// TODO: Create a focus stack separate from draw order.
			FocusedWindow = WindowStack.Peek().Name;
		}
		WindowDrawList.Add( popped );
	}
}
