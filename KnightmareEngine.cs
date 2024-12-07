global using System;
global using Godot;
global using pathmage.ToolKit;
global using pathmage.ToolKit.Collections;
global using static pathmage.ToolKit.Debug.ILogger;
using System.IO;
using System.Reflection;
using pathmage.ToolKit.Debug;

namespace pathmage.KnightmareEngine;

partial class Y : Node, Game.ISceneLoadHelper<Y>;

partial class X : Y, Game.ISceneLoadHelper<X>;

public sealed partial class KnightmareEngine : Node, Plugin<KnightmareEngine>
{
	static KnightmareEngine()
	{
		Logger.Singleton = new LoggerWrapper(GD.Print);

		foreach (var file in "res://".PickSceneFiles())
		{
			print(GD.Load<PackedScene>(file).Instantiate());
		}

		// Plugin.findSceneFiles(DirAccess.Open("res://"));
		// foreach (var type in Plugin.FindSceneTypes(typeof(KnightmareEngine).Assembly))
		// 	print(type.ToText());

		// print("OS PATH", OS.GetExecutablePath().GetBaseDir());
		// print("Project settings path", ProjectSettings.GlobalizePath("res://"));
		//
		// var path = OS.GetExecutablePath();
		//
		// if (OS.HasFeature("editor"))
		// 	path = ProjectSettings.GlobalizePath("res://");
		//
		// print(path);
		// foreach (var file in DirAccess.GetFilesAt(path))
		// 	print(file);
		// Load(typeof(KnightmareEngine).Assembly, ProjectSettings.GlobalizePath("res://"));
	}

	// public override void _Ready()
	// {
	// 	var assembly = Assembly.LoadFile($"{Directory.GetCurrentDirectory()}/pathmage.Knighturn.dll");
	// 	print(assembly);
	// 	ProjectSettings.LoadResourcePack("res://Knighturn.pck");
	// 	AddChild(GD.Load<PackedScene>("res://ui/user-interface.tscn").Instantiate());
	// }
	//
	// public static void Load(Assembly assembly, string file_path)
	// {
	// 	print(file_path);
	// 	print(ProjectSettings.GlobalizePath(file_path));
	// }
}

public static partial class Extensions;
