namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	private void InitStyle()
	{
		Style = new();
		ImGui.StyleColorsDark( Style );
	}

	public ImGuiStyle Style { get; private set; }
}
