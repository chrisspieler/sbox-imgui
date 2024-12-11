using Duccsoft.ImGui.Rendering;

namespace Duccsoft.ImGui;

internal class TextWidget : Widget
{
	public TextWidget( Window parent, string text ) : base( parent )
	{
		Text = text;
		Show();
	}

	public string Text { get; set; }

	public override Vector2 Size => ImGui.CalcTextSize( Text );

	public override void Draw( ImDrawList drawList )
	{
		drawList.AddText( ScreenPosition, ImGui.GetColorU32( ImGuiCol.Text ), Text );
	}
}
