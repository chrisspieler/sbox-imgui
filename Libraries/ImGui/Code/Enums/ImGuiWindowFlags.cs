using System;

namespace Duccsoft.ImGui;

[Flags]
public enum ImGuiWindowFlags
{
	None						= 0, 
	/// <summary>
	/// The window will have no title bar, which is useful for popups.
	/// </summary>
	NoTitleBar					= 1 << 0, /*
	NoResize					= 1 << 1,
	NoMove						= 1 << 2,
	NoScrollbar					= 1 << 3,
	NoScrollWithMouse			= 1 << 4,
	NoCollapse					= 1 << 5,
	AlwaysAutoResize			= 1 << 6,
	NoBackground				= 1 << 7,
	NoSavedSettings				= 1 << 8,
	NoMouseInputs				= 1 << 9, 
	MenuBar						= 1 << 10,
	HorizontalScrollbar			= 1 << 11, */
	/// <summary>
	/// Prevent the window from taking focus when transitioning from hidden to visible state.
	/// </summary>
	NoFocusOnAppearing			= 1 << 12, /*
	NoBringToFrontOnFocus		= 1 << 13,
	AlwaysVerticalScrollbar		= 1 << 14,
	AlwaysHorizontalScrollbar	= 1 << 15,
	NoNavInputs					= 1 << 16,
	NoNavFocus					= 1 << 17,
	UnsavedDocument				= 1 << 18,
	NoNav						= NoNavInputs | NoNavFocus,
	NoDecoration				= NoTitleBar | NoResize | NoScrollbar | NoCollapse,
	NoInputs					= NoMouseInputs | NoNavInputs | NoNavFocus,
	*/
}
