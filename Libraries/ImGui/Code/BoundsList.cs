using Duccsoft.ImGui.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Duccsoft.ImGui;

/// <summary>
/// Represents every widget that is ready to be drawn in a frame.
/// </summary>
internal class BoundsList
{
	private static ImGuiSystem System => ImGuiSystem.Current;

	internal class BoundsElement
	{
		public BoundsElement( int id, BoundsElement parent, ElementFlags inputState, Rect screenBounds, List<int> children = null )
		{
			Id = id;
			Parent = parent;
			ElementFlags = inputState;
			ScreenBounds = screenBounds;
			Children = children;
		}

		public bool IsWindow => Parent is null;
		public int Id { get; set; }
		public BoundsElement Parent { get; set; }
		public List<int> Children { get; set; }
		public ElementFlags ElementFlags { get; set; }
		public Rect ScreenBounds { get; set; }

		public bool IsHovered => ElementFlags.IsHovered();
		public bool IsFocused => ElementFlags.IsFocused();
		public bool IsActive => ElementFlags.IsActive();
		public bool IsDragged => ElementFlags.IsDragged();
		public bool IsVisible => ElementFlags.IsVisible();

		public bool IsAncestor( BoundsElement element )
		{
			if ( element is null || Parent is null )
				return false;

			if ( Parent == element )
				return true;

			return Parent.IsAncestor( element );
		}

		public static BoundsElement From( Element element, BoundsElement parent )
		{
			if ( element is null )
				return null;

			var children = element
				.Children
				.Select( c => c.Id )
				.ToList();
			return new( element.Id, parent, default, element.ScreenRect, children );
		}
	}

	private Dictionary<int, BoundsElement> Elements { get; set; } = new();
	private List<BoundsElement> RootElements { get; set; } = new();
	public IEnumerable<BoundsElement> GetRootElements() => RootElements;

	public bool HasId( int id ) => Elements.ContainsKey( id );

	public void AddElement( Element element, Element parent )
	{
		ArgumentNullException.ThrowIfNull( element );

		BoundsElement parentBounds = null;
		if ( parent is not null )
		{
			if ( !Elements.TryGetValue( parent.Id, out parentBounds ) )
				throw new InvalidOperationException( $"Attempted to add child ({element.GetType().Name},{element.Id}) of parent ({parent.GetType().Name},{parent.Id}) before parent was added to BoundsList." );
		}

		AddElement( element, parentBounds );
	}

	private void AddElement( Element element, BoundsElement parent )
	{
		var boundsElement = BoundsElement.From( element, parent );
		if ( parent is null )
		{
			RootElements.Add( boundsElement );
		}
		Elements.Add( element.Id, boundsElement );
	}

	public BoundsElement GetElement( int id )
	{
		Elements.TryGetValue( id, out var element );
		return element;
	}

	public bool TryGetElement( int id, out BoundsElement element ) => Elements.TryGetValue( id, out element );

	public ElementFlags GetElementFlags( int id )
	{
		Elements.TryGetValue( id, out var element );
		if ( element is null )
			return default;

		return element.ElementFlags;
	}

	public bool IsVisible( int id )
	{
		if ( !Elements.TryGetValue( id, out var bounds ) )
			return false;

		if ( bounds.IsWindow )
			return IsWindowVisible( bounds );

		foreach( var window in RootElements )
		{
			// If we are contained within the topmost window, are automatically visible.
			if ( bounds.IsAncestor( window ) )
				return true;

			if ( window.ScreenBounds.IsInside( bounds.ScreenBounds, true ) )
			{
				return false;
			}
		}
		return false;
	}

	public int? TraceElement( Vector2 screenPos )
	{
		var windowId = TraceWindow( screenPos );
		if ( windowId is null )
			return null;

		var windowElement = Elements[windowId.Value];
		return Trace( screenPos, windowElement );
	}

	public int? TraceWindow( Vector2 screenPos )
	{
		for ( int i = RootElements.Count - 1; i > -1; i-- )
		{
			var window = RootElements[i];
			if ( window.ScreenBounds.IsInside( screenPos ) )
				return window.Id;
		}
		return null;
	}

	private int? Trace( Vector2 screenPosition, BoundsElement element )
	{
		if ( !element.ScreenBounds.IsInside( screenPosition ) )
			return null;

		foreach( var childId in element.Children )
		{
			var childElement = Elements[childId];
			var result = Trace( screenPosition, childElement );
			if ( result is not null )
				return result;
		}
		// No children contain this point, so this element is the one we hit.
		return element.Id;
	}

	private bool IsWindowVisible( BoundsElement bounds )
	{
		foreach ( var otherWindow in RootElements )
		{
			if ( bounds.Id == otherWindow.Id )
				continue;

			if ( otherWindow.ScreenBounds.IsInside( bounds.ScreenBounds, true ) )
				return false;
		}
		return true;
	}

	private void CascadeElementFlag( int id, ElementFlags elementFlag, bool enabled )
	{
		var element = Elements[id];
		SetElementFlag( id, elementFlag, enabled );
		var parent = element.Parent;
		// Don't cascade flag disables
		// Why? Consider the case where an item is no longer hovered, but its window still is hovered.]
		if ( enabled && parent is not null )
		{
			// Cascade flag enables. E.g., if we click on an item, its window should be hovered.
			CascadeElementFlag( parent.Id, elementFlag, true );
		}
	}

	private void SetElementFlag( int id, ElementFlags elementFlag, bool enabled )
	{
		var element = Elements[id];
		if ( enabled )
		{
			element.ElementFlags |= elementFlag;
		}
		else
		{
			element.ElementFlags &= ~elementFlag;
		}
	}

	public void SortWindows()
	{
		for ( int i = 0; i < RootElements.Count; i++ )
		{
			var lastIdx = RootElements.Count - 1;
			// If we've reached the end of the list, either there is no focused window, or it's already on top.
			if ( i == lastIdx )
				continue;

			var window = RootElements[i];
			if ( window.IsFocused )
			{
				var temp = RootElements[lastIdx];
				RootElements[lastIdx] = window;
				RootElements[i] = temp;
				return;
			}
		}
		return;
		//Log.Info( $"Printing Window Order" );
		//for ( int i = 0; i < RootElements.Count; i++ )
		//{
		//	Log.Info( $"{i}: {RootElements[i].Id}" );
		//}
	}

	public void ApplyElementFlags()
	{
		foreach( var window in RootElements )
		{
			var current = System.GetElement( window.Id );
			var isFocused = window.Id == System.FocusedWindowId;
			SetElementFlag( window.Id, ElementFlags.IsFocused, isFocused );
			SetElementFlag( window.Id, ElementFlags.IsHovered, current.IsHovered );
			SetElementFlag( window.Id, ElementFlags.IsVisible, IsVisible( window.Id ) );
		}
		foreach( var element in Elements.Values )
		{
			if ( element.IsWindow )
				continue;

			var current = System.GetElement(element.Id);
			CascadeElementFlag( current.Id, ElementFlags.IsHovered, current.IsHovered );
			SetElementFlag( current.Id, ElementFlags.IsFocused, current.IsFocused );
			SetElementFlag( current.Id, ElementFlags.IsActive, current.IsActive );
			SetElementFlag( current.Id, ElementFlags.IsDragged, current.IsDragged );
			SetElementFlag( current.Id, ElementFlags.IsVisible, IsVisible( current.Id ) );
		}
	}
}
