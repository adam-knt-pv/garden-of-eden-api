namespace pathmage.KnightmareEngine.Globals;

public interface Constants : ToolKit.Globals.Constants
{
	new interface Misc : ToolKit.Globals.Constants.Misc { }

	new interface File : ToolKit.Globals.Constants.File;

	new interface Text : ToolKit.Globals.Constants.Text;

	interface Multiplayer
	{
		const char ServerAddressSeparator = '#';

		// const int MinPort = 1024;
		const int MinPort = 0;
		const int DefaultPort = 5252;
		const int MaxPort = 65535;

		const int MinPlayers = 2;
		const int MaxPlayers = 4096;
	}

	interface InputMap;

	internal interface KnightmareEngine
	{
		const int FindFilesInitLength = 16;

		const int PickChildrenInitLength = 16;
		const int FindChildrenExpectedLength = PickChildrenInitLength * 2;
	}
}
