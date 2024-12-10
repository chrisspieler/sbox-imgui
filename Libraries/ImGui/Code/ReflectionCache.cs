using System;
using System.Collections.Generic;
using System.Linq;

namespace Duccsoft.ImGui;

internal class ReflectionCache : IHotloadManaged
{
	private Dictionary<Type, TypeDescription> _typeCache { get; set; } = new();
	private Dictionary<Type, List<PropertyDescription>> _propertyCache { get; set; } = new();
	public TypeDescription GetTypeDescription( Type type )
	{
		ArgumentNullException.ThrowIfNull( type );
		if ( !_typeCache.TryGetValue( type, out var typeDesc ) )
		{
			typeDesc = TypeLibrary.GetType( type );
			if ( typeDesc is null )
				throw new Exception( $"Type {type?.FullName} not found in {nameof( TypeLibrary )}" );
			_typeCache[type] = typeDesc;
		}
		return _typeCache[type];
	}

	public List<PropertyDescription> GetProperties( Type type )
	{
		ArgumentNullException.ThrowIfNull( type );
		if ( !_propertyCache.TryGetValue( type, out var properties ) )
		{
			var typeDesc = GetTypeDescription( type );
			properties = typeDesc.Properties
				.Where( p => p.HasAttribute<PropertyAttribute>() )
				.ToList();
			_propertyCache[type] = properties;
		}
		return _propertyCache[type];
	}

	private void Clear()
	{
		_typeCache?.Clear();
		_propertyCache?.Clear();
		_typeCache ??= new();
		_propertyCache ??= new();
	}

	public void Created( IReadOnlyDictionary<string, object> state ) => Clear();
	public void Persisted() => Clear();
}
