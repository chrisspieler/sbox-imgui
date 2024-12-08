﻿using Sandbox.UI;
using System.Linq;

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

	public Window HoveredWindow { get; set; }

	private void UpdateWindowFocus()
	{
		UpdateHoveredWindow();
		if ( MouseState.LeftClickPressed )
		{
			Focus( HoveredWindow );
		}
		HoveredWindow?.UpdateInput();
	}

	private void UpdateInputState()
	{
		MouseState.Position = Mouse.Position;
	}

	private void ClearInputState()
	{
		if ( !MouseState.LeftClickDown )
		{
			ClickedWidgetId = 0;
		}
		MouseState.LeftClickPressed = false;
		MouseState.LeftClickReleased = false;
		MouseState.RightClickPressed = false;
		MouseState.RightClickReleased = false;
		MouseState.MiddleClickPressed = false;
		MouseState.MiddleClickReleased = false;
	}

	public void UpdateHoveredWindow()
	{
		var windows = CurrentDrawList.Windows.ToArray();
		var lastMouseOver = -1;
		for ( int i = 0; i < windows.Length; i++ )
		{
			var window = windows[i];
			window.IsHovered = false;
			if ( window.IsMouseInScreenRect )
			{
				lastMouseOver = i;
				if ( window.IsFocused )
				{
					break;
				}
			}
		}

		if ( lastMouseOver < 0 )
			return;

		var hovered = windows[lastMouseOver];
		hovered.IsHovered = true;
		HoveredWindow = hovered;
	}
}
