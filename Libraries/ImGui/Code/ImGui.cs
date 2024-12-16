using Duccsoft.ImGui.Elements;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	private static ImGuiSystem System => ImGuiSystem.Current;
	private static IdStack IdStack => ImGuiSystem.Current.IdStack;
	internal static Window CurrentWindow => System.CurrentWindow;
	internal static Element CurrentItemRecursive => CurrentWindow?.CurrentItemRecursive;
	internal static Element LastItemRecursive => CurrentWindow?.LastItemRecursive;
}
