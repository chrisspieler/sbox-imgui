using Duccsoft.ImGui.Elements;
using System;
using System.Linq;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	public bool UseSceneCamera { get; } = true;
	private CameraComponent TargetCamera { get; set; }

	private IDisposable _uiRenderHook;

	private void UpdateTargetCamera()
	{
		if ( !UseSceneCamera )
		{
			_uiRenderHook?.Dispose();
			_uiRenderHook = null;
			TargetCamera = null;
			return;
		}

		var sceneCamera = Scene.Camera;
		var isTargetValid = TargetCamera.IsValid();

		// If our target camera is still the main camera in the scene.
		if ( isTargetValid && sceneCamera == TargetCamera )
			return;

		// If the old camera is still valid, but we're switching away from it.
		if ( isTargetValid )
		{
			_uiRenderHook?.Dispose();
			_uiRenderHook = null;
		}

		if ( sceneCamera.IsValid() )
		{
			TargetCamera = sceneCamera;
			_uiRenderHook?.Dispose();
			_uiRenderHook = null;
			_uiRenderHook = TargetCamera.AddHookAfterTransparent( "ImDrawList Rendering", 10_000, Render );
		}
	}

	private void BuildDrawLists()
	{
		if ( !Game.IsPlaying )
			return;

		void DrawWindow( int? id )
		{
			if ( id is null )
				return;

			var currentWindow = GetElement( id.Value ) as Window;
			currentWindow.Draw( currentWindow.DrawList );
		}

		int? focusedWindow = null;
		foreach ( var window in CurrentBoundsList.GetRootElements() )
		{
			if ( window.IsFocused )
			{
				focusedWindow = window.Id;
				continue;
			}
			DrawWindow( window.Id );
		}
		// Draw the focused window on top of everything else.
		DrawWindow( focusedWindow );
	}

	private void Render( SceneCamera camera )
	{
		if ( !Game.IsPlaying )
			return;

		int commandCount = 0;
		var windows = CurrentBoundsList
			.GetRootElements()
			.Select( r => GetElement( r.Id ) )
			.OfType<Window>()
			.ToList();
		// Log.Info( $"Printing Draw List" );
		for ( int i = 0; i < windows.Count; i++ )
		{
			var window = windows[i];
			commandCount += window.DrawList.Count;
			// Log.Info( $"{i}: {window.Id}, {window.DrawList.Count} draw command(s)" );
			// We assume the windows were already sorted in to the correct order.
			window.DrawList.Render();
		}
	}
}
