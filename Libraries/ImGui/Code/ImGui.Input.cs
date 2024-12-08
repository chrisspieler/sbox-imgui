namespace Duccsoft.ImGui;

public static partial class ImGui
{
	internal static MouseState Mouse => ImGuiSystem.Current.MouseState;

	public static Vector2 GetMousePos() => Mouse.Position;
}
