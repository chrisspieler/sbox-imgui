using System;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

internal class Window
{
	public string Name 
	{
		get => _name;
		set
		{
			
			_name = value;
		}
	}
	private string _name;
	public Action OnClose { get; set; }

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
			var size = ContentScreenSize;
			size += style.WindowPadding * 2;
			// size += style.ItemSpacing * ( Children.Count - 1 );
			// Add title text size
			var titleTextSize = GetTitleTextSize();
			size.x = MathF.Max( titleTextSize.x * 2, size.x );
			size.y += titleTextSize.y;
			return size;
		}
	}
	public Vector2 CustomScreenSize { get; set; }
	public Rect ContentRect => new( GetContentScreenPosition(), ContentScreenSize );
	public Vector2 ContentScreenSize { get; private set; }

	#endregion

	#region Layout
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

	#region Style
	public static Color32 BackgroundColor => ImGui.GetColorU32( ImGuiCol.ImGuiCol_WindowBg );
	public static Color32 BorderColor => ImGui.GetColorU32( ImGuiCol.ImGuiCol_Border );
	public static Color32 TitleActiveColor => ImGui.GetColorU32( ImGuiCol.ImGuiCol_TitleBgActive );
	#endregion

	public Vector2 AddChild( Widget childWidget )
	{
		var widgetPosition = Children.Count == 0
			? GetContentScreenPosition()
			: ImGuiSystem.Current.CursorScreenPosition;
		childWidget.ParentWindow = this;
		childWidget.ScreenPosition = widgetPosition;
		Children.Add( childWidget );
		var size = childWidget.GetSize();
		var spacing = Children.Count > 1 ? ImGui.GetStyle().ItemSpacing.y : 0;
		ContentScreenSize = ContentScreenSize with
		{
			x = MathF.Max( size.x, ContentScreenSize.x ),
			y = ContentScreenSize.y + size.y + spacing,
		};
		return widgetPosition + new Vector2( 0f, size.y );
	}

	public Vector2 GetContentScreenPosition()
	{
		var position = ScreenRect.Position;
		// Account for title frame frame height.
		position += new Vector2( 0f, ImGui.GetFrameHeightWithSpacing() );
		position += ImGui.GetStyle().WindowPadding;
		return position;
	}

	private Rect GetTitleBarRect()
	{
		var textPanelSize = new Vector2( ScreenRect.Width, ImGui.GetFrameHeightWithSpacing() );
		return new Rect( ScreenRect.Position, textPanelSize );
	}

	private Vector2 GetTitleTextSize() => ImGui.CalcTextSize( Name ) + ImGui.GetStyle().FramePadding;

	public void Paint( ImGuiPainter painter )
	{
		// Paint background
		painter.DrawRect( ScreenRect, BackgroundColor, default, Vector4.One, BorderColor );

		PaintTitleFrame( painter );
		PaintChildren( painter );
	}

	private void PaintTitleFrame( ImGuiPainter painter )
	{
		var titleBarRect = GetTitleBarRect();

		// Paint title background
		painter.DrawRect( titleBarRect, TitleActiveColor );

		// Paint title
		var textPanelSize = GetTitleTextSize();
		var xTextOffset = titleBarRect.Size.x * 0.5f - textPanelSize.x * 0.5f;
		var yTextOffset = textPanelSize.y * 0.25f;
		var textPanelPos = titleBarRect.Position + new Vector2( xTextOffset, yTextOffset );
		var textRect = new Rect( textPanelPos, textPanelSize );
		painter.DrawText( Name, textRect, TextFlag.Center );
	}

	private void PaintChildren( ImGuiPainter painter )
	{
		foreach( var child in Children )
		{
			child.Paint( painter );
		}
	}
}
