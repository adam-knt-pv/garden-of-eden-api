using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace pathmage.KnightmareEngine;

public interface Plugin : ToolKit.Globals.Plugin
{
	static void Load(Assembly assembly, string local_path)
	{
		var scene_types = assembly.GetTypesWithInterface<Scene>();
		var scene_files = Files.FindSceneFiles(local_path);
		var scenes = GrowArray<(Type Type, string LocalPath)>.New(scene_types.Count);

		foreach (var type in scene_types)
		{
			foreach (var i in scene_files.Count)
			{
				if (scene_files[i].EndsWith($"{type.Name.ToDashCase()}.tscn"))
				{
					scenes.Append((type, scene_files[i]));
					scene_files.Remove(i);
					break;
				}
			}
		}

		var plugin_impl_type = assembly.GetTypesWithInterface<Plugin>()[0];
		var plugin_gen_type = typeof(Plugin<>).MakeGenericType(plugin_impl_type);

		var plugin_scene_type = plugin_gen_type
			.GetNestedType("Scene")!
			.MakeGenericType(plugin_impl_type);

		var plugin_scenes = new PackedScene[scene_types.Count];
		plugin_scene_type
			.GetField("scenes", Constants.AnyAccessModifierBindingFlags)!
			.SetValue(null, plugin_scenes);

		var scene_to_id = new List<KeyValuePair<string, int>>();

		foreach (var i in scenes.Count)
		{
			plugin_scenes[i] = GD.Load<PackedScene>(scenes[i].LocalPath);

			var plugin_scene_gen_type = plugin_gen_type
				.GetNestedType("Scene`1")!
				.MakeGenericType(plugin_impl_type, scenes[i].Type);

			plugin_scene_gen_type
				.GetField("id", Constants.AnyAccessModifierBindingFlags)!
				.SetValue(null, i);

			scene_to_id.Add(new(scenes[i].Type.Name, i));
		}

		plugin_gen_type
			.GetField("scene_to_id", Constants.AnyAccessModifierBindingFlags)!
			.SetValue(null, scene_to_id.ToFrozenDictionary());
	}

	interface Scene { }
}

public interface Plugin<TPlugin> : Plugin
	where TPlugin : Plugin<TPlugin>
{
	/// <summary>
	/// The current (latest) version of the plugin.
	/// </summary>
	static PluginVersion Version { get; } = TPlugin.version_history[0];

	static string RootPath { get; } = $"user://{Version.ToFilename()}/";

	protected static abstract PluginVersion[] version_history { get; }

	private static FrozenDictionary<string, int> scene_to_id = null!;

	static Plugin()
	{
		DirAccess.MakeDirAbsolute(RootPath);
	}

	static void Load() => Load(typeof(TPlugin).Assembly, "res://");

	/// <summary>
	///
	/// </summary>
	/// <param name="at">Index of the version to get counting from the current version (e.g. 0 - current version, 1 - previous version)</param>
	/// <param name="version"></param>
	/// <returns></returns>
	static bool TryGetPreviousVersion(int at, out PluginVersion version)
	{
		if (TPlugin.version_history.Length > at)
		{
			version = TPlugin.version_history[at];
			return true;
		}

		version = PluginVersion.Null;
		return false;
	}

	new interface Scene : Plugin.Scene
	{
		protected static PackedScene[] scenes = null!;

		static Node New(string scene) => scenes[scene_to_id[scene]].Instantiate();

		static TScene New<TScene>(string scene)
			where TScene : Node => scenes[scene_to_id[scene]].Instantiate<TScene>();

		static PackedScene Packed(string scene) => scenes[scene_to_id[scene]];
	}

	interface Scene<TScene> : Scene
		where TScene : Node, Scene<TScene>
	{
		private static int id = -1;

		static new TScene New => scenes[id].Instantiate<TScene>();

		static new PackedScene Packed => scenes[id];
	}
}
