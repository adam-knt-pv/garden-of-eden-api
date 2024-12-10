#if DEBUG
#define FILE_CHECKS
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pathmage.KnightmareEngine;

public readonly struct ListFile<TEnum>(string local_path, string[] items) : IDisposable
	where TEnum : struct, Enum
{
	readonly string local_path = local_path;

	public string this[TEnum at]
	{
		get => items[Convert.ToInt32(at)];
		set => items[Convert.ToInt32(at)] = value;
	}
	readonly string[] items = items;

	static readonly int enum_length = Enum.GetNames<TEnum>().Length;

	public static ListFile<TEnum> CreateOrOpen(string local_path)
	{
		if (!FileAccess.FileExists(local_path))
		{
			using var file_access = FileAccess.Open(local_path, FileAccess.ModeFlags.Write);

			foreach (var _ in enum_length)
				file_access.StoreLine("");
		}

		return new(local_path);
	}

	public static bool TryOpen(string local_path, out ListFile<TEnum> file)
	{
		if (FileAccess.FileExists(local_path))
		{
			file = new(local_path);
			return true;
		}

		file = default;
		return false;
	}

	public ListFile(string local_path)
		: this(local_path, new string[enum_length])
	{
		this.local_path = local_path;

		using var new_file = FileAccess.Open(local_path, FileAccess.ModeFlags.Read);

		foreach (var i in enum_length)
			items[i] = new_file.GetLine();
	}

	public void Dispose() => Save();

	public void Save()
	{
		using var file = FileAccess.Open(local_path, FileAccess.ModeFlags.Write);

		foreach (var i in enum_length)
			file.StoreLine(items[i]);
	}

	public T Field<T>(TEnum at)
		where T : IParsable<T>
	{
#if FILE_CHECKS
		CheckFieldAttribute(at, typeof(FileFieldAttribute<>).Name);
		CheckFieldType<T>(at);
#endif
		return T.Parse(this[at], null);
	}

	public void Field<T>(TEnum at, T value)
		where T : IParsable<T>
	{
#if FILE_CHECKS
		CheckFieldAttribute(at, typeof(FileFieldAttribute<>).Name);
		CheckFieldType<T>(at);
#endif
		this[at] = value.ToString()!;
	}

	public T[] Array<T>(TEnum at)
		where T : IParsable<T>
	{
#if FILE_CHECKS
		CheckFieldAttribute(at, typeof(FileArrayAttribute<>).Name);
		CheckFieldType<T>(at);
#endif
		var split_items = this[at].Split(Constants.File.ItemSeparator);
		var result = new T[split_items.Length];

		foreach (var i in result.Length)
			result[i] = T.Parse(split_items[i], null);

		return result;
	}

	public void Array<T>(TEnum at, params T[] values)
		where T : IParsable<T>
	{
#if FILE_CHECKS
		CheckFieldAttribute(at, typeof(FileArrayAttribute<>).Name);
		CheckFieldType<T>(at);
#endif
		var result = new string[values.Length];

		foreach (var i in result.Length)
			result[i] = values[i].ToString()!;

		this[at] = string.Join(Constants.File.ItemSeparator, result);
	}

	public IEnumerator<string> GetEnumerator()
	{
		foreach (var i in enum_length)
			yield return items[i];
	}

#if FILE_CHECKS
	static void CheckFieldAttribute(TEnum at, string attribute_name)
	{
		if (
			typeof(TEnum)
				.GetFields()[Convert.ToInt32(at) + 1]
				.CustomAttributes.First()
				.ToString()
				.Find(attribute_name) == -1
		)
		{
			var message =
				$"Trying to use an invalid method for this enum's ({typeof(TEnum).Name}) field ({at})!";
			print($"ENUM FILE CHECK FAILED: {message}");
			throw new Exception(message);
		}
	}

	static void CheckFieldType<T>(TEnum at)
	{
		if (
			typeof(TEnum)
				.GetFields()[Convert.ToInt32(at) + 1]
				.CustomAttributes.First()
				.ToString()
				.Find(typeof(T).FullName!) == -1
		)
		{
			var message =
				$"Trying to use an invalid type ({typeof(T)}) for this enum's ({typeof(TEnum).Name}) field ({at})!";
			print($"ENUM FILE CHECK FAILED: {message}");
			throw new Exception(message);
		}
	}
#endif
}
