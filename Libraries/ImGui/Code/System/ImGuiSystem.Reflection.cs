using System;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem : IHotloadManaged
{
	private ReflectionCache ReflectionCache { get; set; } = new();

	public TypeDescription GetType( Type type )
	{
		return null;
	}
}
