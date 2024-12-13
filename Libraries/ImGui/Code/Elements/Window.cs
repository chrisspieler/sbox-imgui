using Duccsoft.ImGui.Rendering;
using System;

namespace Duccsoft.ImGui.Elements;

public class Window : Element
{
	public Window( string name, Vector2 screenPos, Vector2 pivot, Vector2 size, ImGuiWindowFlags flags )
		: base( null )
	{
		Name = name;
		Id = ImGui.GetID( Name );
		WindowFlags = flags;
		Position = screenPos;
		if ( System.CustomWindowPositions.TryGetValue( Id, out var customPos ) )
		{
			// Window positions are stored unscaled in case screen size changes,
			// so we need to scale them back up here.
			Position = customPos * ImGuiStyle.UIScale;
		}
		Pivot = pivot;
		Padding = ImGui.GetStyle().WindowPadding;
		CustomSize = size;
		ImGuiSystem.Current.IdStack.Push( Id );
		ImGuiSystem.Current.WindowStack.Push( this );
		CursorPosition = ImGui.GetStyle().WindowPadding;
		CursorStartPosition = CursorPosition;

		OnBegin();
	}

	public string Name { get; init; }
	public ImDrawList DrawList { get; set; } = new();
	public ImGuiWindowFlags WindowFlags { get; init; }

	public Action OnClose { get; set; }

	internal WindowTitleBar TitleBar { get; set; }

	public static Color32 BackgroundColor => ImGui.GetColorU32( ImGuiCol.WindowBg );
	public static Color32 BorderColor => ImGui.GetColorU32( ImGuiCol.Border );

	public override void OnEnd()
	{
		base.OnEnd();

		TitleBar?.OnEnd();
	}

	protected override void OnDrawSelf( ImDrawList drawList )
	{
		DrawList.AddRect( ScreenRect.TopLeft, ScreenRect.BottomRight, BorderColor, rounding: 0, flags: ImDrawFlags.None, thickness: 1 );
		DrawList.AddRectFilled( ScreenRect.TopLeft, ScreenRect.BottomRight, BackgroundColor );
	}
}
