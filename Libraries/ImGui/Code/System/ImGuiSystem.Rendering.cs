using Duccsoft.ImGui.Elements;
using Duccsoft.ImGui.Rendering;
using Sandbox.Rendering;
using System.Linq;

namespace Duccsoft.ImGui;

internal partial class ImGuiSystem
{
	public bool UseSceneCamera { get; } = true;
	public CommandList MainCommandList { get; } = new CommandList( "ImGui Main CommandList" );
	private CameraComponent TargetCamera { get; set; }

	private void UpdateTargetCamera()
	{
		if ( !UseSceneCamera )
		{
			TargetCamera?.RemoveCommandList( MainCommandList );
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
			TargetCamera?.RemoveCommandList( MainCommandList );
		}

		// We are switching to a new camera for the first time.
		if ( sceneCamera.IsValid() )
		{
			TargetCamera = sceneCamera;
			TargetCamera.AddCommandList( MainCommandList, Sandbox.Rendering.Stage.AfterUI, 10_000 );
		}
	}

	private void RemoveExpiredDrawLists()
	{
		foreach ( (int id, _ ) in DrawLists.ToList() )
		{
			if ( !CurrentElements.ContainsKey( id ) )
			{
				RemoveDrawList( id );
			}
		}
	}

	public bool TryGetDrawList( int id, out ImDrawList drawList ) => DrawLists.TryGetValue( id, out drawList );
	public bool RemoveDrawList( int id ) => DrawLists.Remove( id );
	public void AddDrawList( int id, ImDrawList drawList ) => DrawLists[id] = drawList;

	private void BuildDrawLists()
	{
		if ( !Game.IsPlaying )
			return;

		MainCommandList.Reset();

		void DrawWindow( int? id )
		{
			if ( id is null )
				return;

			var currentWindow = GetElement( id.Value ) as Window;
			currentWindow.Draw( currentWindow.DrawList );
			MainCommandList.InsertList( currentWindow.DrawList.CommandList );
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
}
