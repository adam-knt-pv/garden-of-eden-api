global using System;
global using Godot;
global using pathmage.KnightmareEngine.Extensions;
global using pathmage.KnightmareEngine.Globals;
global using pathmage.ToolKit;
global using pathmage.ToolKit.Collections;
global using static pathmage.ToolKit.Debug.ILogger;
global using pathmage.ToolKit.Extensions;
global using pathmage.ToolKit.Utilities;
using System.IO;
using System.Reflection;
using pathmage.ToolKit.Debug;
using pathmage.ToolKit.Globals;

namespace pathmage.KnightmareEngine
{
	public sealed partial class KnightmareEngine : Node
	{
		KnightmareEngine()
		{
			Plugin.Logger = new LoggerWrapper(GD.Print);
		}

		public override void _Ready() { }
	}

	namespace Extensions
	{
		public static partial class Extensions;
	}
}
