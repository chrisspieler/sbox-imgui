using System;
using System.Collections.Generic;
using System.Linq;

namespace Duccsoft.ImGui;

public static class ComponentExtensions
{
	public static void ImGuiInspector( this Component component, bool newWindow = true )
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

		// TODO: Retrieve all TypeDescriptions and PropertyDescriptions from ReflectionCache
		var typeDesc = TypeLibrary.GetType( component.GetType() );
		var name = typeDesc.ClassName;
		var properties = typeDesc.Properties
			.Where( p => p.HasAttribute<PropertyAttribute>() )
			.ToList();
		if ( !newWindow )
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
		{ typeof(float), ImGuiFloatProperty }
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
		ImGui.Text( prop.Name ); ImGui.SameLine();
		Func<float> getter = () => (float)prop.GetValue( component );
		Action<float> setter = v => prop.SetValue( component, v );
		// TODO: Without RangeAttribute, use DragFloat instead of SliderFloat.
		var min = -100_000f;
		var max = 100_000f;
		var range = prop.GetCustomAttribute<RangeAttribute>();
		if ( range is not null )
		{
			min = range.Min;
			max = range.Max;
		}
		ImGui.SliderFloat( prop.Name, getter, setter, min, max );
	}
}
