using System.Collections.Generic;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	private static ImGuiSystem System => ImGuiSystem.Current;
	private static IdStack IdStack => ImGuiSystem.Current.IdStack;
	private static Widget CurrentWidget => CurrentWindow?.CurrentWidget;
	private static Window CurrentWindow => ImGuiSystem.Current.CurrentWindow;
}
