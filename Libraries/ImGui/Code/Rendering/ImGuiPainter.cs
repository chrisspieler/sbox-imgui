using Sandbox.Rendering;

namespace Duccsoft;

/// <summary>
/// Wraps a HudPainter to extend its drawing functionality.
/// </summary>
internal readonly ref struct ImGuiPainter
{
	public ImGuiPainter( CommandList list )
	{
		_hudPainter = new HudPainter( list );
	}
	public readonly CommandList CommandList => _hudPainter.list;

	private readonly HudPainter _hudPainter;

	public readonly void DrawRect( in Rect rect, in Color color, in Vector4 cornerRadius = default, in Vector4 borderWidth = default, in Color borderColor = default )
	{
		_hudPainter.DrawRect( rect, color, cornerRadius, borderWidth, borderColor );
	}

	public readonly void DrawText( in string text, in Rect rect )
	{
		var textColor = ImGui.GetColorU32( ImGuiCol.ImGuiCol_Text );
		var scope = new TextRendering.Scope( text, textColor, ImGui.GetFontSize(), "Consolas" );
		_hudPainter.DrawText( scope, rect );
	}
}
