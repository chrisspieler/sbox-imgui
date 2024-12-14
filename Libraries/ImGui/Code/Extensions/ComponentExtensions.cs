using Sandbox.Diagnostics;
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
		Action<float> setter = v => prop.SetValue( component, v );
		ImGui.SliderFloat( prop.Name, value, setter, min, max, "F3" );
	}

	private static void ImGuiIntProperty( Component component, PropertyDescription prop )
	{
		var range = prop.GetCustomAttribute<RangeAttribute>();
		if ( range is not null )
		{
			// TODO: Draw SliderInt
		}
		else
		{
			ImGuiDragIntProperty( component, prop );
		}
	}

	private static void ImGuiDragIntProperty( Component component, PropertyDescription prop )
	{
		ImGui.Text( prop.Name ); ImGui.SameLine();
		Func<int> getter = () => (int)prop.GetValue( component );
		Action<int> setter = v => prop.SetValue( component, v );
		ImGui.DragInt( prop.Name, getter, setter, 0.2f );
	}

	private static void ImGuiBoolProperty( Component component, PropertyDescription prop )
	{
		Action<bool> setter = v => prop.SetValue( component, v );
		ImGui.Checkbox( prop.Name, (bool)prop.GetValue( component ), setter );
	}
}
