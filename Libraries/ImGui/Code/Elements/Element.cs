using Duccsoft.ImGui.Rendering;
using System.Collections.Generic;
namespace Duccsoft.ImGui.Elements;

public abstract class Element
{
	protected Element( Element parent )
	{
		Parent = parent;
		if ( Id == 0 )
		{
			var typeId = GetType().GetHashCode();
			Id = ImGui.GetID( typeId );
		}
	}

	internal static ImGuiSystem System => ImGuiSystem.Current;

	public int Id { get; init; }

	#region Layout

	public Element Parent { get; set; }
	public Window Window
	{
		get
		{
			if ( Parent is null )
				return this as Window;

			return Parent.Window;
		}
	}

	public IReadOnlyList<Element> Children => _children;
	protected List<Element> _children = new();
	public Element CurrentItem
	{
		get
		{
			var lastChildIdx = Children.Count - 1;
			if ( lastChildIdx < 0 )
				return null;

			return Children[lastChildIdx];
		}
	}
	public Vector2 CursorStartPosition { get; set; }
	public Vector2 CursorPosition { get; set; }
	#endregion

	#region Transform
	public Vector2 Pivot { get; set; }
	public Vector2 Position { get; set; }
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
	#endregion

	#region History
	public ElementFlags PreviousInputState { get; set; }
	public bool IsAppearing { get; set; }
	public bool IsVisible { get; set; }
	#endregion

	/// <summary>
	/// Adds this element to the current BoundsList, sets its Parent, updates its input data, and calls ImGui.NewLine.
	/// <br/><br/>
	/// For any subclass of Element, Initialize should be called either at the end of the constructor, or
	/// immediately after the element is constructed.
	/// </summary>
	public virtual void Begin()
	{
		Parent?.AddChild( this );
		PreviousInputState = System.PreviousBoundsList.GetElementFlags( Id );
		IsAppearing = !System.PreviousBoundsList.HasId( Id );
		IsVisible = PreviousInputState.IsVisible();
		ImGui.NewLine();
	}

	public virtual void End()
	{
		// Only after all children are added will we know what this item's bounds are.
		UpdateInput();
	}

	public virtual void AddChild( Element child )
	{
		if ( child is null )
			return;

		child.Parent = this;
		child.Position = CursorPosition;
		_children.Add( child );
		var spacing = ImGui.GetStyle().ItemSpacing.y;
		var maxs = CursorPosition + child.Size + new Vector2( 0f, spacing );
		ContentSize = ContentSize.ComponentMax( maxs );
	}

	public virtual void UpdateInput()
	{
		IsHovered = false;

		if ( Parent is not null && !Parent.PreviousInputState.IsHovered() )
			return;

		if ( !IsVisible )
			return;

		if ( !ScreenRect.IsInside( MouseState.Position ) )
			return;

		IsHovered = true;
		if ( MouseState.LeftClickPressed )
		{
			Click( MouseState.Position );
		}
	}

	public virtual void Click( Vector2 screenPos )
	{
		ImGuiSystem.Current.ClickedElementId = Id;
		System.Focus( this );
		Log.Info( $"Clicked {GetType().Name} # {Id}" );
	}

	protected virtual void DrawSelf( ImDrawList drawList ) { }

	public void Draw( ImDrawList drawList )
	{
		DrawSelf( drawList );
		foreach( var child in Children )
		{
			child.Draw( drawList );
		}
	}
}
