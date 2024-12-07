using System.Reflection;

namespace pathmage.KnightmareEngine;

partial class Extensions
{
	public static Set<string> GetAllSceneFiles(this string file_path) =>
		file_path.GetAllFilesThat(file => file.TrimSuffix(".remap").GetExtension() == "tscn");

	public static Set<string> GetAllFilesThat(this string file_path, Func<string, bool> condition)
	{
		var result = Set<string>.With(100);

		search(file_path);

		void search(string curr_path)
		{
			var curr_dir = DirAccess.Open(curr_path);

			foreach (var file in curr_dir.GetFiles())
				if (condition(file))
					result.Append(curr_path.PathJoin(file));

			foreach (var child_dir in curr_dir.GetDirectories())
			{
				var child_dir_path = curr_path.PathJoin(child_dir);
				var child_dir_access = DirAccess.Open(child_dir_path);

				if (
					child_dir_access.GetDirectories().Length > 0
					|| child_dir_access.GetFiles().Length > 0
				)
					search(child_dir_path);
			}
		}

		return result;
	}
}
