global using System;
global using AdamKnight.KnightmareEngine.Extensions;
global using AdamKnight.KnightmareEngine.Globals;
global using AdamKnight.ToolKit;
global using AdamKnight.ToolKit.Collections;
global using static AdamKnight.ToolKit.Debug.ILogger;
global using AdamKnight.ToolKit.Extensions;
global using AdamKnight.ToolKit.Utilities;
global using Godot;
using System.IO;
using System.Reflection;
using AdamKnight.ToolKit.Debug;
using AdamKnight.ToolKit.Globals;

namespace AdamKnight.KnightmareEngine
{
	public sealed partial class KnightmareEngine : Node
	{
		static KnightmareEngine()
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
