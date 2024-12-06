using System;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	public DrawList CurrentDrawList { get; private set; } = new();
	// Keep the previous frame's draw list so that we can check whether a widget
	// drawn before a window was occluded by that window on the previous frame.
	public DrawList PreviousDrawList { get; set; } = new();
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
		PreviousDrawList = CurrentDrawList;
		CurrentDrawList = new();
		WindowStack.Clear();
	}

	public void BeginWindow( string name, Action onClose, ImGuiWindowFlags flags )
	{
		NextWindow.Name = name;
		WindowStack.Push( NextWindow );
		FocusedWindow ??= NextWindow.Name;
		CursorScreenPosition = NextWindow.GetContentScreenPosition();
		NextWindow = new();
	}

	public void AddWidget( Widget widget )
	{
		CursorScreenPosition = CurrentWindow.AddChild( widget );
		CurrentDrawList.AddWidget( widget );
	}

	public void EndWindow()
	{
		var popped = WindowStack.Pop();
		if ( WindowStack.Count > 0 && popped.Name == FocusedWindow )
		{
			// TODO: Create a focus stack separate from draw order.
			FocusedWindow = WindowStack.Peek().Name;
		}
		CurrentDrawList.AddWindow( popped );
	}
}
