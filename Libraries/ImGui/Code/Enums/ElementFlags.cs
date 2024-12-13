using System;

namespace Duccsoft.ImGui;

[Flags]
public enum ElementFlags
{
	None		= 0,
	IsHovered	= 1 << 0,
	IsFocused	= 1 << 1,
	IsActive	= 1 << 2,
	IsDragged	= 1 << 3,
	IsVisible	= 1 << 4,
}

public static class ElementFlagsExtensions
{
	public static bool IsHovered( this ElementFlags flags ) => flags.HasFlag( ElementFlags.IsHovered );
	public static bool IsFocused( this ElementFlags flags ) => flags.HasFlag( ElementFlags.IsFocused );
	public static bool IsActive( this ElementFlags flags ) => flags.HasFlag( ElementFlags.IsActive );
	public static bool IsDragged( this ElementFlags flags ) => flags.HasFlag( ElementFlags.IsDragged );
	public static bool IsVisible( this ElementFlags flags ) => flags.HasFlag( ElementFlags.IsVisible );
}
