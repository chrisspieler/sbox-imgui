using Duccsoft.ImGui;
using System;

namespace Sandbox;

public class ImGuiDemo : Component
{
	protected override void OnUpdate()
	{
		Mouse.Visible = true;
		ImGui.SetNextWindowPos( GetPosition() );
		ImGui.SetNextWindowSize( GetSize() );
		if ( ImGui.Begin( "Test Window" ) )
		{

		}
		ImGui.End();
		if ( ImGui.Begin( "Window 1" ) )
		{
			ImGui.SetWindowPos( new Vector2( 200, 300 ) );
			// ImGui.SetWindowSize( new Vector2( 200, 100 ) );
		}
		ImGui.End();
		if ( ImGui.Begin( "Window 2" ) )
		{
			ImGui.SetWindowPos( new Vector2( 250, 310 ) );
			// ImGui.SetWindowSize( new Vector2( 150, 100 ) );
			ImGui.Text( "Hello," );
			ImGui.Text( "World!" );
			ImGui.Text( "How's it going, everyone?" );
		}
		ImGui.End();
	}

	private Vector2 GetPosition()
	{
		var sin = (MathF.Sin( Time.Now * 0.8f ) + 1) * 0.5f;
		var cos = (MathF.Cos( Time.Now * 0.8f ) + 1) * 0.5f;
		var sinCos = new Vector2( sin, cos );
		sinCos *= 0.7f;
		sinCos += 0.075f;
		return sinCos * Screen.Size;
	}

	private Vector2 GetSize()
	{
		var size = new Vector2( 0.20f, 0.10f );
		var sin1 = (MathF.Sin( Time.Now * 0.7f ) + 1) * 0.5f;
		var sin2 = (MathF.Sin( Time.Now * 0.85f ) + 1) * 0.5f;
		var offset = new Vector2( 0.4f, 0.2f ) * new Vector2( sin1, sin2 );
		return ( size + offset ) * Screen.Size;
	}
}
