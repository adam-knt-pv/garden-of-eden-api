using System.Reflection;

namespace pathmage.KnightmareEngine;

public interface Plugin
{
	static void Load(Assembly assembly, string local_path)
	{
		foreach (var type in assembly.GetTypesWithInterface<Scene>())
		{
			print(type.ToText());
		}
	}

	interface Scene { }
}

public interface Plugin<TSelf> : Plugin
	where TSelf : Plugin<TSelf>
{
	static void Load() => Load(typeof(TSelf).Assembly, "res://");

	new interface Scene : Plugin.Scene { }

	interface Scene<TScene> : Scene
		where TScene : Node, Scene<TScene> { }
}
