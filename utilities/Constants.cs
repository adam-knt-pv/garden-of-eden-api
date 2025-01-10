namespace pathmage.KnightmareEngine.Globals;

public interface Constants : ToolKit.Globals.Constants
{
	new interface File : ToolKit.Globals.Constants.File;

	new interface Text : ToolKit.Globals.Constants.Text;

	new interface Reflection : ToolKit.Globals.Constants.Reflection;

	interface Multiplayer
	{
		const char ServerAddressSeparator = '#';

		const int MinPort = 1024;
		const int DefaultPort = 5252;
		const int MaxPort = 65535;

		const int MinPlayers = 2;
		const int MaxPlayers = 4096;
	}

	interface InputMap;
}
