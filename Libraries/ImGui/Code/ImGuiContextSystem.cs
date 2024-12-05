namespace Duccsoft;

internal partial class ImGuiContextSystem : GameObjectSystem<ImGuiContextSystem>
{
	public ImGuiContextSystem( Scene scene ) : base( scene )
	{
		WindowStack = new();
	}
}
