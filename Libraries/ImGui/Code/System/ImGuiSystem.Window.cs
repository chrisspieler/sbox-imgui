using System;
using System.Collections.Generic;
using System.Linq;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	public DrawList CurrentDrawList { get; private set; } = new();
	// Keep the previous frame's draw list so that we can check whether a widget
	// drawn before a window was occluded by that window on the previous frame.
	public DrawList PreviousDrawList { get; set; } = new();
	private Stack<Window> WindowStack { get; init; } = new();
	public IdStack IdStack { get; set; } = new();

	public Window CurrentWindow
	{
		get
		{
			if ( WindowStack.Count < 1 )
				return null;

			return WindowStack.Peek();
		}
	}
	public int CurrentWindowCount => CurrentDrawList.WindowIds.Values.Count;

	public Vector2 NextWindowPosition { get; set; }
	public Vector2 NextWindowPivot { get; set; }
	public Vector2 NextWindowSize { get; set; }
	public bool ShouldFocusNextWindow { get; set; } = false;
	public int? FocusedWindowId { get; private set; }
	public int ClickedWidgetId { get; set; }
	public int CurrentWidgetCount => CurrentDrawList.WidgetIds.Values.Count;
	public Widget CurrentWidget => CurrentWindow?.CurrentWidget;

	public Window GetPreviousWindow( int id )
	{
		PreviousDrawList.WindowIds.TryGetValue( id, out var previous );
		return previous;
	}

	public Widget GetPreviousWidget( int id )
	{
		PreviousDrawList.WidgetIds.TryGetValue( id, out var previous );
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
		PreviousDrawList = CurrentDrawList;
		CurrentDrawList = new();
		WindowStack.Clear();
	}

	public void BeginWindow( string name, Action onClose, ImGuiWindowFlags flags )
	{
		var next = new Window( name, NextWindowPosition, NextWindowPivot, NextWindowSize, flags );
		IdStack.Push( next.Id );
		WindowStack.Push( next );
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
		var widgetRect = window.AddChild( widget );
		// TODO: Add SameLine support.
		var cursorOffset = new Vector2( 0f, widgetRect.Size.y + Style.ItemSpacing.y );
		CursorScreenPosition = widgetRect.Position + cursorOffset;
		CurrentDrawList.AddWidget( widget );
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
		CurrentDrawList.AddWindow( popped );
	}
}
