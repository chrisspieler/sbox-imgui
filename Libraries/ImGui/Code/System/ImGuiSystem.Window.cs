using System;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	public BoundsList CurrentBoundsList { get; private set; } = new();
	// Keep the previous frame's bounds list so that we can check whether a widget
	// drawn before a window was occluded by that window on the previous frame.
	public BoundsList PreviousBoundsList { get; set; } = new();
	public Stack<Window> WindowStack { get; private init; } = new();
	public IdStack IdStack { get; private init; } = new();
	public Dictionary<int, Vector2> CustomWindowPositions { get; private set; } = new();

	public Window CurrentWindow
	{
		get
		{
			if ( WindowStack.Count < 1 )
				return null;

			return WindowStack.Peek();
		}
	}
	public int CurrentWindowCount => CurrentBoundsList.WindowIds.Values.Count;

	public Vector2 NextWindowPosition { get; set; }
	public Vector2 NextWindowPivot { get; set; }
	public Vector2 NextWindowSize { get; set; }
	public bool ShouldFocusNextWindow { get; set; } = false;
	public int? FocusedWindowId { get; private set; }
	public Window FocusedWindow
	{
		get 
		{
			if ( !FocusedWindowId.HasValue )
				return null;

			return Window.Get( FocusedWindowId.Value );
		}
	}

	public int ClickedWidgetId { get; set; }
	public int CurrentWidgetCount => CurrentBoundsList.WidgetIds.Values.Count;
	public Widget CurrentWidget => CurrentWindow?.CurrentWidget;

	public Window GetPreviousWindow( int id )
	{
		PreviousBoundsList.WindowIds.TryGetValue( id, out var previous );
		return previous;
	}

	public Widget GetPreviousWidget( int id )
	{
		PreviousBoundsList.WidgetIds.TryGetValue( id, out var previous );
		return previous;
	}

	public bool IsWindowAppearing( int id )
	{
		return GetPreviousWindow( id ) is null;
	}

	public bool IsWindowFocused( int id, ImGuiFocusedFlags flags = default )
	{
		if ( FocusedWindowId is null )
			return false;

		return FocusedWindowId == id;
	}

	public bool IsWindowHovered( int id, ImGuiHoveredFlags flags = default )
	{
		if ( HoveredWindow is null )
			return false;

		return HoveredWindow.Id == id;
	}

	public void Focus( Window window )
	{
		FocusedWindowId = window?.Id;
	}

	public void ClearWindows()
	{
		PreviousBoundsList = CurrentBoundsList;
		CurrentBoundsList = new();
		WindowStack.Clear();
	}

	public void BeginWindow( string name, Action onClose, ImGuiWindowFlags flags )
	{
		if ( CustomWindowPositions.TryGetValue( ImGui.GetID( name ), out var customPos ) )
		{
			NextWindowPosition = customPos;
		}
		var next = new Window( name, NextWindowPosition, NextWindowPivot, NextWindowSize, flags );
		if ( ShouldFocusNextWindow )
		{
			Focus( next );
		}
		if ( next.IsAppearing && !next.Flags.HasFlag( ImGuiWindowFlags.NoFocusOnAppearing ) )
		{
			Focus( next );
		}
		ResetNextWindowSettings();
	}

	private void ResetNextWindowSettings()
	{
		NextWindowPosition = default;
		NextWindowPivot = default;
		NextWindowSize = default;
		ShouldFocusNextWindow = false;
	}

	public void AddWidget( Window window, Widget widget )
	{
		window.AddChild( widget );
		CurrentBoundsList.AddWidget( widget );
		ImGui.NewLine();
	}

	public void EndWindow()
	{
		var popped = WindowStack.Pop();
		IdStack.Pop();
		if ( WindowStack.Count > 0 && popped.Id == FocusedWindowId )
		{
			// TODO: Create a focus stack separate from draw order.
			FocusedWindowId = WindowStack.Peek().Id;
		}
		CurrentBoundsList.AddWindow( popped );
	}
}
