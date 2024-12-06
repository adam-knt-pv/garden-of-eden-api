namespace pathmage.KnightmareEngine;

public readonly struct PluginVersion
{
	public int Prefix { get; }
	public int Major { get; }
	public int Minor { get; }
	public int Suffix { get; } = -1;

	public static implicit operator PluginVersion(string item) => new(item);

	public PluginVersion(string version)
	{
		var split_version = version.Split('.', '_');
		Prefix = int.Parse(split_version[0]);
		Major = int.Parse(split_version[1]);
		Minor = int.Parse(split_version[2]);

		if (split_version.Length > 3)
			Suffix = int.Parse(split_version[3]);
	}

	public static implicit operator string(PluginVersion item) => item.ToString();

	public override string ToString() =>
		$"v{Prefix}.{Major}.{Minor}{(Suffix == -1 ? "" : $"_{(Suffix > 9 ? Suffix : $"0{Suffix}")}")}";
}
