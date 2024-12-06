using System.Security.Cryptography.X509Certificates;

namespace Duccsoft.ImGui;

internal abstract class Widget
{
	public int Id { get; set; }
	public void Show() => ImGuiSystem.Current.AddWidget( this );
	public Window ParentWindow { get; set; }
	public Vector2 ScreenPosition { get; set; }
	public bool IsActive => ImGuiSystem.Current.ClickedWidget == Id;
	public bool IsHovered { get; set; }
	public bool IsVisible { get; set; }

	public abstract Vector2 GetSize();
	public Rect GetScreenBounds() => new( ScreenPosition, GetSize() );
	public abstract void Paint( ImGuiPainter painter );

	public virtual void UpdateInput( MouseState mouse )
	{
		IsHovered = false;

		var wasWindowHovered = false;
		var lastDrawList = ImGuiSystem.Current.PreviousDrawList;
		if ( lastDrawList.WindowIds.TryGetValue( ParentWindow.Id, out var lastWindow ) )
		{
			wasWindowHovered = lastWindow.IsHovered;
		}

		if ( !wasWindowHovered )
			return;

		if ( !lastDrawList.IsVisible( Id ) )
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
		ImGuiSystem.Current.ClickedWidget = Id;
	}
}
