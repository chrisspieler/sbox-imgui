using Duccsoft.ImGui.Rendering;
using System;

namespace Duccsoft.ImGui.Elements;

public class Checkbox : Element
{
	public Checkbox( Element parent, string label, ref bool isChecked ) : base( parent )
	{
		Checked = isChecked;
		Label = label;

		OnBegin();
		OnEnd();

		isChecked = Checked;
	}

	public string Label { get; set; }
	public bool Checked { get; set; }

	private Color32 CheckMarkColor => ImGui.GetColorU32( ImGuiCol.CheckMark );

	private Rect CheckboxRect
	{
		get => new( ScreenPosition, new Vector2( ImGui.GetTextLineHeightWithSpacing() ) );
	}

	private Rect TextRect
	{
		get
		{
			var textPos = CheckboxRect.TopRight + new Vector2( Style.ItemInnerSpacing.x, 0f );
			return new Rect( textPos, ImGui.CalcTextSize( Label ) );
		}
	}

	public override Vector2 Size
	{
		get => new Vector2( CheckboxRect.Size.x, 0f )
				+ new Vector2( Style.ItemInnerSpacing.x, 0f )
				+ TextRect.Size;
	}

	public override void OnEnd()
	{
		base.OnEnd();

		if ( IsReleased )
		{
			Checked = !Checked;
		}
	}

	protected override void OnDrawSelf( ImDrawList drawList )
	{
		drawList.AddRectFilled( CheckboxRect.TopLeft, CheckboxRect.BottomRight, FrameColor );
		if ( Checked )
		{
			drawList.AddText( CheckboxRect.Center, CheckMarkColor, "✓", TextFlag.Center );
		}

		drawList.AddText( TextRect.Position, TextColor, Label );
	}
}
