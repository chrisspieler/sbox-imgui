using Sandbox.Rendering;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

/// <summary>
/// Wraps a HudPainter to extend its drawing functionality.
/// </summary>
internal ref struct ImGuiPainter
{
	public ImGuiPainter( CommandList list )
	{
		_hudPainter = new HudPainter( list );
	}
	public readonly CommandList CommandList => _hudPainter.list;

	private HudPainter _hudPainter { get; set; }

	public readonly void DrawRect( in Rect rect, in Color color, in Vector4 cornerRadius = default, in Vector4 borderWidth = default, in Color borderColor = default )
	{
		_hudPainter.DrawRect( rect, color, cornerRadius, borderWidth, borderColor );
	}

	public readonly void DrawRect( in Rect rect, Texture colorTex, Color tintColor, in Vector2 uv0, in Vector2 uv1, in Vector4 cornerRadius = default, in Vector4 borderWidth = default, in Color borderColor = default )
    {
		CommandList.Set( "BoxPosition", new Vector2( rect.Left, rect.Top ) );
		CommandList.Set( "BoxSize", new Vector2( rect.Width, rect.Height ) );
		CommandList.Set( "BorderRadius", cornerRadius );
		// Just get *something* to display on screen for now.
		CommandList.GrabFrameTexture( "Texture" );
		// Uncomment after resolution to: https://github.com/Facepunch/sbox-issues/issues/7139
		// CommandList.Set( "Texture", colorTex );
		CommandList.SetCombo( "D_BACKGROUND_IMAGE", 1 );
		var texToRectScale = 1f / ( colorTex.Size / rect.Size);
		var offset = uv0 * colorTex.Size * texToRectScale;
		var size = uv1 * colorTex.Size * texToRectScale - offset;
		var bgPos = new Vector4( offset.x, offset.y, size.x, size.y );
		CommandList.Set( "BgPos", bgPos );
		CommandList.Set( "BgRepeat", -1 );
		if ( !borderWidth.IsNearZeroLength )
        {
			CommandList.Set( "HasBorder", 1 );
			CommandList.SetCombo( "D_BORDER_IMAGE", 0 );
			CommandList.Set( "BorderSize", borderWidth );
			CommandList.Set( "BorderColorL", borderColor );
			CommandList.Set( "BorderColorT", borderColor );
			CommandList.Set( "BorderColorR", borderColor );
			CommandList.Set( "BorderColorB", borderColor );
        }
        else
        {
			CommandList.Set( "HasBorder", 0 );
			CommandList.SetCombo( "D_BORDER_IMAGE", 0 );
		}
		CommandList.DrawQuad( rect, Material.UI.Box, tintColor );
	}

	public readonly void DrawText( in string text, in Rect rect )
	{
		var textColor = ImGui.GetColorU32( ImGuiCol.Text );
		var scope = new TextRendering.Scope( text, textColor, ImGui.GetFontSize(), "Consolas" );
		_hudPainter.DrawText( scope, rect );
	}

	public readonly void DrawText( in string text, in Vector2 position, TextFlag flags = TextFlag.Center )
	{
		var textColor = ImGui.GetColorU32( ImGuiCol.Text );
		var scope = new TextRendering.Scope( text, textColor, ImGui.GetFontSize(), "Consolas" );
		_hudPainter.DrawText( scope, position, flags );
	}
}
