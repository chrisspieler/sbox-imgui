using System;
using System.Collections.Generic;

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
		ImGuiSystem.Current.CursorScreenPosition = ScreenRect.Position;
		if ( !flags.HasFlag( ImGuiWindowFlags.ImGuiWindowFlags_NoTitleBar ) )
		{
			_ = new WindowTitleBar( this );
		}
		ImGuiSystem.Current.CursorScreenPosition += ImGui.GetStyle().WindowPadding;
	}

	public int Id { get; init; }
	public string Name { get; init; }
	public Window Previous => ImGuiSystem.Current.GetPreviousWindow( Id );
	public ImGuiWindowFlags Flags { get; init; }

	public Action OnClose { get; set; }

	public bool IsHovered { get; private set; }
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

	#endregion

	#region Layout
	public Vector2 Padding => ImGui.GetStyle().WindowPadding;
	public List<Widget> Children { get; set; } = new();
	public Rect ScreenRect
	{
		get
		{
			var pivotPx = ScreenSize * Pivot;
			var position = ScreenPosition - pivotPx;
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

	public static Color32 BackgroundColor => ImGui.GetColorU32( ImGuiCol.ImGuiCol_WindowBg );
	public static Color32 BorderColor => ImGui.GetColorU32( ImGuiCol.ImGuiCol_Border );

	public Rect AddChild( Widget childWidget )
	{
		childWidget.Parent = this;
		childWidget.ScreenPosition = ImGuiSystem.Current.CursorScreenPosition;
		Children.Add( childWidget );
		var childRect = childWidget.GetScreenBounds();
		var size = childRect.Size;
		var spacing = Children.Count > 1 ? ImGui.GetStyle().ItemSpacing.y : 0;
		ContentScreenSize = ContentScreenSize with
		{
			x = MathF.Max( size.x, ContentScreenSize.x ),
			y = ContentScreenSize.y + size.y + spacing,
		};
		return childRect;
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

	public void UpdateInput( MouseState mouse )
	{
		
	}
}
