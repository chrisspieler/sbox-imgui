using Duccsoft.ImGui;
using System;

namespace Sandbox;

public class ImGuiDemo : Component
{
	private int _clickCounter;
	private bool _shouldDrawWindow2 = true;
	private float _myFloatValue = 24f;
	private bool _shouldFocusFloatingWindow = false;

	protected override void OnUpdate()
	{
		Mouse.Visible = true;
		DrawWindow1();
		DrawWindow2();
		DrawWindowNoTitle();
		DrawMovingWindow();
	}

	private void DrawWindow1()
	{
		ImGui.SetNextWindowPos( new Vector2( 300, 200 ) * ImGuiStyle.UIScale );
		ImGui.Begin( "Window 1" );
		if ( ImGui.Button( "Click me!" ) )
		{
			_clickCounter++;
		}
		ImGui.Text( "Clicked {0} times.", _clickCounter );
		if ( ImGui.Button( "Focus Floating Window" ) )
		{
			_shouldFocusFloatingWindow = true;
		}
		ImGui.SliderFloat( "My Float", () => _myFloatValue, v => _myFloatValue = v, -128f, 256f );
		ImGui.End();
	}

	private void DrawWindow2()
	{
		if ( !_shouldDrawWindow2 )
			return;

		if ( ImGui.Begin( "Window 2", onClose: () => _shouldDrawWindow2 = false ) )
		{
			ImGui.SetWindowPos( new Vector2( 150, 250 ) * ImGuiStyle.UIScale );
			ImGui.Text( "Hello," );
			ImGui.Text( "World!" );
			ImGui.Text( "How's it going, everyone?" );
		}
		ImGui.End();
	}

	private void DrawWindowNoTitle()
	{
		ImGui.SetNextWindowPos( new Vector2( 500, 100 ) * ImGuiStyle.UIScale );
		var flags =	ImGuiWindowFlags.ImGuiWindowFlags_NoTitleBar
				| ImGuiWindowFlags.ImGuiWindowFlags_NoFocusOnAppearing;
		ImGui.Begin( "Window No Title", null, flags );
		ImGui.Text( "This window has no title!" );
		ImGui.Text( "Wow." );
		ImGui.End();
	}
	
	private void DrawMovingWindow()
	{
		Vector2 GetPosition()
		{
			var sin = (MathF.Sin( Time.Now * 0.8f ) + 1) * 0.5f;
			var cos = (MathF.Cos( Time.Now * 0.8f ) + 1) * 0.5f;
			var sinCos = new Vector2( sin, cos );
			sinCos *= 0.7f;
			sinCos += 0.075f;
			return sinCos * new Vector2( 1280, 720 );
		}

		Vector2 GetSize()
		{
			var size = new Vector2( 0.20f, 0.10f );
			var sin1 = (MathF.Sin( Time.Now * 0.7f ) + 1) * 0.5f;
			var sin2 = (MathF.Sin( Time.Now * 0.85f ) + 1) * 0.5f;
			var offset = new Vector2( 0.4f, 0.2f ) * new Vector2( sin1, sin2 );
			return (size + offset) * new Vector2( 1280, 720 );
		}

		ImGui.SetNextWindowPos( GetPosition() * ImGuiStyle.UIScale );
		ImGui.SetNextWindowSize( GetSize() * ImGuiStyle.UIScale );
		if ( _shouldFocusFloatingWindow )
		{
			ImGui.SetNextWindowFocus();
		}
		var flags = ImGuiWindowFlags.ImGuiWindowFlags_NoFocusOnAppearing;
		ImGui.Begin( "Floating Window", null, flags );
		ImGui.End();
		_shouldFocusFloatingWindow = false;
	}
}
