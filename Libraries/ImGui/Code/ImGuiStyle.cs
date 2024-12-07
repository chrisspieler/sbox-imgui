using System;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

public class ImGuiStyle
{
	public static float UIScale => MathF.Min( Screen.Width, Screen.Height ) * Screen.DesktopScale / 1080f;
	public Vector2 WindowPadding
	{
		get => _windowPadding * UIScale;
		set => _windowPadding = value;
	}
	private Vector2 _windowPadding = new Vector2( 8, 8 );
	public Vector2 FramePadding
	{
		get => _framePadding * UIScale;
		set => _framePadding = value;
	}
	private Vector2 _framePadding = new Vector2( 4, 3 );
	public Vector2 ItemSpacing
	{
		get => _itemSpacing * UIScale;
		set => _itemSpacing = value;
	}
	private Vector2 _itemSpacing = new Vector2( 8, 4 );
	public Vector2 ItemInnerSpacing
	{
		get => _itemInnerSpacing * UIScale;
		set => _itemInnerSpacing = value;
	}
	private Vector2 _itemInnerSpacing = new Vector2( 4, 4 );
	public float IndentSpacing
	{
		get => _indentSpacing * UIScale;
		set => _indentSpacing = value;
	}
	private float _indentSpacing = 21f;
	public float GrabMinSize
	{
		get => _grabMinSize * UIScale;
		set => _grabMinSize = value;
	}
	private float _grabMinSize = 12f;

	public Dictionary<ImGuiCol, Color32> Colors = new();
}
