using System.Collections.Generic;

namespace Duccsoft.ImGui;

internal class ReflectionCache : IHotloadManaged
{
	private void Clear()
	{

	}

	public void Created( IReadOnlyDictionary<string, object> state )
	{
		Log.Info( "Created" );
		Clear();
	}

	public void Persisted()
	{
		Log.Info( "Persisted" );
		Clear();
	}
}
