using System.Reflection;

namespace pathmage.KnightmareEngine;

public interface Plugin
{
	static void Load(Assembly assembly)
	{
		foreach (var type in assembly.GetTypesWithInterface<ISceneLoadHelper>())
		{
			print(type.ToText());
		}
	}

	interface ISceneLoadHelper;
}

public interface Plugin<TSelf> : Plugin
	where TSelf : Plugin<TSelf>
{
	interface Scene { }

	interface ISceneLoadHelper<TScene> : Scene, ISceneLoadHelper
		where TScene : Node, ISceneLoadHelper<TScene> { }
}
