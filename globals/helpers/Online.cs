namespace pathmage.KnightmareEngine.Globals;

public interface Online : ToolKit.Globals.Online
{
	const char ServerAddressSeparator = '#';

	const int DefaultPort = 5252;

	const int MinPlayers = 2;
	const int MaxPlayers = 4096;
}
