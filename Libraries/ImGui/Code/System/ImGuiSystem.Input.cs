using Sandbox.UI;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	/// <summary>
	/// Filters in the current mouse state in to the "highest priority" button clicked,
	/// returning null if no button is clicked.
	/// </summary>
	public ImGuiMouseButton? MouseButton
	{
		get
		{
			if ( MouseState.MiddleClickDown )
				return ImGuiMouseButton.Middle;
			else if ( MouseState.RightClickDown )
				return ImGuiMouseButton.Right;
			else if ( MouseState.LeftClickDown )
				return ImGuiMouseButton.Left;

			return null;
		}
	}

	private PassthroughPanel _inputPanel;

	private void InitInput()
	{
		CreatePassthrough();
	}
	private void CreatePassthrough()
	{
		_inputPanel = new PassthroughPanel()
		{
			Scene = Scene
		};
		_inputPanel.Style.PointerEvents = PointerEvents.All;
		_inputPanel.LeftClick += p =>
		{
			if ( p )
			{
				MouseState.LeftClickPosition = Mouse.Position;
			}
			MouseState.LeftClickPressed = p;
			MouseState.LeftClickDown = p;
			MouseState.LeftClickReleased = !p;
		};
		_inputPanel.RightClick += p =>
		{
			if ( p )
			{
				MouseState.RightClickPosition = Mouse.Position;
			}
			MouseState.RightClickPressed = p;
			MouseState.RightClickDown = p;
			MouseState.RightClickReleased = !p;
		};
		_inputPanel.MiddleClick += p =>
		{
			if ( p )
			{
				MouseState.MiddleClickPosition = Mouse.Position;
			}
			MouseState.MiddleClickPressed = p;
			MouseState.MiddleClickDown = p;
			MouseState.MiddleClickReleased = !p;
		};
	}

	public int HoveredItemId { get; set; }
	public int HoveredWindowId { get; set; }

	private void InitializeInput()
	{
		UpdateMouseState();
	}

	private void ClearInput()
	{
		ClearMouseState();
	}

	private void UpdateMouseState()
	{
		MouseState.Position = Mouse.Position;
		switch ( MouseButton )
		{
			case ImGuiMouseButton.Right:
				MouseState.RightClickDragFrameDelta = Mouse.Delta;
				break;
			case ImGuiMouseButton.Middle:
				MouseState.MiddleClickDragFrameDelta = Mouse.Delta;
				break;
			default:
				MouseState.LeftClickDragFrameDelta = Mouse.Delta;
				break;
		}
	}

	private void ClearMouseState()
	{
		if ( !MouseState.LeftClickDown )
		{
			ClickedElementId = null;
		}
		MouseState.LeftClickPressed = false;
		MouseState.LeftClickReleased = false;
		MouseState.LeftClickDragFrameDelta = 0f;
		MouseState.RightClickPressed = false;
		MouseState.RightClickReleased = false;
		MouseState.RightClickDragFrameDelta = 0f;
		MouseState.MiddleClickPressed = false;
		MouseState.MiddleClickReleased = false;
		MouseState.MiddleClickDragFrameDelta = 0f;
	}
}
