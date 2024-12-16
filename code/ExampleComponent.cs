using System;

namespace Sandbox;

public class ExampleComponent : Component
{
	public enum ExampleCombo
	{
		Foo,
		Bar,
		Baz,
		Duccs
	}

	[Property] public bool Checkbox { get; set; } = true;
	[Property] public float DragFloat { get; set; } = 20f;
	[Property] public Vector2 DragFloat2 { get; set; } = new Vector2( -20, 50 );
	[Property] public Vector3 DragFloat3 { get; set; } = new Vector3( 10, -30, 50 );
	[Property, Range(-1000f, 1000f)] public float SliderFloat { get; set; } = 400f;
	[Property, Range( 0, 1 )] public Vector2 SliderFloat2 { get; set; }
	[Property, Range( -360f, 360f )] public Vector3 SliderFloat3 { get; set; }
	[Property, Range( 0, 1 )] public Vector4 SliderFloat4 { get; set; }
	[Property] public int DragInt { get; set; } = 64;
	[Property, Range( 0, 256 )] public int SliderInt { get; set; } = 8;
	[Property] public ExampleCombo Combo { get; set; } = ExampleCombo.Duccs;
	[Property] public Color ColorEdit4 { get; set; } = Color.Orange;
}
