using System;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	private ReflectionCache ReflectionCache { get; set; } = new();

	public TypeDescription GetTypeDescription( Type type ) => ReflectionCache.GetTypeDescription( type );
	public List<PropertyDescription> GetProperties( Type type ) => ReflectionCache.GetProperties( type );
}
