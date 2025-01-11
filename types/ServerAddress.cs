using System.Diagnostics.CodeAnalysis;

namespace pathmage.KnightmareEngine;

public struct ServerAddress
{
	public static ServerAddress Null { get; } = "";

	/// <summary>
	/// Either a fully qualified domain name or an IP address in IPv4 or IPv6 format.
	/// </summary>
	public string Address { get; set; }
	public int Port { get; set; }

	public static implicit operator ServerAddress(string from) => new(from);

	public ServerAddress(string from)
	{
		var address = from.Split(Constants.Multiplayer.ServerAddressSeparator);
		Address = address[0];

		if (address.Length > 1 && int.TryParse(address[1], out var port))
			Port = port;

		if (Port is < Constants.Multiplayer.MinPort or > Constants.Multiplayer.MaxPort)
			Port = Constants.Multiplayer.DefaultPort;
	}

	public static implicit operator string(ServerAddress address) => address.ToString();

	public override string ToString() =>
		$"{Address}{Constants.Multiplayer.ServerAddressSeparator}{Port}";
}
