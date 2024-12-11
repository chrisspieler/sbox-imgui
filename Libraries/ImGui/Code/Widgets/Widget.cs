using Duccsoft.ImGui.Rendering;

namespace Duccsoft.ImGui;

internal abstract class Widget : IUniqueId
{
	protected Widget( Window parent )
	{
		Parent = parent;
		var typeId = GetType().GetHashCode();
		Id = ImGui.GetID( typeId );
	}

	public int Id { get; init; }
	public ImGuiStyle Style => ImGui.GetStyle();
	public bool IsActive => ImGuiSystem.Current.ClickedWidgetId == Id;
	public bool IsHovered { get; set; }
	public Window Parent { get; set; }
	/// <summary>
	/// The position of the widget relative to its parent.
	/// </summary>
	public Vector2 Position { get; set; }
	public Vector2 ScreenPosition => Parent.ScreenRect.Position + Position;

	#region History
	// Because we don't know the layout of the screen until after everything has been drawn,
	// we check the history of this widget to determine its likely state while building the UI.

	public Widget Previous => ImGuiSystem.Current.GetPreviousWidget( Id );
	public bool WasVisible => ImGuiSystem.Current.PreviousBoundsList.IsVisible( Id );
	#endregion

	/// <summary>
	/// The size of the widget in pixels.
	/// </summary>
	public abstract Vector2 Size { get; }
	public Rect ScreenRect => new( ScreenPosition, Size );

	/// <summary>
	/// Adds the widget to the current draw list and sets its parent window.
	/// </summary>
	public void Show()
	{
		ImGuiSystem.Current.AddWidget( Parent, this );
		UpdateInput();
	}

	public abstract void Draw( ImDrawList drawList );

	public virtual void UpdateInput()
	{
		IsHovered = false;

		if ( Parent.Previous?.IsHovered != true )
			return;

		if ( !WasVisible )
			return;

		if ( !ScreenRect.IsInside( MouseState.Position ) )
			return;

		IsHovered = true;
		if ( MouseState.LeftClickPressed )
		{
			Click( MouseState.Position );
		}
	}

	public void Click( Vector2 screenPos )
	{
		ImGuiSystem.Current.ClickedWidgetId = Id;
	}
}

internal static class WidgetExtensions
{
	public static bool IsClicked( this Widget widget, ImGuiMouseButton button )
	{
		var currentButton = ImGuiSystem.Current.MouseButton;
		if ( currentButton is null || widget is null )
			return false;

		return ImGuiSystem.Current.ClickedWidgetId == widget.Id && currentButton == button;
	}
}
