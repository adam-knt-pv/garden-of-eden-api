namespace pathmage.KnightmareEngine;

public readonly struct PluginVersion
{
	public DevelopmentPhases Phase { get; }
	public int Prefix { get; }
	public int Major { get; }
	public int Minor { get; }
	public int Suffix { get; } = -1;

	public static implicit operator PluginVersion(
		(DevelopmentPhases Phase, string Version) version
	) => new(version.Phase, version.Version);

	public PluginVersion(DevelopmentPhases phase, string version)
		: this($"{phase} {version}") { }

	public static implicit operator PluginVersion(string version) => new(version);

	public PluginVersion(string version)
	{
		var phase_version = version.Split(' ');
		Phase = Enum.Parse<DevelopmentPhases>(phase_version[0]);

		var split_version = phase_version[1].Split('.', '_');
		Prefix = int.Parse(split_version[0][0] == 'v' ? split_version[0][1..] : split_version[0]);
		Major = int.Parse(split_version[1]);
		Minor = int.Parse(split_version[2]);

		if (split_version.Length > 3)
			Suffix = int.Parse(split_version[3]);
	}

	public static implicit operator string(PluginVersion version) => version.ToString();

	public override string ToString() =>
		$"{Enum.GetName(Phase)} v{Prefix}.{Major}.{Minor}{(Suffix == -1 ? "" : $"_{(Suffix > 9 ? Suffix : $"0{Suffix}")}")}";

	public string ToFilename() => ToString().ToLower().Replace(' ', '-');

	public static PluginVersion FromFilename(string filename)
	{
		var phase_version = filename.Split('-');
		return new PluginVersion(
			$"{phase_version[0][0].ToUpper()}{phase_version[0][1..]} {phase_version[1]}"
		);
	}

	public enum DevelopmentPhases
	{
		Indev,
		Alpha,
		Beta,
		Release,
	}
}
