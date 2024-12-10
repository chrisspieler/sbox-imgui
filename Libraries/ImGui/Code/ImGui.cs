using System.Collections.Generic;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	private static ImGuiSystem System => ImGuiSystem.Current;
	private static IdStack IdStack => ImGuiSystem.Current.IdStack;
	internal static Widget CurrentWidget => CurrentWindow?.CurrentWidget;
	internal static Window CurrentWindow => ImGuiSystem.Current.CurrentWindow;
}
