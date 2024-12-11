namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static Vector2 GetMousePos() => MouseState.Position;
	public static Vector2 GetMouseDragDelta( ImGuiMouseButton button, float lockThreshold = -1.0f )
	{
		if ( lockThreshold < 0f )
		{
			// TODO: Use io.MouseDraggingThreshold
			lockThreshold = 1.0f;
		}
		var mouseDelta = button switch
		{
			ImGuiMouseButton.Left	=> MouseState.LeftClickDragTotalDelta,
			ImGuiMouseButton.Right	=> MouseState.RightClickDragTotalDelta,
			ImGuiMouseButton.Middle	=> MouseState.MiddleClickDragTotalDelta,
			_						=> Vector2.Zero
		};
		if ( mouseDelta.Length < lockThreshold )
			return Vector2.Zero;

		return mouseDelta;
	}
}
