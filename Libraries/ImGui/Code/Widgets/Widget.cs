namespace Duccsoft.ImGui;

internal abstract class Widget
{
	public Window ParentWindow { get; set; }
	public Vector2 ScreenPosition { get; set; }

	public abstract Vector2 GetSize();
	public Rect GetScreenBounds() => new( ScreenPosition, GetSize() );
	public abstract void Paint( ImGuiPainter painter );
}
