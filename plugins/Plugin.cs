using System.Collections.Frozen;
using System.Collections.Generic;
using System.Reflection;

namespace pathmage.KnightmareEngine;

public interface Plugin
{
	static void Load(Assembly assembly, string local_path)
	{
		var scene_types = assembly.GetTypesWithInterface<Scene>();
		var scene_files = local_path.FindSceneFiles("pathmage.KnightmareEngine");
		var scenes = Vec<(Type Type, string LocalPath)>.With(scene_types.Count);

		foreach (var type in scene_types)
		{
			foreach (var i in scene_files.Count)
			{
				if (scene_files[i].EndsWith($"{type.Name.ToSnakeCase()}.tscn"))
				{
					scenes.Append((type, scene_files[i]));
					scene_files.Remove(i);
					break;
				}
			}
		}

		var plugin_type = assembly.GetTypesWithInterface<Plugin>()[0];
		var plugin_gen_type = typeof(Plugin<>).MakeGenericType(plugin_type);
		var plugin_scene_type = plugin_gen_type.GetNestedType("Scene")!.MakeGenericType(plugin_type);

		var plugin_scenes = new PackedScene[scene_types.Count];
		plugin_scene_type
			.GetField("scenes", Constants.Reflection.BindingFlagsAllMembers)!
			.SetValue(null, plugin_scenes);

		var scene_to_id = new List<KeyValuePair<string, int>>();

		foreach (var i in scenes.Count)
		{
			plugin_scenes[i] = GD.Load<PackedScene>(scenes[i].LocalPath);

			var plugin_scene_gen_type = plugin_gen_type
				.GetNestedType("Scene`1")!
				.MakeGenericType(plugin_type, scenes[i].Type);

			plugin_scene_gen_type
				.GetField("id", Constants.Reflection.BindingFlagsAllMembers)!
				.SetValue(null, i);

			scene_to_id.Add(new(scenes[i].Type.Name, i));
		}

		plugin_gen_type.GetField("scene_to_id")!.SetValue(null, scene_to_id.ToFrozenDictionary());
	}

	interface Scene { }
}

public interface Plugin<TPlugin> : Plugin
	where TPlugin : Plugin<TPlugin>
{
	static FrozenDictionary<string, int> scene_to_id = null!;

	static void Load() => Load(typeof(TPlugin).Assembly, "res://");

	new interface Scene : Plugin.Scene
	{
		protected static PackedScene[] scenes = null!;

		static Node New(string scene) => scenes[scene_to_id[scene]].Instantiate();

		static TScene New<TScene>(string scene)
			where TScene : Node => scenes[scene_to_id[scene]].Instantiate<TScene>();
	}

	interface Scene<TScene> : Scene
		where TScene : Node, Scene<TScene>
	{
		private static int id = -1;

		static TScene New() => scenes[id].Instantiate<TScene>();
	}
}
