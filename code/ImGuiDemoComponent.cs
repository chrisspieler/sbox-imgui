using Duccsoft.ImGui;
using System;

namespace Sandbox;

public class ImGuiDemo : Component
{
	[Property] public ExampleComponent ExampleComponent { get; set; }
	[Property] public Texture ExampleTexture { get; set; }

	private int _clickCounter;
	private bool _shouldDrawWindow2 = true;
	private float _myFloatValue = 24f;
	private bool _shouldFocusFloatingWindow = false;

	protected override void OnUpdate()
	{
		Mouse.Visible = true;
		DrawWindow1();
		DrawWindow2();
		DrawDupeWidgetWindows();
		DrawWindowNoTitle();
		DrawMovingWindow();
		ExampleComponent.ImGuiInspector();
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
		ImGui.SliderFloat( "My Float", ref _myFloatValue, -128f, 256f );
		ImGui.Button( "1" ); ImGui.SameLine();
		ImGui.Button( "2" ); ImGui.SameLine();
		ImGui.Button( "3" ); ImGui.SameLine();
		ImGui.Button( "4" );
		ImGui.Image( ExampleTexture, new Vector2( 128 ) * ImGuiStyle.UIScale, Color.White, ImGui.GetColorU32( ImGuiCol.Border ) );
		ImGui.End();
	}

	private void DrawWindow2()
	{
		if ( !_shouldDrawWindow2 )
			return;

		ImGui.SetNextWindowPos( new Vector2( 150, 250 ) * ImGuiStyle.UIScale );
		if ( ImGui.Begin( "Window 2", ref _shouldDrawWindow2 ) )
		{
			ImGui.Text( "Hello," );
			ImGui.Text( "World!" );
			ImGui.Text( "How's it going, everyone?" );
		}
		ImGui.End();
	}

	private float _dupeFloat1;
	private float _dupeFloat2;
	private float _dupeFloat3;

	private void DrawDupeWidgetWindows()
	{
		ImGui.SetNextWindowPos( new Vector2( 700, 500 ) * ImGuiStyle.UIScale );
		if ( ImGui.Begin( "Window 3" ) )
		{
			ImGui.Text( "Slider1" ); ImGui.SameLine();
			var dupeFloat1 = _dupeFloat1;
			ImGui.SliderFloat( "Slider1", ref dupeFloat1, 0, 100 );
			_dupeFloat1 = dupeFloat1;
			ImGui.Text( "Slider2" ); ImGui.SameLine();
			var dupeFloat2 = _dupeFloat2;
			ImGui.SliderFloat( "Slider2", ref dupeFloat2, 0, 100 );
			_dupeFloat2 = dupeFloat2;
			ImGui.Text( "Slider2" ); ImGui.SameLine();
			var dupeFloat3 = _dupeFloat3;
			ImGui.SliderFloat( "Slider2", ref dupeFloat3, 0, 100 );
			_dupeFloat3 = dupeFloat3;
		}
		ImGui.End();
	}

	private bool _logPassthroughClick;

	private void DrawWindowNoTitle()
	{
		ImGui.SetNextWindowPos( new Vector2( 500, 100 ) * ImGuiStyle.UIScale );
		var flags =	ImGuiWindowFlags.NoTitleBar
				| ImGuiWindowFlags.NoFocusOnAppearing;
		ImGui.Begin( "Window No Title", flags );
		ImGui.Text( "Press the E key to test whether Input.Pressed still works." );
		ImGui.Text( "(Check the console for use action output)" );
		ImGui.NewLine();
		var io = ImGui.GetIO();
		ImGui.Text( $"WantCaptureMouse: {io.WantCaptureMouse}" );
		var logPassthroughClick = _logPassthroughClick;
		ImGui.Checkbox( "Log Passthrough Click", ref logPassthroughClick );
		_logPassthroughClick = logPassthroughClick;
		if ( _logPassthroughClick )
		{
			ImGui.Text( "(Check the console for passthrough click output)" );
		}
		ImGui.End();

		if ( Input.Pressed( "use" ) )
		{
			Log.Info( "Pressed use action!" );
		}
		if ( _logPassthroughClick )
		{
			if ( Input.Pressed( "attack1" ) )
			{
				Log.Info( "Pressed attack1 action!" );
			}
			if ( Input.Released( "attack1" ) )
			{
				Log.Info( $"Released attack1 action!" );
			}
		}
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
		var flags = ImGuiWindowFlags.NoFocusOnAppearing;
		ImGui.Begin( "Floating Window", flags );
		ImGui.End();
		_shouldFocusFloatingWindow = false;
	}
}
