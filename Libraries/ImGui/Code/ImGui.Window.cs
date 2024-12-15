using Duccsoft.ImGui.Rendering;
using System;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static bool Begin( string name, ref bool open, ImGuiWindowFlags flags = default )
	{
		System.BeginWindow( name, ref open, flags );
		return true;
	}

	public static bool Begin( string name, ImGuiWindowFlags flags = default )
	{
		var open = true;
		return Begin( name, ref open, flags );
	}

	// TODO: Allow windows to be collapsed.
	public static bool IsWindowCollapsed() => throw new NotImplementedException();
	public static bool IsWindowAppearing() => CurrentWindow?.IsAppearing == true;
	public static bool IsWindowFocused( ImGuiFocusedFlags flags ) => CurrentWindow?.IsFocused == true; // TODO: Use flags
	public static bool IsWindowHovered( ImGuiHoveredFlags flags ) => CurrentWindow?.IsHovered == true; // TODO: Use flags

	public static void End()
	{
		ImGuiSystem.Current.EndWindow();
	}

	public static ImDrawList GetWindowDrawList()
	{
		if ( CurrentWindow is null )
			return default;

		return CurrentWindow.DrawList;
	}

	public static Vector2 GetWindowPos()
	{
		if ( CurrentWindow is null )
			return default;

		return CurrentWindow.ScreenRect.Position;
	}

	public static Vector2 GetWindowSize()
	{
		if ( CurrentWindow is null )
			return default;

		return CurrentWindow.ScreenRect.Size;
	}

	public static float GetWindowWidth() => GetWindowSize().x;
	public static float GetWindowHeight() => GetWindowSize().y;

	public static void SetNextWindowPos( Vector2 position, ImGuiCond condition = default, Vector2 pivot = default )
	{
		ImGuiSystem.Current.NextWindowPosition = position;
		ImGuiSystem.Current.NextWindowPivot = pivot;
	}

	public static void SetWindowPos( Vector2 position, ImGuiCond condition = default )
	{
		if ( CurrentWindow is null )
			return;

		CurrentWindow.Position = position;
	}

	public static void SetNextWindowSize( Vector2 size, ImGuiCond condition = default )
	{
		ImGuiSystem.Current.NextWindowSize = size;
	}

	public static void SetWindowSize( Vector2 size, ImGuiCond condition = default )
	{
		var window = ImGuiSystem.Current.CurrentWindow;
		if ( window is not null )
		{
			window.CustomSize = size;
		}
	}

	/// <summary>
	/// Causes the next window that is created to start focused. Should be called before Begin().
	/// </summary>
	public static void SetNextWindowFocus()
	{
		ImGuiSystem.Current.ShouldFocusNextWindow = true;
	}
}
