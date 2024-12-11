namespace Duccsoft.ImGui;

internal static class MouseState
{
	public static Vector2 Position { get; set; }
	public static bool LeftClickPressed { get; set; }
	public static bool LeftClickDown { get; set; }
	public static bool LeftClickReleased { get; set; }
	public static Vector2 LeftClickPosition { get; set; }
	public static Vector2 LeftClickDragTotalDelta => Position - LeftClickPosition;
	public static Vector2 LeftClickDragFrameDelta { get; set; }
	public static bool RightClickPressed { get; set; }
	public static bool RightClickDown { get; set; }
	public static bool RightClickReleased { get; set; }
	public static Vector2 RightClickPosition { get; set; }
	public static Vector2 RightClickDragTotalDelta => Position - RightClickPosition;
	public static Vector2 RightClickDragFrameDelta { get; set; }
	public static bool MiddleClickPressed { get; set; }
	public static bool MiddleClickDown { get; set; }
	public static bool MiddleClickReleased { get; set; }
	public static Vector2 MiddleClickPosition { get; set; }
	public static Vector2 MiddleClickDragTotalDelta => Position - MiddleClickPosition;
	public static Vector2 MiddleClickDragFrameDelta { get; set; }
}
