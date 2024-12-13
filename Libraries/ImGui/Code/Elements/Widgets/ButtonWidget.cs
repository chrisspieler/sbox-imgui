using Duccsoft.ImGui.Rendering;

namespace Duccsoft.ImGui.Elements;

internal class ButtonWidget : Element
{
	public ButtonWidget( Window parent, string label ) : base( parent )
	{
		Label = label;
		Id = ImGui.GetID( Label.GetHashCode() );

		Size = ImGui.CalcTextSize( Label ) + ImGui.GetStyle().FramePadding;

		OnBegin();
		OnEnd();
	}

	public string Label { get; set; }

	protected override void OnDrawSelf( ImDrawList drawList )
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
