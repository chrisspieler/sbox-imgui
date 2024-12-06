namespace Duccsoft.ImGui;

internal class ButtonWidget : Widget
{
	public ButtonWidget( string label )
	{
		Label = label;
		Id = Label.GetHashCode();
		Show();
	}

	public string Label { get; set; }
	public override Vector2 GetSize() => ImGui.CalcTextSize( Label ) + ImGui.GetStyle().FramePadding;

	public override void Paint( ImGuiPainter painter )
	{
		var buttonColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_Button );
		if ( IsActive )
		{
			buttonColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_ButtonActive );
		}
		else if ( IsHovered )
		{
			buttonColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_ButtonHovered );
		}
		painter.DrawRect( GetScreenBounds(), buttonColor );
		painter.DrawText( Label, ScreenPosition + GetSize() * 0.5f );
	}
}
