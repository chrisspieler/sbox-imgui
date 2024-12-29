using Duccsoft.ImGui.Rendering;
using System.Collections.Generic;
namespace Duccsoft.ImGui.Elements;

public abstract class Element
{
	protected Element( Element parent )
	{
		Parent = parent;
		if ( Parent is null )
		{
			Window = this as Window;
		}
		else
		{
			Window = Parent.Window;
		}

		if ( Id == 0 )
		{
			var typeId = GetType().GetHashCode();
			Id = ImGui.GetID( typeId );
		}
	}

	internal static ImGuiSystem System => ImGuiSystem.Current;

	public int Id { get; init; }

	#region Layout
	public Element Parent { get; init; }
	public Window Window { get; init; }

	public IReadOnlyList<Element> Children => _children;
	protected List<Element> _children = new();
	/// <summary>
	/// Returns the element currently being built as a child of this element, or null if 
	/// no child is currently being built.
	/// </summary>
	internal Element CurrentItem { get; set; }
	internal Element CurrentItemRecursive
	{
		get
		{
			if ( CurrentItem is not null )
				return CurrentItem.CurrentItemRecursive;

			if ( Parent is null )
				return null;

			return IsEnded ? null : this;
		}
	}
	/// <summary>
	/// Returns the last child that has already been built, or null if no child has been built.
	/// </summary>
	internal Element LastItem
	{
		get
		{
			var lastChildIdx = Children.Count - 1;
			if ( lastChildIdx < 0 )
				return null;

			if ( Children[lastChildIdx].IsEnded )
				return Children[lastChildIdx];

			if ( lastChildIdx < 1 )
				return null;

			return Children[lastChildIdx - 1];
		}
	}
	internal Element LastItemRecursive
	{
		get
		{
			// If our last item is still being built...
			if ( CurrentItem is not null )
			{
				var currentLast = CurrentItem.LastItemRecursive;
				// ...and it has built an item of its own, return that item.
				if ( currentLast is not null )
					return currentLast;
			}

			if ( LastItem is not null )
				return LastItem.LastItemRecursive;

			return IsEnded ? this : null;
		}
	}
	internal bool IsEnded { get; private set; }

	internal Element FindAncestor( int id )
	{
		if ( Parent is null )
			return null;

		if ( Parent.Id == id )
			return Parent;

		return Parent.FindAncestor( id );
	}

	internal Element FindDescendant( int id )
	{
		if ( _children.Count < 1 )
			return null;

		foreach( var child in _children )
		{
			if ( child.Id == id )
				return child;
		}
		foreach( var child in _children )
		{
			var found = child.FindDescendant( id );
			if ( found is not null )
				return found;
		}
		return null;
	}

	public bool IsAncestor( Element element )
	{
		if ( element is null || Parent is null || element == this )
			return false;

		if ( Parent == element )
			return true;

		return Parent.IsAncestor( element );
	}

	public bool IsDescendant( Element element )
	{
		if ( element is null || element == this )
			return false;

		return element.IsAncestor( this );
	}
	#endregion

	#region Transform
	public Vector2 Pivot { get; set; }
	public Vector2 Position 
	{ 
		get
		{
			var pos = _position;
			if ( IsDragged )
			{
				pos += MouseState.LeftClickDragTotalDelta;
			}
			return pos;
		}
		set
		{
			var pos = value;
			pos -= Size * Pivot;
			_position = pos;
		}
	}
	private Vector2 _position;
	public virtual Vector2 ScreenPosition
	{
		get
		{
			if ( Parent is null )
				return Position;

			return Parent.ScreenPosition + Position;
		}
	}
	public Vector2 Padding { get; set; }
	public virtual Vector2 Size
	{
		get
		{
			if ( !CustomSize.IsNearZeroLength )
				return CustomSize;

			var padding = ImGui.GetStyle().WindowPadding;
			var size = ContentSize + padding * 2f;
			return size;
		}
		set => CustomSize = value;
	}
	public Vector2 CustomSize { get; set; }
	public Rect ScreenRect => new( ScreenPosition, Size );
	public Vector2 ContentSize { get; set; }
	public Rect ContentScreenRect => new( ScreenRect.Position + Padding, ContentSize );
	#endregion

