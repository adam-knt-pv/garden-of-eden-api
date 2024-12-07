using System.Reflection;

namespace pathmage.KnightmareEngine;

public interface Plugin
{
	static void Load(Assembly assembly, string local_path)
	{
		var plugin_type = assembly.GetTypesWithInterface<Plugin>()[0];

		var scene_types = assembly.GetTypesWithInterface<Scene>();

		var scene_files = local_path.FindSceneFiles();

		while (scene_types.Count != 0)
		{
			var scene_type = scene_types[0];
			var scene_type_name = scene_type.Name.ToSnakeCase();

			var i = 0;
			while (scene_files.Count != 0)
			{
				if (scene_files[i].EndsWith($"/{scene_type_name}.tscn"))
				{
					print(scene_type.ToText(), scene_type_name, scene_files[i]);
					scene_types.Remove(0);
					scene_files.Remove(i);
					i = 0;
					continue;
				}

				i++;
			}
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
