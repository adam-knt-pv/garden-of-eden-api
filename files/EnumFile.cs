using System.Collections;
using System.Collections.Generic;

namespace pathmage.KnightmareEngine;

public class EnumFileHintAttribute;

[AttributeUsage(AttributeTargets.Field)]
public class EnumFileStringAttribute : Attribute;

public class EnumFileFieldAttribute<T> : EnumFileStringAttribute
	where T : IParsable<T>;

public sealed class EnumFileArrayAttribute<T> : EnumFileFieldAttribute<T>
	where T : IParsable<T>;

public readonly struct EnumFile<TEnum> : IDisposable
	where TEnum : struct, Enum
{
	readonly string local_path;

	public string this[TEnum at]
	{
		get => items[Convert.ToInt32(at)];
		set => items[Convert.ToInt32(at)] = value;
	}
	readonly string[] items;

	static readonly int enum_length = Enum.GetNames<TEnum>().Length;

	public static EnumFile<TEnum> Open(string local_path) => new(local_path);

	EnumFile(string local_path)
	{
		this.local_path = local_path;

		if (FileAccess.FileExists(local_path))
		{
			using var file = FileAccess.Open(local_path, FileAccess.ModeFlags.Read);

			items = new string[enum_length];

			foreach (var i in enum_length)
				items[i] = file.GetLine();

			return;
		}

		using var new_file = FileAccess.Open(local_path, FileAccess.ModeFlags.Write);

		items = new string[enum_length];

		foreach (var i in enum_length)
			new_file.StoreLine("");
	}

	public void Dispose() => Save();

	public void Save()
	{
		using var file = FileAccess.Open(local_path, FileAccess.ModeFlags.Write);

		foreach (var i in enum_length)
			file.StoreLine(items[i]);
	}

	public T Get<T>(TEnum at)
		where T : IParsable<T> => T.Parse(this[at], null);

	public void Set<T>(TEnum at, T value) { }

	public IEnumerator<string> GetEnumerator()
	{
		foreach (var i in enum_length)
			yield return items[i];
	}
}
