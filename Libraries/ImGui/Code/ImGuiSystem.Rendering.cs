using Sandbox.Rendering;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	private void InitRendering()
	{
		CommandList = new CommandList( "ImGui" )
		{
			Flags = CommandList.Flag.Hud
		};

		Listen( Stage.StartUpdate, 0, Clear, "ImGui Clear" );
		Listen( Stage.FinishUpdate, 100, Draw, "ImGui Draw" );
	}

	public bool UseSceneCamera { get; } = true;
	public ImGuiPainter Painter => new( CommandList );
	private CameraComponent TargetCamera { get; set; }

	public CommandList CommandList { get; private set; }
	public Sandbox.Rendering.Stage RenderingStage { get; set; } = Sandbox.Rendering.Stage.AfterPostProcess;

	private void Clear()
	{
		if ( !Game.IsPlaying )
			return;

		UpdateTargetCamera();
		// Clear the command list, in case it hasn't been cleared by whatever is rendering it.
		CommandList.Reset();
	}

	private void UpdateTargetCamera()
	{
		if ( !UseSceneCamera )
		{
			TargetCamera?.RemoveCommandList( CommandList );
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
			TargetCamera.RemoveCommandList( CommandList );
		}

		if ( sceneCamera.IsValid() )
		{
			TargetCamera = sceneCamera;
			TargetCamera.AddCommandList( CommandList, RenderingStage, 10_000 );
		}
	}

	private void Draw()
	{
		if ( !Game.IsPlaying )
			return;

		Window focusedWindow = null;
		foreach ( var window in CurrentDrawList.Windows )
		{
			if ( window.IsFocused )
			{
				focusedWindow = window;
				continue;
			}
			window.Paint( Painter );
		}
		// Paint the focused window last.
		focusedWindow?.Paint( Painter );
		ClearWindows();
	}
}
