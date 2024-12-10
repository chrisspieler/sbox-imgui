using System;

namespace Sandbox;

public class ExampleComponent : Component
{
	[Property] public float FloatProperty1 { get; set; } = 20f;
	[Property, Range(-1000f, 1000f)] public float FloatProperty2 { get; set; } = 400f;
}
