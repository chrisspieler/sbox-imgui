using Sandbox.UI;
using System;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem : GameObjectSystem<ImGuiSystem>
{
	public ImGuiSystem( Scene scene ) : base( scene )
	{
		InitStyle();
		InitInput();
		InitRendering();
		Listen( Stage.StartUpdate, -100, StartUpdate, "ImGui Start Update" );
		Listen( Stage.FinishUpdate, 100, FinishUpdate, "ImGui FinishUpdate" );
	}

	private void StartUpdate()
	{
		ClearCommandList();
		// Input
		UpdateInputState();
	}

	private void FinishUpdate()
	{
		// Input
		UpdateWindowFocus();
		ClearInputState();
		// Rendering
		Draw();
	}
}
