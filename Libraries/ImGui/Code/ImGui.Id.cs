namespace Duccsoft.ImGui;

public static partial class ImGui
{
	public static void PushID( string id )
	{
		IdStack.Push( id );
	}

	public static void PushID( int id )
	{
		IdStack.Push( id );
	}

	public static void PopID()
	{
		IdStack.Pop();
	}

	public static int GetID( string id )
	{
		return IdStack.GetHash( id );
	}

	public static int GetID( int id )
	{
		return IdStack.GetHash( id );
	}
}
