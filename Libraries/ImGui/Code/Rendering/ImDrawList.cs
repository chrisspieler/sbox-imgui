using Sandbox.Rendering;
using System;

namespace Duccsoft.ImGui.Rendering;

public class ImDrawList
{
	public ImDrawList( string name )
	{
		CommandList = new CommandList( $"ImGui DrawList {name}" )
		{
			Flags = CommandList.Flag.Hud
		};
	}

	public CommandList CommandList { get; private set; }

	#region Rect
	public void AddRect( Vector2 upperLeft, Vector2 lowerRight, Color32 color, float rounding = 0f, ImDrawFlags flags = ImDrawFlags.None, float thickness = 1.0f )
	{
		DrawRect( upperLeft, lowerRight, Color.Transparent, color, rounding, flags, thickness );
	}
	public void AddRectFilled( Vector2 upperLeft, Vector2 lowerRight, Color32 color, float rounding = 0f, ImDrawFlags flags = ImDrawFlags.None )
		=> DrawRect( upperLeft, lowerRight, color, Color.Transparent, rounding, flags, borderThickness: 0f );

	private void DrawRect( Vector2 upperLeft, Vector2 lowerRight, Color fillColor, Color borderColor, float rounding, ImDrawFlags flags, float borderThickness )
	{
		// Transform
		CommandList.Set( "BoxPosition", upperLeft );
		CommandList.Set( "BoxSize", lowerRight - upperLeft );

		// Background
		CommandList.SetCombo( "D_BACKGROUND_IMAGE", 0 );
		if ( borderThickness >= 1f )
		{
			// Border
			CommandList.Set( "HasBorder", 1 );
			// TODO: Use ImDrawFlags to determine which borders are rounded.
			CommandList.Set( "BorderSize", borderThickness );
			CommandList.Set( "BorderRadius", rounding );
			CommandList.Set( "BorderColorL", borderColor );
			CommandList.Set( "BorderColorT", borderColor );
			CommandList.Set( "BorderColorR", borderColor );
			CommandList.Set( "BorderColorB", borderColor );
			CommandList.SetCombo( "D_BORDER_IMAGE", 0 );
		}

		CommandList.DrawQuad( new Rect( upperLeft, lowerRight - upperLeft ), Material.UI.Box, fillColor );
	}
	#endregion

	#region Triangle
	//public void AddTriangleFilled( Vector2 p1, Vector2 p2, Vector3 p3, Color32 color )
	//{
	//	throw new NotImplementedException();
	//}
	#endregion

	#region Text
	private static TextRendering.Scope TextScope( string text, Color color ) 
		=> new( text, color, ImGui.GetTextLineHeight(), "Consolas" );

	public void AddText( Vector2 pos, Color32 color, string text, TextFlag flags = TextFlag.LeftTop )
		=> DrawText( new Rect( pos, 1f ), TextScope( text, color ), flags );

	private void DrawText( Rect rect, TextRendering.Scope scope, TextFlag flags )
	{
		var textTexture = TextRendering.GetOrCreateTexture( in scope, clip: default, flags );
		if ( !textTexture.IsValid() )
			return;

		CommandList.Set( "TextureIndex", textTexture.Index );
		var size = textTexture.Size;
		rect = rect.Align( size, flags );
		CommandList.DrawQuad( rect, Material.FromShader( "shaders/ui_text.shader" ), Color.White );
	}
	#endregion

	#region Image
	public void AddImage( Texture texture, Vector2 upperLeft, Vector2 lowerRight, Vector2 uv0, Vector2 uv1, Color32 tintColor )
		=> DrawImage( texture, upperLeft, lowerRight, uv0, uv1, tintColor );

	public void AddImage( Texture texture, Vector2 upperLeft, Vector2 lowerRight )
		=> AddImage( texture, upperLeft, lowerRight, uv0: new Vector2( 0, 0 ), uv1: new Vector2( 1, 1 ), tintColor: Color.White );

	private void DrawImage( Texture texture, Vector2 upperLeft, Vector2 lowerRight, Vector2 uv0, Vector2 uv1, Color32 tintColor )
	{
		if ( !texture.IsValid() )
			return;

		// Transform
		CommandList.Set( "BoxPosition", upperLeft );
		CommandList.Set( "BoxSize", lowerRight - upperLeft );

		// Background
		CommandList.SetCombo( "D_BACKGROUND_IMAGE", 1 );
		CommandList.Set( "BgRepeat", -1 );
		CommandList.Set( "TextureIndex", texture.Index );
		var texToRectScale = 1f / (texture.Size / (lowerRight - upperLeft));
		var offset = uv0 * texture.Size * texToRectScale;
		var size = uv1 * texture.Size * texToRectScale - offset;
		var bgPos = new Vector4( offset.x, offset.y, size.x, size.y );
		CommandList.Set( "BgPos", bgPos );

		// Border
		CommandList.Set( "HasBorder", 0 );

		CommandList.DrawQuad( new Rect( upperLeft, lowerRight - upperLeft ), Material.FromShader( "shaders/imgui_rect.shader"), tintColor );
	}
	#endregion Image
}
