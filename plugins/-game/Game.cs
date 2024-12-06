namespace pathmage.KnightmareEngine;

public interface Game : Plugin<Game>
{
	static void Load()
	{
		foreach (var dir in DirAccess.GetDirectoriesAt("res://"))
			print(dir);
	}
}
