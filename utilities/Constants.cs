namespace pathmage.KnightmareEngine;

public interface Constants : ToolKit.Constants
{
	new interface File : ToolKit.Constants.File;

	new interface Text : ToolKit.Constants.Text;

	new interface Reflection : ToolKit.Constants.Reflection;

	interface Node
	{
		const string RootPath = "/root";
	}

	interface Multiplayer
	{
		const char ServerAddressSeparator = '#';

		const int MinPort = 1024;
		const int DefaultPort = 5121;
		const int MaxPort = 65535;

		const int MaxClients = 4095;
	}
}
