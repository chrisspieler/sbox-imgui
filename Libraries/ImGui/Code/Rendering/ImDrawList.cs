using System;

namespace Duccsoft.ImGui.Rendering;

public class ImDrawList
{
	protected Action Actions { get; set; }
	public void Clear() => Actions = null;
	public int Count => Actions.GetInvocationList().Length;

	public void Render()
	{
		if ( !Graphics.IsActive )
		{
			Log.Error( $"{nameof( ImDrawList )}.{nameof( Render )}() called outside of a render block!" );
			return;
		}
		Actions?.Invoke();
	}

	#region Rect
	public void AddRect( Vector2 upperLeft, Vector2 lowerRight, Color32 color, float rounding = 0f, ImDrawFlags flags = ImDrawFlags.None, float thickness = 1.0f )
	{
		Actions += () => DrawRect( upperLeft, lowerRight, Color.Transparent, color, rounding, flags, thickness );
	}
	public void AddRectFilled( Vector2 upperLeft, Vector2 lowerRight, Color32 color, float rounding = 0f, ImDrawFlags flags = ImDrawFlags.None )
		=> Actions += () => DrawRect( upperLeft, lowerRight, color, Color.Transparent, rounding, flags, borderThickness: 0f );

	private void DrawRect( Vector2 upperLeft, Vector2 lowerRight, Color fillColor, Color borderColor, float rounding, ImDrawFlags flags, float borderThickness )
	{
		Graphics.Attributes.Clear();

		// Transform
		Graphics.Attributes.Set( "BoxPosition", upperLeft );
		Graphics.Attributes.Set( "BoxSize", lowerRight - upperLeft );

		// Background
		Graphics.Attributes.SetCombo( "D_BACKGROUND_IMAGE", 0 );

		if ( borderThickness >= 1f )
		{
			// Border
			Graphics.Attributes.Set( "HasBorder", 1 );
			// TODO: Use ImDrawFlags to determine which borders are rounded.
			Graphics.Attributes.Set( "BorderSize", borderThickness );
			Graphics.Attributes.Set( "BorderRadius", rounding );
			Graphics.Attributes.Set( "BorderColorL", borderColor );
			Graphics.Attributes.Set( "BorderColorT", borderColor );
			Graphics.Attributes.Set( "BorderColorR", borderColor );
			Graphics.Attributes.Set( "BorderColorB", borderColor );
			Graphics.Attributes.SetCombo( "D_BORDER_IMAGE", 0 );
		}

		Graphics.DrawQuad( new Rect( upperLeft, lowerRight - upperLeft ), Material.UI.Box, fillColor );
	}
	#endregion

	#region Triangle
	public void AddTriangleFilled( Vector2 p1, Vector2 p2, Vector3 p3, Color32 color )
	{

	}
	#endregion

	#region Text
	public void AddText( Vector2 pos, Color32 color, string text, TextFlag flags = TextFlag.LeftTop )
		=> Actions += () => DrawText( TextScope( text, color ), pos, flags );

	private static TextRendering.Scope TextScope( string text, Color color ) 
		=> new TextRendering.Scope( text, color, ImGui.GetTextLineHeight(), "Consolas" );
	private static Material TextMaterial = Material.FromShader( "shaders/ui_text.shader" );

	private void DrawText( TextRendering.Scope scope, Vector2 point, TextFlag flags )
	{
		Texture textTexture = TextRendering.GetOrCreateTexture( in scope, default, flags );
		if ( textTexture is null || !textTexture.IsLoaded )
			return;

		Graphics.Attributes.Clear();

		Graphics.Attributes.Set( "TextureIndex", textTexture.Index );
		var textRect = new Rect( point, 1f );
		textRect = textRect.Align( textTexture.Size, flags );
		Graphics.DrawQuad( textRect, TextMaterial, Color.White );
	}
	#endregion

	#region Image
	public void AddImage( Texture texture, Vector2 upperLeft, Vector2 lowerRight, Vector2 uv0, Vector2 uv1, Color32 tintColor )
		=> Actions += () => DrawImage( texture, upperLeft, lowerRight, uv0, uv1, tintColor );

	public void AddImage( Texture texture, Vector2 upperLeft, Vector2 lowerRight )
		=> AddImage( texture, upperLeft, lowerRight, uv0: new Vector2( 0, 0 ), uv1: new Vector2( 1, 1 ), tintColor: Color.White );

	private void DrawImage( Texture texture, Vector2 upperLeft, Vector2 lowerRight, Vector2 uv0, Vector2 uv1, Color32 tintColor )
	{
		if ( !texture.IsValid() )
			return;

		Graphics.Attributes.Clear();

		// Transform
		Graphics.Attributes.Set( "BoxPosition", upperLeft );
		Graphics.Attributes.Set( "BoxSize", lowerRight - upperLeft );

		// Background
		Graphics.Attributes.SetCombo( "D_BACKGROUND_IMAGE", 1 );
		Graphics.Attributes.Set( "BgRepeat", -1 );
		Graphics.Attributes.Set( "Texture", texture );
		var texToRectScale = 1f / (texture.Size / (lowerRight - upperLeft));
		var offset = uv0 * texture.Size * texToRectScale;
		var size = uv1 * texture.Size * texToRectScale - offset;
		var bgPos = new Vector4( offset.x, offset.y, size.x, size.y );
		Graphics.Attributes.Set( "BgPos", bgPos );

		// Border
		Graphics.Attributes.Set( "HasBorder", 0 );

		Graphics.DrawQuad( new Rect( upperLeft, lowerRight - upperLeft ), Material.UI.Box, tintColor );
	}
	#endregion Image
}
