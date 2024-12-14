﻿using System;
using Duccsoft.ImGui.Elements;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static bool IsItemClicked( ImGuiMouseButton button = ImGuiMouseButton.Left )
	{
		return System.ClickedElementId == CurrentItem?.Id;
	}

	public static void Text( string formatString, params object[] args )
	{
		var text = string.Format( formatString, args );
		_ = new TextWidget( CurrentWindow, text );
	}

	public static bool Button( string label, Vector2 size = default )
	{
		var button = new ButtonWidget( CurrentWindow, label );
		return button.IsReleased;
	}

	public static bool Checkbox( string label, bool value, Action<bool> valueChanged )
	{
		var checkbox = new Checkbox( CurrentWindow, label, value, valueChanged );
		return checkbox.IsReleased;
	}

	public static bool DragInt( string label, Func<int> getter, Action<int> setter, float speed = 1.0f, int min = 0, int max = 0, string format = null, ImGuiSliderFlags flags = 0 )
	{
		_ = new DragInt( CurrentWindow, label, getter, setter, speed, min, max, format, flags );
		// TODO: Is returning true correct?
		return true;
	}

	public static bool SliderFloat( string label, float value, Action<float> setter, float min, float max, string format = null, ImGuiSliderFlags flags = 0 )
	{
		_ = new SliderBar<float>( CurrentWindow, 0, value, setter, min, max, format );
		return true;
	}

	public static bool SliderInt( string label, int value, Action<int> setter, int min, int max, string format = null, ImGuiSliderFlags flags = 0 )
	{
		_ = new SliderBar<int>( CurrentWindow, 0, value, setter, min, max, format );
		return true;
	}

	public static void Image( Texture texture, Vector2 size, Color tintColor, Color borderColor )
	{
		Image( texture, size, Vector2.Zero, Vector2.One, tintColor, borderColor );
	}

	public static void Image( Texture texture, Vector2 size, Vector2 uv0, Vector2 uv1, Color tintColor, Color borderColor )
	{
		_ = new ImageWidget( CurrentWindow, texture, size, uv0, uv1, tintColor, borderColor );
	}
}
