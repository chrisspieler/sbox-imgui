using System;

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

	private void Draw()
	{
		if ( !Game.IsPlaying )
			return;

		Window focusedWindow = null;
		foreach ( var window in CurrentBoundsList.Windows )
		{
			if ( window.IsFocused )
			{
				focusedWindow = window;
				continue;
			}
			window.Draw();
		}
		// Draw the focused window last.
		focusedWindow?.Draw();
		ClearWindows();
	}

	private void Render( SceneCamera camera )
	{
		if ( !Game.IsPlaying )
			return;

		int commandCount = 0;
		Window focusedWindow = null;
		foreach ( var window in PreviousBoundsList.Windows )
		{
			commandCount += window.DrawList.Count;
			if ( window.IsFocused )
			{
				focusedWindow = window;
				continue;
			}
			window.DrawList.Render();
		}
		// Render the focused window last.
		focusedWindow?.DrawList?.Render();
	}
}
