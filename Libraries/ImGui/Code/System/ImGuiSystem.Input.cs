﻿using Sandbox.UI;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	public MouseState MouseState { get; private set; }
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

	private bool _leftClickPressed { get; set; }
	private bool _leftClickDown { get; set; }
	private bool _leftClickReleased { get; set; }
	private bool _rightClickPressed { get; set; }
	private bool _rightClickDown { get; set; }
	private bool _rightClickReleased { get; set; }
	private bool _middleClickPressed { get; set; }
	private bool _middleClickDown { get; set; }
	private bool _middleClickReleased { get; set; }

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
			_leftClickPressed = p;
			_leftClickDown = p;
			_leftClickReleased = !p;
		};
		_inputPanel.RightClick += p =>
		{
			_rightClickPressed = p;
			_rightClickDown = p;
			_rightClickReleased = !p;
		};
		_inputPanel.MiddleClick += p =>
		{
			_middleClickPressed = p;
			_middleClickDown = p;
			_middleClickReleased = !p;
		};
	}

	public Window HoveredWindow { get; set; }

	private void UpdateWindowFocus()
	{
		HoveredWindow = GetHoveredWindow( MouseState.Position );
		if ( MouseState.LeftClickPressed )
		{
			Focus( HoveredWindow );
		}
		HoveredWindow?.UpdateInput( MouseState );
	}

	private void UpdateInputState()
	{
		MouseState = new MouseState()
		{
			Position = Mouse.Position,
			LeftClickPressed = _leftClickPressed,
			LeftClickDown = _leftClickDown,
			LeftClickReleased = _leftClickReleased,
			RightClickPressed = _rightClickPressed,
			RightClickDown = _rightClickDown,
			RightClickReleased = _rightClickReleased,
			MiddleClickPressed = _middleClickPressed,
			MiddleClickDown = _middleClickDown,
			MiddleClickReleased = _middleClickReleased,
		};
	}
	private void ClearInputState()
	{
		if ( !_leftClickDown )
		{
			ClickedWidgetId = 0;
		}
		_leftClickPressed = false;
		_leftClickReleased = false;
		_rightClickPressed = false;
		_rightClickReleased = false;
		_middleClickPressed = false;
		_middleClickReleased = false;
	}

	public Window GetHoveredWindow( Vector2 hoverPos, ImGuiHoveredFlags flags = default )
	{
		if ( FocusedWindow?.IsMouseInScreenRect == true )
			return FocusedWindow;
		
		Window hovered = null;
		foreach ( var window in CurrentDrawList.Windows )
		{
			// The last non-focused window is guaranteed frontmost.
			if ( window.IsMouseInScreenRect )
				hovered = window;
		}
		return hovered;
	}
}
