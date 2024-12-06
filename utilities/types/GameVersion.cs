namespace pathmage.KnightmareEngine;

public readonly struct GameVersion
{
	public DevelopmentPhases Phase { get; }
	public PluginVersion Version { get; }

	public static implicit operator GameVersion((DevelopmentPhases Phase, string Version) item) =>
		new(item.Phase, item.Version);

	public GameVersion(DevelopmentPhases phase, string version) =>
		(Phase, Version) = (phase, version);

	public static implicit operator GameVersion(string item) => new(item);

	public GameVersion(string version)
	{
		var split_version = version.Split(' ');
		Phase = Enum.Parse<DevelopmentPhases>(split_version[0]);
		Version = split_version[1];
	}

	public static implicit operator string(GameVersion item) => item.ToString();

	public override string ToString() => $"{Enum.GetName(Phase)} {Version}";

	public enum DevelopmentPhases
	{
		Indev,
		Alpha,
		Beta,
		Release,
	}
}
