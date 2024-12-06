global using System;
global using Godot;
global using pathmage.ToolKit;
global using pathmage.ToolKit.Collections;
global using static pathmage.ToolKit.Debug.ILogger;
using pathmage.ToolKit.Debug;

namespace pathmage.KnightmareEngine;

public sealed partial class KnightmareEngine : Node, Game.Scene<KnightmareEngine>
{
	static KnightmareEngine()
	{
		Logger.Singleton = new LoggerWrapper(GD.Print);
	}
}
