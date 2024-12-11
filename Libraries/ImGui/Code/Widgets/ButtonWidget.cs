using Duccsoft.ImGui.Rendering;
using System.Reflection;

namespace Duccsoft.ImGui;

internal class ButtonWidget : Widget
{
	public ButtonWidget( Window parent, string label ) : base( parent )
	{
		Label = label;
		Id = ImGui.GetID( Label.GetHashCode() );
		Show();
	}

	public string Label { get; set; }
	public bool IsReleased { get; set; }
	public override Vector2 Size => ImGui.CalcTextSize( Label ) + ImGui.GetStyle().FramePadding;

	public override void UpdateInput()
	{
		base.UpdateInput();
		IsReleased = false;

		var lastDrawList = ImGuiSystem.Current.PreviousBoundsList;
		if ( lastDrawList.WidgetIds.TryGetValue( Id, out var lastWidget ) )
		{
			if ( lastWidget.IsActive && !MouseState.LeftClickDown )
			{
				IsReleased = true;
			}
		}
	}

	public override void Draw( ImDrawList drawList )
	{
		var buttonColor = ImGui.GetColorU32( ImGuiCol.Button );
		if ( IsActive )
		{
			buttonColor = ImGui.GetColorU32( ImGuiCol.ButtonActive );
		}
		else if ( IsHovered )
		{
			buttonColor = ImGui.GetColorU32( ImGuiCol.ImGuiColButtonHovered );
		}
		drawList.AddRectFilled( ScreenPosition, ScreenPosition + Size, buttonColor );
		drawList.AddText( ScreenPosition + Size * 0.5f, ImGui.GetColorU32( ImGuiCol.Text ), Label, TextFlag.Center );
	}
}
