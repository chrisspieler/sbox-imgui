﻿using System;

namespace Duccsoft;

public static partial class ImGui
{
	private static float GetUIScale => MathF.Min( Screen.Width, Screen.Height ) * Screen.DesktopScale;
	public static float GetFontSize() => (int)(0.012f * GetUIScale);

	public static ImGuiStyle GetStyle()
	{
		return ImGuiRenderSystem.Current.Style;
	}

	public static Color32 GetColorU32( ImGuiCol color, float alphaMul = 1.0f )
	{
		var colors = ImGuiRenderSystem.Current.Style.Colors;

		if ( colors is null || !colors.TryGetValue( color, out Color32 styleColor ) )
			return new Color32( 0xFF, 0x00, 0xFF, (byte)(0xFF * alphaMul) );

		return styleColor with { a = (byte)(styleColor.a * alphaMul) };
	}

	#region Style Colors
	public static void StyleColorsDark( ImGuiStyle style )
	{
		if ( style is null )
			return;

		style.Colors ??= new();
		style.Colors[ImGuiCol.ImGuiCol_WindowBg] = new( 0x0F, 0x0F, 0x0F );
		style.Colors[ImGuiCol.ImGuiCol_Border] = new( 0x42, 0x42, 0x4C );
		style.Colors[ImGuiCol.ImGuiCol_Text] = new( 0xFF, 0xFF, 0xFF );
		style.Colors[ImGuiCol.ImGuiCol_TitleBg] = new( 0x0A, 0x0A, 0x0A );
		style.Colors[ImGuiCol.ImGuiCol_TitleBgActive] = new( 0x29, 0x4A, 0x7A );
	}
	#endregion
}