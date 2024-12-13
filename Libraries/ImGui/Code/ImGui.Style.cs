using System;

namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static float GetFontSize() => (int)(18 * ImGuiStyle.UIScale);

	public static ImGuiStyle GetStyle()
	{
		return ImGuiSystem.Current.Style;
	}

	public static Color32 GetColorU32( ImGuiCol color, float alphaMul = 1.0f )
	{
		var colors = ImGuiSystem.Current.Style.Colors;

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
		style.Colors[ImGuiCol.WindowBg]						= new( 0x0F, 0x0F, 0x0F, 240 );
		style.Colors[ImGuiCol.Border]						= new( 0x42, 0x42, 0x4C, 128 );
		style.Colors[ImGuiCol.Text]							= new( 0xFF, 0xFF, 0xFF );
		style.Colors[ImGuiCol.TitleBg]						= new( 0x0A, 0x0A, 0x0A );
		style.Colors[ImGuiCol.TitleBgActive]				= new( 0x29, 0x4A, 0x7A );
		style.Colors[ImGuiCol.Button]						= new( 66, 150, 250, 102 );
		style.Colors[ImGuiCol.ImGuiColButtonHovered]		= new( 66, 150, 250 );
		style.Colors[ImGuiCol.ButtonActive]					= new( 15, 135, 250 );
		style.Colors[ImGuiCol.FrameBg]						= new( 41, 74, 122, 138 );
		style.Colors[ImGuiCol.FrameBgHovered]				= new( 66, 150, 250, 102 );
		style.Colors[ImGuiCol.FrameBgActive]				= new( 66, 150, 250, 171 );
		style.Colors[ImGuiCol.SliderGrab]					= new( 61, 133, 244 );
		style.Colors[ImGuiCol.SliderGrabActive]				= new( 66, 150, 250, 255 );
		style.Colors[ImGuiCol.CheckMark]					= new( 66, 150, 250, 255 );
	}
	#endregion
}
