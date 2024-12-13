using Duccsoft.ImGui.Elements;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	private static ImGuiSystem System => ImGuiSystem.Current;
	private static IdStack IdStack => ImGuiSystem.Current.IdStack;
	internal static Element CurrentItem => CurrentWindow?.CurrentItem;
	internal static Window CurrentWindow => System.CurrentWindow;
}
