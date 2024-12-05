﻿using Duccsoft;
using Sandbox.Rendering;

internal class ImGuiRenderSystem : GameObjectSystem<ImGuiRenderSystem>
{
	public ImGuiRenderSystem( Scene scene ) : base( scene )
	{
		CommandList = new CommandList( "ImGui" )
		{
			Flags = CommandList.Flag.Hud
		};
		Style = new();
		ImGui.StyleColorsDark( Style );
		Listen( Stage.StartUpdate, 0, Clear, "ImGui Clear" );
		Listen( Stage.FinishUpdate, 0, Draw, "ImGui Draw" );
	}

	public ImGuiStyle Style { get; private set; }
	public bool UseSceneCamera { get; } = true;
	public ImGuiPainter Painter => new( CommandList );
	private CameraComponent TargetCamera { get; set; }

	public CommandList CommandList { get; }
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

		var windows = ImGuiContextSystem.Current.WindowDrawList;
		foreach ( var window in windows )
		{
			window.Paint( Painter );
		}
		ImGuiContextSystem.Current.ClearWindows();
	}
}