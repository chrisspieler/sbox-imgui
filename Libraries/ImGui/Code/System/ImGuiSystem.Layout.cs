using Duccsoft.ImGui.Elements;
using Duccsoft.ImGui.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	public BoundsList CurrentBoundsList { get; private set; } = new();
	// Keep the previous frame's bounds list so that we can check whether a widget
	// drawn before a window was occluded by that window on the previous frame.
	public BoundsList PreviousBoundsList { get; set; } = new();
	public int? PreviousHoveredWindowId { get; set; }
	public int? PreviousHoveredElementId { get; set; }

	public Stack<Window> WindowStack { get; private init; } = new();
	private Dictionary<int, ImDrawList> DrawLists { get; set; } = new();
	public IdStack IdStack { get; private init; } = new();
	private Dictionary<int, Element> CurrentElements { get; set; } = new();

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

	public Vector2 NextWindowPosition { get; set; }
	public Vector2 NextWindowPivot { get; set; }
	public Vector2 NextWindowSize { get; set; }
	public bool ShouldFocusNextWindow { get; set; } = false;

	public int? FocusedWindowId { get; private set; }
	public int? FocusedElementId { get; private set; }
	public int? ClickedElementId { get; set; }

	private void AddElement( Element element )
	{
		// Items will share an ID in some common cases.
		if ( CurrentElements.ContainsKey( element.Id ) )
			return;
		
		CurrentElements.Add( element.Id, element );
		CurrentBoundsList.AddElement( element, element.Parent );
	}

	internal Element GetElement( int id )
	{
		CurrentElements.TryGetValue( id, out var element );
		return element;
	}

	public void Focus( Element element )
	{
		FocusedWindowId = element?.Window?.Id;
		FocusedElementId = element?.Id;
	}

	private void SwapBounds()
	{
		PreviousBoundsList = CurrentBoundsList;
		CurrentBoundsList = new();
		PreviousHoveredWindowId = PreviousBoundsList.TraceWindow( MouseState.Position );
		PreviousHoveredElementId = PreviousBoundsList.TraceElement( MouseState.Position );
		// Log.Info( $"Hovered window: {PreviousHoveredWindowId}" );
		// Log.Info( $"Hovered element: {PreviousHoveredElementId}" );
	}

	private void ClearElements()
	{
		CurrentElements.Clear();
		WindowStack.Clear();
		IdStack.Clear();
	}
	
	private void FinalizeBounds()
	{
		CurrentBoundsList.ApplyElementFlags();
		CurrentBoundsList.SortWindows();
	}

	public void BeginWindow( string name, ref bool open, ImGuiWindowFlags flags )
	{
		// TODO: Move this method in to Window.Begin()

		if ( CustomWindowPositions.TryGetValue( ImGui.GetID( name ), out var customPos ) )
		{
			NextWindowPosition = customPos;
		}
		var nextWindow = new Window( name, ref open, NextWindowPosition, NextWindowPivot, NextWindowSize, flags );
		if ( !flags.HasFlag( ImGuiWindowFlags.NoTitleBar ) )
		{
			nextWindow.CursorPosition = Vector2.Zero;
			nextWindow.TitleBar = new WindowTitleBar( nextWindow );
			nextWindow.CursorPosition += Style.WindowPadding;
			nextWindow.CursorStartPosition = nextWindow.CursorPosition;
		}
		if ( ShouldFocusNextWindow )
		{
			Focus( nextWindow );
		}
		if ( nextWindow.IsAppearing && !nextWindow.WindowFlags.HasFlag( ImGuiWindowFlags.NoFocusOnAppearing ) )
		{
			Focus( nextWindow );
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

	
	public void EndWindow()
	{
		// TODO: Move this method in to Window.End()

		var popped = WindowStack.Pop();
		IdStack.Pop();
		popped.OnEnd();
		if ( WindowStack.Count > 0 && popped.Id == FocusedWindowId )
		{
			// TODO: Create a focus stack separate from draw order.
			FocusedWindowId = WindowStack.Peek().Id;
		}
		AddElementRecursive( popped );
	}

	private void AddElementRecursive( Element element )
	{
		AddElement( element );
		foreach( var child in element.Children )
		{
			AddElementRecursive( child );
		}
	}
}
