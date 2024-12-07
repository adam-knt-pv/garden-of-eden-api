global using System;
global using Godot;
global using pathmage.ToolKit;
global using pathmage.ToolKit.Collections;
global using static pathmage.ToolKit.Debug.ILogger;
using System.IO;
using System.Reflection;
using pathmage.ToolKit.Debug;

namespace pathmage.KnightmareEngine;

public sealed partial class KnightmareEngine : Node, Plugin<KnightmareEngine>
{
	static KnightmareEngine()
	{
		Logger.Singleton = new LoggerWrapper(GD.Print);

		var version = new PluginVersion(PluginVersion.DevelopmentPhases.Alpha, "1.0.0_01");

		print(version, version.ToFilename());
		print(PluginVersion.FromFilename("indev-v1.0.0_01"));
	}
}

public static partial class Extensions;
