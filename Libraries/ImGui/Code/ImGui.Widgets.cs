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

	public static bool Checkbox( string label, ref bool value )
	{
		var checkbox = new Checkbox( CurrentWindow, label, ref value );
		return checkbox.IsReleased;
	}

	public static bool DragInt( string label, ref int value, float speed = 1.0f, int min = 0, int max = 0, string format = null, ImGuiSliderFlags flags = 0 )
	{
		_ = new DragInt( CurrentWindow, label, ref value, speed, min, max, format, flags );
		// TODO: Is returning true correct?
		return true;
	}

	public static bool SliderFloat( string label, ref float value, float min, float max, string format = "F3", ImGuiSliderFlags flags = 0 )
	{
		_ = new SliderBar<float>( CurrentWindow, 0, ref value, min, max, format );
		return true;
	}

	public static bool SliderFloat2( string label, ref Vector2 value, float min, float max, string format = "F3", ImGuiSliderFlags flags = 0 )
	{
		throw new NotImplementedException();
	}

	public static bool SliderFloat3( string label, ref Vector3 value, float min, float max, string format = "F3", ImGuiSliderFlags flags = 0 )
	{
		throw new NotImplementedException();
	}

	public static bool SliderFloat4( string label, ref Vector4 value, float min, float max, string format = "F3", ImGuiSliderFlags flags = 0 )
	{
		throw new NotImplementedException();
	}

	public static bool SliderInt( string label, ref int value, int min, int max, string format = null, ImGuiSliderFlags flags = 0 )
	{
		_ = new SliderBar<int>( CurrentWindow, 0, ref value, min, max, format );
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