	#region Input
	public bool IsActive => System.ClickedElementId == Id;
	public bool IsFocused 
	{ 
		get
		{
			return System.FocusedWindowId == Id;
		}
		set
		{
			System.Focus( value ? this : null );
		}
	}
	public bool IsHovered { get; set; }
	public bool IsDragged { get; set; }
	public bool IsReleased { get; set; }
	#endregion

	#region History
	public ElementFlags PreviousInputState { get; set; }
	public bool IsAppearing { get; set; }
	public bool IsVisible { get; set; }
	#endregion

	#region Style
	public ImGuiStyle Style => ImGui.GetStyle();
	public Color32 TextColor => ImGui.GetColorU32( ImGuiCol.Text );
	public Color32 FrameColor
	{
		get
		{
			if ( IsActive )
			{
				return ImGui.GetColorU32( ImGuiCol.FrameBgActive );
			}
			else if ( IsHovered )
			{
				return ImGui.GetColorU32( ImGuiCol.FrameBgHovered );
			}
			else
			{
				return ImGui.GetColorU32( ImGuiCol.FrameBg );
			}
		}
	}
	#endregion

	/// <summary>
	/// Add this element to its Parent, updates its input data, and calls ImGui.NewLine.
	/// <br/><br/>
	/// For any subclass of Element, OnBegin should be called within its constructor, or
	/// immediately after the element is constructed.
	/// </summary>
	public virtual void OnBegin()
	{

		Parent?.AddChild( this );
		PreviousInputState = System.PreviousBoundsList.GetElementFlags( Id );
		IsAppearing = !System.PreviousBoundsList.HasId( Id );
		IsVisible = PreviousInputState.IsVisible();
		IsReleased = PreviousInputState.IsHovered() && PreviousInputState.HasFlag( ElementFlags.IsActive ) && MouseState.LeftClickReleased;
	}

	/// <summary>
	/// For any subclass of Element that contains no children, OnEnd should be called
	/// immediately after OnBegin.
	/// </summary>
	public virtual void OnEnd()
	{
		// Only after all children are added will we know what this item's bounds are.
		OnUpdateInput();
		if ( Parent is not null )
		{
			Parent.CurrentItem = null;
		}
		IsEnded = true;
		ImGui.NewLine();
	}

	// TODO: Replace this with AddToParent
	public virtual void AddChild( Element child )
	{
		if ( child is null || child.Parent != this )
			return;

		CurrentItem = child;
		child.Position = ImGui.GetCursorScreenPos() - ScreenPosition;
		_children.Add( child );
		var spacing = ImGui.GetStyle().ItemSpacing.y;
		var maxs = Window.CursorPosition + child.Size + new Vector2( 0f, spacing );
		ContentSize = ContentSize.ComponentMax( maxs );
	}

	public virtual void OnUpdateInput()
	{
		IsHovered = false;

		if ( Window is not null && System.PreviousHoveredWindowId != Window.Id )
			return;

		if ( !IsVisible )
			return;

		if ( !ScreenRect.IsInside( MouseState.Position ) )
			return;

		IsHovered = true;
		if ( MouseState.LeftClickPressed )
		{
			OnClick( MouseState.Position );
		}
	}

	public virtual void OnClick( Vector2 screenPos )
	{
		if ( System.ClickedElementId.HasValue )
		{
			// Items won't be indexed in the BoundsList until the containing window is built.
			var clickedDescendant = FindDescendant( System.ClickedElementId.Value );
			// Within a frame, clicks should prioritize descendants over ancestors.
			if ( clickedDescendant is not null )
				return;
		}

		System.ClickedElementId = Id;
		System.Focus( this );
	}

	protected virtual void OnDrawSelf( ImDrawList drawList ) { }

	public void Draw( ImDrawList drawList )
	{
		OnDrawSelf( drawList );
		foreach( var child in Children )
		{
			child.Draw( drawList );
		}
	}

	public override string ToString()
	{
		return $"({GetType().Name} # {Id})";
	}
}
