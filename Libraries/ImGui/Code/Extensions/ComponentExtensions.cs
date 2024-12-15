﻿using Sandbox.Diagnostics;
using System;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

public static class ComponentExtensions
{
	public static void ImGuiInspector( this Component component )
	{
		void PrintProperties( List<PropertyDescription> properties )
		{
			for ( int i = 0; i < properties.Count; i++ )
			{
				ImGui.PushID( i );
				ImGuiProperty( component, properties[i] );
				ImGui.PopID();
			}
		}
		if ( !component.IsValid() )
			return;

		var sw = FastTimer.StartNew();
		var typeDesc = ImGuiSystem.Current.GetTypeDescription( component.GetType() );
		var name = typeDesc.ClassName;
		var properties = ImGuiSystem.Current.GetProperties( component.GetType() );
		if ( ImGui.CurrentWindow is not null )
		{
			PrintProperties( properties );
		}
		else
		{
			if ( ImGui.Begin( typeDesc.Name ) )
			{
				PrintProperties( properties );
			}
			ImGui.End();
		}
	}

	private static Dictionary<Type, Action<Component, PropertyDescription>> _propertyPrintStrategy = new()
	{
		{ typeof(float), ImGuiFloatProperty },
		{ typeof(int), ImGuiIntProperty },
		{ typeof(bool), ImGuiBoolProperty }
	};

	public static void ImGuiProperty( this Component component, PropertyDescription prop )
	{
		if ( !component.IsValid() || prop is null )
			return;

		if ( !_propertyPrintStrategy.TryGetValue( prop.PropertyType, out var strategy ) )
			return;

		strategy( component, prop );
	}

	private static void ImGuiFloatProperty( Component component, PropertyDescription prop )
	{
		var range = prop.GetCustomAttribute<RangeAttribute>();
		if ( range is not null )
		{
			ImGuiSliderFloatProperty( component, prop, range.Min, range.Max );
		}
		else
		{
			// TODO: Draw DragFloat
		}
	}

	private static void ImGuiSliderFloatProperty( Component component, PropertyDescription prop, float min, float max )
	{
		ImGui.Text( prop.Name ); ImGui.SameLine();
		var value = (float)prop.GetValue( component );
		ImGui.SliderFloat( prop.Name, ref value, min, max, "F3" );
		prop.SetValue( component, value );
	}

	private static void ImGuiIntProperty( Component component, PropertyDescription prop )
	{
		var range = prop.GetCustomAttribute<RangeAttribute>();
		if ( range is not null )
		{
			ImGuiSliderIntProperty( component, prop, (int)range.Min, (int)range.Max );
		}
		else
		{
			ImGuiDragIntProperty( component, prop );
		}
	}

	private static void ImGuiSliderIntProperty( Component component, PropertyDescription prop, int min, int max )
	{
		ImGui.Text( prop.Name ); ImGui.SameLine();
		var value = (int)prop.GetValue( component );
		ImGui.SliderInt( prop.Name, ref value, min, max );
		prop.SetValue( component, value );
	}

	private static void ImGuiDragIntProperty( Component component, PropertyDescription prop )
	{
		ImGui.Text( prop.Name ); ImGui.SameLine();
		var value = (int)prop.GetValue( component );
		ImGui.DragInt( prop.Name, ref value, 0.2f );
		prop.SetValue( component, value );
	}

	private static void ImGuiBoolProperty( Component component, PropertyDescription prop )
	{
		var value = (bool)prop.GetValue( component );
		ImGui.Checkbox( prop.Name, ref value );
		prop.SetValue( component, value );
	}
}
