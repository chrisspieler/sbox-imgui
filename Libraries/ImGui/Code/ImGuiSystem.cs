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
	}

	
}
