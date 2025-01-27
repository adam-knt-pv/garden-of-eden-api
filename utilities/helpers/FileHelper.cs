using System.IO;
using System.Linq;
using System.Reflection;

namespace pathmage.KnightmareEngine.Helpers;

public interface FileHelper
{
	public static SetArray<string> PickSceneFiles(string local_path)
	{
		var output = PickFilesThat(
			local_path,
			file => file.TrimSuffix(".remap").GetExtension() == "tscn"
		);

		foreach (var i in output.Count)
			output[i] = output[i].TrimSuffix(".remap");

		return output;
	}

	public static SetArray<string> PickFilesThat(string local_path, Func<string, bool> filter)
	{
		var files = DirAccess.Open(local_path).GetFiles();
		var output = SetArray<string>.New(files.Length);

		foreach (var file in files)
			if (filter(file))
				output.Append(local_path.PathJoin(file));

		return output;
	}

	public static SetArray<string> FindSceneFiles(string local_path, params string[] exclude_dirs)
	{
		var output = FindFilesThat(
			local_path,
			file => file.TrimSuffix(".remap").GetExtension() == "tscn",
			exclude_dirs
		);

		foreach (var i in output.Count)
			output[i] = output[i].TrimSuffix(".remap");

		return output;
	}

	public static SetArray<string> FindFilesThat(
		string local_path,
		Func<string, bool> filter,
		params string[] exclude_dirs
	)
	{
		var output = SetArray<string>.New(Constants.KnightmareEngine.FindFilesInitLength);

		searchDir(local_path);

		void searchDir(string at_path)
		{
			var curr_dir = DirAccess.Open(at_path);

			foreach (var file in curr_dir.GetFiles())
				if (filter(file))
					output.Append(at_path.PathJoin(file));

			foreach (var child_dir in curr_dir.GetDirectories())
			{
				if (exclude_dirs.Contains(child_dir))
					continue;

				var child_dir_path = at_path.PathJoin(child_dir);
				var child_dir_access = DirAccess.Open(child_dir_path);

				if (
					child_dir_access.GetDirectories().Length > 0
					|| child_dir_access.GetFiles().Length > 0
				)
					searchDir(child_dir_path);
			}
		}

		return output;
	}
}
