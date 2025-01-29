namespace pathmage.KnightmareEngine;

public interface IDataFile<TSelf> : IDataType<TSelf>
	where TSelf : IDataFile<TSelf>, new()
{
	static abstract TSelf LoadFile(string file_path);

	protected static TSelf defaultLoadFile(string file_path)
	{
		if (!FileAccess.FileExists(file_path))
			TSelf.SaveFile(file_path, new TSelf());

		return TSelf.FromString(FileAccess.GetFileAsString(file_path));
	}

	static abstract void SaveFile(string file_path, in TSelf value);

	protected static void defaultSaveFile(string file_path, in TSelf value)
	{
		using var file = FileAccess.Open(file_path, FileAccess.ModeFlags.Write);

		file.StoreString(value.ToString());
	}
}
