using System.Collections.Generic;
using System.Linq;

namespace Duccsoft.ImGui;

/// <summary>
/// Represents every widget that is ready to be drawn in a frame.
/// </summary>
internal class BoundsList
{
	public IEnumerable<Window> Windows => WindowIds.Values;
	public Dictionary<int, Window> WindowIds { get; set; } = new();
	public IEnumerable<Widget> Widgets => WidgetIds.Values;
	public Dictionary<int, Widget> WidgetIds { get; set; } = new();

	public Widget this[int id] => WidgetIds[id];

	public void AddWindow( Window window )
	{
		if ( window is null )
			return;

		WindowIds[window.Id] = window;
	}

	public void AddWidget( Widget widget )
	{
		if ( widget is null )
			return;

		WidgetIds[widget.Id] = widget;
	}

	public bool IsVisible( int id )
	{
		if ( !WidgetIds.TryGetValue( id, out var widget ) )
			return false;

		return IsVisible( widget );
	}

	public bool IsVisible( Widget widget )
	{
		if ( widget is null || !WidgetIds.ContainsKey( widget.Id ) )
			return false;

		// The last window in the DrawList is the topmost window.
		foreach ( var window in WindowIds.Values.Reverse() )
		{
			if ( widget.Parent == window )
			{
				// We've reached this widget's window, and the widget hasn't been covered so far.
				return true;
			}

			var widgetBounds = widget.ScreenRect;

			// Check whether a window rendered later than this widget has fully obscured this widget.
			if ( window.ScreenRect.IsInside( widgetBounds, fullyInside: true ) )
			{
				return false;
			}
		}
		// Somehow, this widget has no parent window.
		return false;
	}
}
