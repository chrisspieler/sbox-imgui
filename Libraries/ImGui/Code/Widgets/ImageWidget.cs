namespace Duccsoft.ImGui;

internal class ImageWidget : Widget
{
    public ImageWidget( Window parent, Texture texture, Vector2 size, 
		Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor  ) 
		: base( parent )
    {
		ColorTexture = texture;
		ImageSize = size;
		UV0 = uv0;
		UV1 = uv1;
		TintColor = tintColor;
		BorderColor = borderColor;
		Show();
    }

	public Texture ColorTexture { get; set; }
	public Vector2 ImageSize { get; set; }
	public Vector2 UV0 { get; set; }
	public Vector2 UV1 { get; set; }
	public Color TintColor { get; set; }
	public Color BorderColor { get; set; }

	public override Vector2 Size => ImageSize;

    public override void Paint(ImGuiPainter painter)
    {
		painter.DrawRect( ScreenRect, ColorTexture, TintColor, UV0, UV1, cornerRadius: default, borderWidth: new Vector4( 2f ), borderColor: BorderColor );
    }
}
