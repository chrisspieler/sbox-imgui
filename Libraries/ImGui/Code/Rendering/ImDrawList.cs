using System;

namespace Duccsoft.ImGui.Rendering;

public class ImDrawList
{
	protected Action Actions { get; set; }
	public int Count => Actions?.GetInvocationList()?.Length ?? 0;

	public void Clear()
	{
		Actions = null;
	}

	public void Render()
	{
		if ( !Graphics.IsActive )
		{
			Log.Error( $"{nameof( ImDrawList )}.{nameof( Render )}() called outside of a render block!" );
			return;
		}
		Actions?.Invoke();
		Clear();
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
		var attributes = new RenderAttributes();

		// Transform
		attributes.Set( "BoxPosition", upperLeft );
		attributes.Set( "BoxSize", lowerRight - upperLeft );

		// Background
		attributes.SetCombo( "D_BACKGROUND_IMAGE", 0 );
		if ( borderThickness >= 1f )
		{
			// Border
			attributes.Set( "HasBorder", 1 );
			// TODO: Use ImDrawFlags to determine which borders are rounded.
			attributes.Set( "BorderSize", borderThickness );
			attributes.Set( "BorderRadius", rounding );
			attributes.Set( "BorderColorL", borderColor );
			attributes.Set( "BorderColorT", borderColor );
			attributes.Set( "BorderColorR", borderColor );
			attributes.Set( "BorderColorB", borderColor );
			attributes.SetCombo( "D_BORDER_IMAGE", 0 );
		}

		Graphics.DrawQuad( new Rect( upperLeft, lowerRight - upperLeft ), Material.UI.Box, fillColor, attributes );
	}
	#endregion

	#region Triangle
	public void AddTriangleFilled( Vector2 p1, Vector2 p2, Vector3 p3, Color32 color )
	{
		throw new NotImplementedException();
	}
	#endregion

	#region Text
	private static TextRendering.Scope TextScope( string text, Color color ) 
		=> new( text, color, ImGui.GetTextLineHeight(), "Consolas" );

	public void AddText( Vector2 pos, Color32 color, string text, TextFlag flags = TextFlag.LeftTop )
		=> Actions += () => Graphics.DrawText( new Rect( pos, 1f ), TextScope( text, color ), flags );
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

		var attributes = new RenderAttributes();
		attributes.Clear();

		// Transform
		attributes.Set( "BoxPosition", upperLeft );
		attributes.Set( "BoxSize", lowerRight - upperLeft );

		// Background
		attributes.SetCombo( "D_BACKGROUND_IMAGE", 1 );
		attributes.Set( "BgRepeat", -1 );
		attributes.Set( "Texture", texture );
		var texToRectScale = 1f / (texture.Size / (lowerRight - upperLeft));
		var offset = uv0 * texture.Size * texToRectScale;
		var size = uv1 * texture.Size * texToRectScale - offset;
		var bgPos = new Vector4( offset.x, offset.y, size.x, size.y );
		attributes.Set( "BgPos", bgPos );

		// Border
		attributes.Set( "HasBorder", 0 );

		Graphics.DrawQuad( new Rect( upperLeft, lowerRight - upperLeft ), Material.UI.Box, tintColor, attributes );
	}
	#endregion Image
}
