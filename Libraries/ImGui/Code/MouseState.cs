namespace Duccsoft.ImGui;

internal struct MouseState
{
	public Vector2 Position { get; init; }
	public bool LeftClickPressed { get; init; }
	public bool LeftClickDown { get; init; }
	public bool LeftClickReleased { get; init; }
	public bool RightClickPressed { get; init; }
	public bool RightClickDown { get; init; }
	public bool RightClickReleased { get; init; }
	public bool MiddleClickPressed { get; init; }
	public bool MiddleClickDown { get; init; }
	public bool MiddleClickReleased { get; init; }
}
