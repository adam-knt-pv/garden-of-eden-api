using System.Reflection;

namespace pathmage.KnightmareEngine;

public interface Plugin
{
	static void Load() { }

	interface ISceneLoadHelper;
}

public interface Plugin<TSelf> : Plugin
	where TSelf : Plugin<TSelf>
{
	interface Scene { }

	interface ISceneLoadHelper<TScene> : Scene, ISceneLoadHelper
		where TScene : Node, ISceneLoadHelper<TScene>
	{
		internal static int ID;
	}
}
