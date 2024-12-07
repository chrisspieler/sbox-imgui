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
	public override Vector2 GetSize() => ImGui.CalcTextSize( Label ) + ImGui.GetStyle().FramePadding;

	public override void Paint( ImGuiPainter painter )
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
		painter.DrawRect( GetScreenBounds(), buttonColor );
		painter.DrawText( Label, ScreenPosition + GetSize() * 0.5f );
	}

	public override void UpdateInput( MouseState mouse )
	{
		base.UpdateInput( mouse );
		IsReleased = false;

		var lastDrawList = ImGuiSystem.Current.PreviousDrawList;
		if ( lastDrawList.WidgetIds.TryGetValue( Id, out var lastWidget ) )
		{
			if ( lastWidget.IsActive && !mouse.LeftClickDown )
			{
				IsReleased = true;
			}
		}
	}
}
