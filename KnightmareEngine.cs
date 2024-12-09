global using System;
global using Godot;
global using pathmage.ToolKit;
global using pathmage.ToolKit.Collections;
global using static pathmage.ToolKit.Debug.ILogger;
using System.IO;
using System.Reflection;
using pathmage.ToolKit.Debug;

namespace pathmage.KnightmareEngine;

public sealed partial class KnightmareEngine : Node
{
	static KnightmareEngine()
	{
		Logger.Singleton = new LoggerWrapper(GD.Print);

		// using var file = EnumFile<Test>.Open("user://test.txt");

		// file[Test.a] = "abc";
		// file[Test.b] = "def";

		// foreach (var item in file)
		// {
		// 	print(item);
		// }
	}

	public static void TestDebug()
	{
#if DEBUG
		print("DEBUG KNIGHTMARE ENGINE");
#else
		print("RELEASE KNIGHTMARE ENGINE");
#endif
	}

	enum Test
	{
		[EnumFileString]
		SomeName,

		[EnumFileField<int>]
		SomeOtherName,

		[EnumFileArray<int>]
		Values,

		[EnumFileArray<int>]
		Test,
	}
}

public static partial class Extensions;
