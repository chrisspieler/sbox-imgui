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
	public Vector2 ScreenPosition { get; set; }

	#region History
	// Because we don't know the layout of the screen until after everything has been drawn,
	// we check the history of this widget to determine its likely state while building the UI.

	public Widget Previous => ImGuiSystem.Current.GetPreviousWidget( Id );
	public bool WasVisible => ImGuiSystem.Current.PreviousDrawList.IsVisible( Id );
	#endregion

	public abstract Vector2 GetSize();
	public Rect GetScreenBounds() => new( ScreenPosition, GetSize() );

	/// <summary>
	/// Adds the widget to the current draw list and sets its parent window.
	/// </summary>
	public void Show()
	{
		ImGuiSystem.Current.AddWidget( Parent, this );
		UpdateInput( ImGuiSystem.Current.MouseState );
	}

	public abstract void Paint( ImGuiPainter painter );

	public virtual void UpdateInput( MouseState mouse )
	{
		IsHovered = false;

		if ( Parent.Previous?.IsHovered != true )
			return;

		if ( !WasVisible )
			return;

		if ( !GetScreenBounds().IsInside( mouse.Position ) )
			return;

		IsHovered = true;
		if ( mouse.LeftClickPressed )
		{
			Click( mouse.Position );
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
