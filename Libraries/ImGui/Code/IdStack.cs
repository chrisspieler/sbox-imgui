using System;
using System.Collections.Generic;

namespace Duccsoft.ImGui;

public class IdStack
{
	private struct HashData
	{
		public HashData( string id )
		{
			StringSource = id;
		}

		public HashData( int id )
		{
			IntSource = id;
		}

		public string StringSource { get; set; }
		public int IntSource { get; set; }
		
		public override int GetHashCode()
		{
			return HashCode.Combine( StringSource, IntSource );
		}
	}

	private Stack<HashData> _data = new();
	private Stack<int> _hashes = new();
	private int GetSeed()
	{
		if ( _hashes.Count == 0 )
		{
			return 0;
		}
		else
		{
			return _hashes.Peek();
		}
	}

	public void Clear()
	{
		_data.Clear();
		_hashes.Clear();
	}

	public int GetHash( string id ) => HashCode.Combine( GetSeed(), id );
	public int GetHash( int id ) => HashCode.Combine( GetSeed(), id );
	private int GetHash( HashData id ) => HashCode.Combine( GetSeed(), id.GetHashCode() );

	public void Push( string id ) => Push( new HashData( id ) );
	public void Push( int id ) => Push( new HashData( id ) );
	private void Push( HashData data )
	{
		_data.Push( data );
		var hash = GetHash( data );
		_hashes.Push( hash );
	}

	public void Pop()
	{
		_data.Pop();
		_hashes.Pop();
	}

}
