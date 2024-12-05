global using System;
global using Godot;
global using pathmage.ToolKit;
global using static pathmage.ToolKit.Debug.ILogger;
global using pathmage.ToolKit.Collections;
using pathmage.ToolKit.Debug;

namespace pathmage.KnightmareEngine;

public sealed partial class KnightmareEngine : Node
{
	static KnightmareEngine()
	{
		Logger.Singleton = new LoggerWrapper(GD.Print);
	}
}
