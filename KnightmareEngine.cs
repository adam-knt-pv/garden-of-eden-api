global using System;
global using Godot;
global using pathmage.ToolKit;
global using pathmage.ToolKit.Collections;
global using static pathmage.ToolKit.Debug.ILogger;
using System.Reflection;
using pathmage.ToolKit.Debug;

namespace pathmage.KnightmareEngine;

public sealed partial class KnightmareEngine : Node, Plugin<KnightmareEngine>
{
	static KnightmareEngine()
	{
		Logger.Singleton = new LoggerWrapper(GD.Print);

		Load(typeof(KnightmareEngine).Assembly, ProjectSettings.GlobalizePath("res://"));
	}

	public static void Load(Assembly assembly, string file_path)
	{
		print(file_path);
	}
}
