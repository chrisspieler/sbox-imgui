﻿using System;

namespace Duccsoft.ImGui;

[Flags]
public enum ImGuiWindowFlags
{
	ImGuiWindowFlags_None						= 0, 
	/// <summary>
	/// The window will have no title bar, which is useful for popups.
	/// </summary>
	ImGuiWindowFlags_NoTitleBar					= 1 << 0, /*
	ImGuiWindowFlags_NoResize					= 1 << 1,
	ImGuiWindowFlags_NoMove						= 1 << 2,
	ImGuiWindowFlags_NoScrollbar				= 1 << 3,
	ImGuiWindowFlags_NoScrollWithMouse			= 1 << 4,
	ImGuiWindowFlags_NoCollapse					= 1 << 5,
	ImGuiWindowFlags_AlwaysAutoResize			= 1 << 6,
	ImGuiWindowFlags_NoBackground				= 1 << 7,
	ImGuiWindowFlags_NoSavedSettings			= 1 << 8,
	ImGuiWindowFlags_NoMouseInputs				= 1 << 9, 
	ImGuiWindowFlags_MenuBar					= 1 << 10,
	ImGuiWindowFlags_HorizontalScrollbar		= 1 << 11, */
	/// <summary>
	/// Prevent the window from taking focus when transitioning from hidden to visible state.
	/// </summary>
	ImGuiWindowFlags_NoFocusOnAppearing = 1 << 12, /*
	ImGuiWindowFlags_NoBringToFrontOnFocus		= 1 << 13,
	ImGuiWindowFlags_AlwaysVerticalScrollbar	= 1 << 14,
	ImGuiWindowFlags_AlwaysHorizontalScrollbar	= 1 << 15,
	ImGuiWindowFlags_NoNavInputs				= 1 << 16,
	ImGuiWindowFlags_NoNavFocus					= 1 << 17,
	ImGuiWindowFlags_UnsavedDocument			= 1 << 18,
	ImGuiWindowFlags_NoNav						= ImGuiWindowFlags_NoNavInputs | ImGuiWindowFlags_NoNavFocus,
	ImGuiWindowFlags_NoDecoration				= ImGuiWindowFlags_NoTitleBar | ImGuiWindowFlags_NoResize | ImGuiWindowFlags_NoScrollbar | ImGuiWindowFlags_NoCollapse,
	ImGuiWindowFlags_NoInputs					= ImGuiWindowFlags_NoMouseInputs | ImGuiWindowFlags_NoNavInputs | ImGuiWindowFlags_NoNavFocus,
	*/
}