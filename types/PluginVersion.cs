using System.Diagnostics.CodeAnalysis;

namespace AdamKnight.KnightmareEngine;

public struct PluginVersion
{
	public static PluginVersion Null { get; } = "Indev v-1.-1.-1";

	public DevelopmentPhase Phase { get; set; }

	public enum DevelopmentPhase
	{
		Indev,
		Alpha,
		Beta,
		Release,
	}

	public int Prefix { get; set; }
	public int Major { get; set; }
	public int Minor { get; set; }
	public int Suffix { get; set; } = -1;

	public static implicit operator PluginVersion(
		(DevelopmentPhase Phase, string Version) version
	) => new(version.Phase, version.Version);

	public PluginVersion(DevelopmentPhase phase, string version)
		: this($"{phase} {version}") { }

	public static implicit operator PluginVersion(string version) => new(version);

	public PluginVersion(string version)
	{
		var phase_version = version.Split(' ');
		Phase = Enum.Parse<DevelopmentPhase>(phase_version[0]);

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

	public string ToFilename() => $".{ToString().ToLower().Replace(' ', '-')}";

	public static PluginVersion FromFilename(string filename)
	{
		var i_dash = filename.IndexOf('-');

		var phase_version = new[] { filename[1..i_dash], filename[(i_dash + 1)..] };

		return new PluginVersion(
			$"{phase_version[0][0].ToUpper()}{phase_version[0][1..]} {phase_version[1]}"
		);
	}
}
