using System.IO;
using System.Linq;
using System.Reflection;

namespace pathmage.KnightmareEngine;

partial class Extensions
{
	public static Set<string> PickSceneFiles(this string local_path)
	{
		var result = local_path.PickFilesThat(file =>
			file.TrimSuffix(".remap").GetExtension() == "tscn"
		);

		foreach (var i in result.Count)
			result[i] = result[i].TrimSuffix(".remap");

		return result;
	}

	public static Set<string> PickFilesThat(this string local_path, Func<string, bool> filter)
	{
		var files = DirAccess.Open(local_path).GetFiles();
		var result = Set<string>.With(files.Length);

		foreach (var file in files)
			if (filter(file))
				result.Append(local_path.PathJoin(file));

		return result;
	}

	public static Set<string> FindSceneFiles(this string local_path, params string[] exclude_dirs)
	{
		var result = local_path.FindFilesThat(
			file => file.TrimSuffix(".remap").GetExtension() == "tscn",
			exclude_dirs
		);

		foreach (var i in result.Count)
			result[i] = result[i].TrimSuffix(".remap");

		return result;
	}

	public static Set<string> FindFilesThat(
		this string local_path,
		Func<string, bool> filter,
		params string[] exclude_dirs
	)
	{
		var result = Set<string>.With(100);

		searchDir(local_path);

		void searchDir(string at_path)
		{
			var curr_dir = DirAccess.Open(at_path);

			foreach (var file in curr_dir.GetFiles())
				if (filter(file))
					result.Append(at_path.PathJoin(file));

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

		return result;
	}
}
