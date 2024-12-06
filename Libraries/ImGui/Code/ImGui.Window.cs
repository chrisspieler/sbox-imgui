using System;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static bool Begin( string name, Action onClose = null, ImGuiWindowFlags flags = default )
	{
		ImGuiSystem.Current.PushWindow( name, onClose, flags );
		return true;
	}

	public static void End()
	{
		ImGuiSystem.Current.PopWindow();
	}

	public static void SetNextWindowPos( Vector2 position, ImGuiCond condition = default, Vector2 pivot = default )
	{
		var window = ImGuiSystem.Current.NextWindow;
		if ( window is not null )
		{
			window.ScreenPosition = position;
			window.Pivot = pivot;
		}
	}

	public static void SetWindowPos( Vector2 position, ImGuiCond condition = default )
	{
		var window = ImGuiSystem.Current.CurrentWindow;
		if ( window is not null )
		{
			var delta = position - window.ScreenPosition;
			window.ScreenPosition = position;
			ImGuiSystem.Current.CursorScreenPosition += delta;
		}
	}

	public static void SetNextWindowSize( Vector2 size, ImGuiCond condition = default )
	{
		var window = ImGuiSystem.Current.NextWindow;
		if ( window is not null )
		{
			window.CustomScreenSize = size;
		}
	}

	public static void SetWindowSize( Vector2 size, ImGuiCond condition = default )
	{
		var window = ImGuiSystem.Current.CurrentWindow;
		if ( window is not null )
		{
			window.CustomScreenSize = size;
		}
	}
}
