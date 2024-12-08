using System;
using System.Collections.Generic;
using System.Linq;

namespace Duccsoft.ImGui;

internal class Window : IUniqueId
{
	public Window( string name, Vector2 screenPos, Vector2 pivot, Vector2 size, ImGuiWindowFlags flags )
	{
		Name = name;
		Id = ImGui.GetID( Name );
		Flags = flags;
		ScreenPosition = screenPos;
		Pivot = pivot;
		CustomScreenSize = size;
		ImGuiSystem.Current.IdStack.Push( Id );
		ImGuiSystem.Current.WindowStack.Push( this );
		if ( !flags.HasFlag( ImGuiWindowFlags.NoTitleBar ) )
		{
			var titleBar = new WindowTitleBar( this );
			CursorPosition += new Vector2( 0f, titleBar.GetSize().y );
		}
		CursorPosition += ImGui.GetStyle().WindowPadding;
	}

	public static Window Get( int id )
	{
		var drawList = ImGuiSystem.Current.CurrentDrawList;
		drawList.WindowIds.TryGetValue( id, out var window );
		return window;
	}

	public int Id { get; init; }
	public string Name { get; init; }
	public Window Previous => ImGuiSystem.Current.GetPreviousWindow( Id );
	public ImGuiWindowFlags Flags { get; init; }

	public Action OnClose { get; set; }

	public bool IsHovered { get; set; }
	public bool IsFocused 
	{
		get => ImGuiSystem.Current.FocusedWindowId == Id; 
		set
		{
			if ( value )
			{
				ImGuiSystem.Current.Focus( this );
			}
			else if ( ImGuiSystem.Current.FocusedWindowId == Id )
			{
				ImGuiSystem.Current.Focus( null );
			}
		}
	}
	public bool IsAppearing => Previous is null;
	public bool IsMouseInScreenRect
	{
		get
		{
			return ScreenRect.IsInside( MouseState.Position );
		}
	}
	public bool IsDragged { get; set; }

	#region Transform
	/// <summary>
	/// The position of the window in screenspace, without the pivot applied.
	/// </summary>
	public Vector2 ScreenPosition { get; set; }
	public Vector2 Pivot { get; set; }
	public Vector2 ScreenSize
	{
		get
		{
			if ( !CustomScreenSize.IsNearZeroLength )
			{
				return CustomScreenSize;
			}
			var style = ImGui.GetStyle();
			var size = ContentScreenSize + style.WindowPadding * 2;
			return size;
		}
	}
	public Vector2 CustomScreenSize { get; set; }
	public Rect ContentRect => new( ScreenRect.Position + Padding, ContentScreenSize );
	public Vector2 ContentScreenSize { get; private set; }
	public Vector2 CursorPosition { get; set; }
	#endregion

	#region Layout
	public Vector2 Padding => ImGui.GetStyle().WindowPadding;
	public List<Widget> Children { get; set; } = new();
	/// <summary>
	/// Returns the current widget that is not a part of the window itself.
	/// </summary>
	public Widget CurrentWidget
	{
		get
		{
			var lastChildIdx = Children.Count - 1;
			if ( lastChildIdx < 0 )
				return null;

			var lastChild = Children[lastChildIdx];
			if ( lastChild is WindowTitleBar )
				return null;

			return lastChild;
		}
	}
	public Rect ScreenRect
	{
		get
		{
			var pivotPx = ScreenSize * Pivot;
			var position = ScreenPosition - pivotPx;
			if ( IsDragged )
			{
				position += MouseState.LeftClickDragDelta;
			}
			return new Rect( position, ScreenSize );
		}
		set
		{
			CustomScreenSize = value.Size;
			var pivotPx = CustomScreenSize * Pivot;
			ScreenPosition = value.Position - pivotPx;
		}
	}
	#endregion

	public static Color32 BackgroundColor => ImGui.GetColorU32( ImGuiCol.WindowBg );
	public static Color32 BorderColor => ImGui.GetColorU32( ImGuiCol.Border );

	public void AddChild( Widget childWidget )
	{
		childWidget.Parent = this;
		childWidget.LocalPosition = CursorPosition;
		Children.Add( childWidget );
		var childRect = childWidget.ScreenRect;
		var size = childRect.Size;
		var spacing = Children.Count > 1 ? ImGui.GetStyle().ItemSpacing.y : 0;
		ContentScreenSize = ContentScreenSize with
		{
			x = MathF.Max( size.x, ContentScreenSize.x ),
			y = ContentScreenSize.y + size.y + spacing,
		};
	}

	public void Paint( ImGuiPainter painter )
	{
		// Paint background
		painter.DrawRect( ScreenRect, BackgroundColor, default, Vector4.One, BorderColor );

		PaintChildren( painter );
	}

	private void PaintChildren( ImGuiPainter painter )
	{
		foreach( var child in Children )
		{
			child.Paint( painter );
		}
	}

	public void UpdateInput()
	{
		
	}
}

/// <summary>
/// Some helper methods that also perform null checks.
/// </summary>
internal static class WindowExtensions
{
	public static bool IsAppearing( this Window window )
	{
		if ( window is null )
			return false;

		return ImGuiSystem.Current.IsWindowAppearing( window.Id );
	}

	public static bool IsFocused( this Window window, ImGuiFocusedFlags flags )
	{
		if ( window is null )
			return false;

		return ImGuiSystem.Current.IsWindowFocused( window.Id, flags );
	}

	public static bool IsHovered( this Window window, ImGuiHoveredFlags flags )
	{
		if ( window is null )
			return false;

		return ImGuiSystem.Current.IsWindowHovered( window.Id, flags );
	}
}
