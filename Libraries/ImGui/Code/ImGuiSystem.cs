namespace Duccsoft.ImGui;

internal partial class ImGuiSystem : GameObjectSystem<ImGuiSystem>
{
	public ImGuiSystem( Scene scene ) : base( scene )
	{
		InitStyle();
		InitRendering();
	}
}
