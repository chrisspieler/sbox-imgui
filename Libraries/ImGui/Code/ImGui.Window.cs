using System;

namespace Duccsoft;

public static partial class ImGui
{
	public static bool Begin( string name, Action onClose = null, ImGuiWindowFlags flags = default )
	{
		ImGuiContextSystem.Current.PushWindow( name, onClose, flags );
		return true;
	}

	public static void End()
	{
		ImGuiContextSystem.Current.PopWindow();
	}

	public static void SetNextWindowPos( Vector2 position, ImGuiCond condition = default, Vector2 pivot = default )
	{
		var window = ImGuiContextSystem.Current.NextWindow;
		if ( window is not null )
		{
			window.Position = position;
			window.Pivot = pivot;
		}
	}

	public static void SetWindowPos( Vector2 position, ImGuiCond condition = default )
	{
		var window = ImGuiContextSystem.Current.CurrentWindow;
		if ( window is not null )
		{
			window.Position = position;
		}
	}

	public static void SetNextWindowSize( Vector2 size, ImGuiCond condition = default )
	{
		var window = ImGuiContextSystem.Current.NextWindow;
		if ( window is not null )
		{
			window.Size = size;
		}
	}

	public static void SetWindowSize( Vector2 size, ImGuiCond condition = default )
	{
		var window = ImGuiContextSystem.Current.CurrentWindow;
		if ( window is not null )
		{
			window.Size = size;
		}
	}
}
