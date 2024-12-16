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

		var typeDesc = ImGuiSystem.Current.GetTypeDescription( component.GetType() );
		var properties = ImGuiSystem.Current.GetProperties( component.GetType() );
		if ( ImGui.CurrentWindow is not null )
		{
			PrintProperties( properties );
		}
		else
		{
			if ( ImGui.Begin( typeDesc.ClassName ) )
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
		{ typeof(bool), ImGuiBoolProperty },
		{ typeof(Vector2), ImGuiVector2Property },
		{ typeof(Vector3), ImGuiVector3Property },
		{ typeof(Vector4), ImGuiVector4Property },
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

	private static void ImGuiVector2Property( Component component, PropertyDescription prop )
	{
		var range = prop.GetCustomAttribute<RangeAttribute>();
		if ( range is not null )
		{
			ImGuiSliderFloat2Property( component, prop, range.Min, range.Max );
		}
		else
		{
			// TODO: Add DragFloat2
		}
	}

	private static void ImGuiSliderFloat2Property( Component component, PropertyDescription prop, float min, float max )
	{
		ImGui.Text( prop.Name ); ImGui.SameLine();
		var value = (Vector2)prop.GetValue( component );
		ImGui.SliderFloat2( prop.Name, ref value, min, max );
		prop.SetValue( component, value );
	}

	private static void ImGuiVector3Property( Component component, PropertyDescription prop )
	{
		var range = prop.GetCustomAttribute<RangeAttribute>();
		if ( range is not null )
		{
			ImGuiSliderFloat3Property( component, prop, range.Min, range.Max );
		}
		else
		{
			// TODO: Add DragFloat3
		}
	}

	private static void ImGuiSliderFloat3Property( Component component, PropertyDescription prop, float min, float max )
	{
		ImGui.Text( prop.Name ); ImGui.SameLine();
		var value = (Vector3)prop.GetValue( component );
		ImGui.SliderFloat3( prop.Name, ref value, min, max );
		prop.SetValue( component, value );
	}

	private static void ImGuiVector4Property( Component component, PropertyDescription prop )
	{
		var range = prop.GetCustomAttribute<RangeAttribute>();
		if ( range is not null )
		{
			ImGuiSliderFloat4Property( component, prop, range.Min, range.Max );
		}
		else
		{
			// TODO: Add DragFloat4
		}
	}

	private static void ImGuiSliderFloat4Property( Component component, PropertyDescription prop, float min, float max )
	{
		ImGui.Text( prop.Name ); ImGui.SameLine();
		var value = (Vector4)prop.GetValue( component );
		ImGui.SliderFloat4( prop.Name, ref value, min, max );
		prop.SetValue( component, value );
	}
}
