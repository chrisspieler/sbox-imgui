using Duccsoft.ImGui.Rendering;

namespace Duccsoft.ImGui.Elements;

internal class TextWidget : Element
{
	public TextWidget( Window parent, string text ) : base( parent )
	{
		Text = text;

		Size = ImGui.CalcTextSize( Text );

		OnBegin();
		OnEnd();
	}

	public string Text { get; set; }

	protected override void OnDrawSelf( ImDrawList drawList )
	{
		drawList.AddText( ScreenPosition, ImGui.GetColorU32( ImGuiCol.Text ), Text );
	}
}
