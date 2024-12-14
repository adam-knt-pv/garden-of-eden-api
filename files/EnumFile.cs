#if DEBUG
#define FILE_CHECKS
#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace pathmage.KnightmareEngine;

[AttributeUsage(AttributeTargets.Field)]
public class EnumFileFieldAttribute<T> : Attribute
	where T : IParsable<T>;

[AttributeUsage(AttributeTargets.Field)]
public sealed class EnumFileArrayAttribute<T> : Attribute
	where T : IParsable<T>;

public readonly struct EnumFile<TEnum>(string local_path) : IDisposable
	where TEnum : struct, Enum
{
	readonly string local_path = local_path;
	readonly string[] items = new string[EnumLength];

	public int LineCount => EnumLength;
	public static readonly int EnumLength = Enum.GetNames<TEnum>().Length;

	public string this[TEnum at]
	{
		get => this[Convert.ToInt32(at)];
		set => this[Convert.ToInt32(at)] = value;
	}

	public string this[int at]
	{
		get => items[at];
		set => items[at] = value;
	}

	public static EnumFile<TEnum> CreateOrOpen(string local_path)
	{
		if (!FileAccess.FileExists(local_path))
			Create(local_path);

		return Open(local_path);
	}

	public static void Create(string local_path)
	{
		using var file = FileAccess.Open(local_path, FileAccess.ModeFlags.Write);

		foreach (var _ in EnumLength)
			file.StoreLine("");
	}

	public static EnumFile<TEnum> Open(string local_path)
	{
		var result = new EnumFile<TEnum>(local_path);

		using var file = FileAccess.Open(local_path, FileAccess.ModeFlags.Read);

		foreach (var i in EnumLength)
			result.items[i] = file.GetLine();

		return result;
	}

	public void Dispose()
	{
		using var file = FileAccess.Open(local_path, FileAccess.ModeFlags.Write);

		foreach (var i in EnumLength)
			file.StoreLine(items[i]);
	}

	public T Field<T>(TEnum at)
		where T : IParsable<T>
	{
#if FILE_CHECKS
		CheckFieldAttribute(at, typeof(EnumFileFieldAttribute<>).Name);
		CheckFieldType<T>(at);
#endif
		return T.Parse(this[at], null);
	}

	public void Field<T>(TEnum at, T value)
		where T : IParsable<T>
	{
#if FILE_CHECKS
		CheckFieldAttribute(at, typeof(EnumFileFieldAttribute<>).Name);
		CheckFieldType<T>(at);
#endif
		this[at] = value.ToString() ?? "";
	}

	public T[] Array<T>(TEnum at)
		where T : IParsable<T>
	{
#if FILE_CHECKS
		CheckFieldAttribute(at, typeof(EnumFileArrayAttribute<>).Name);
		CheckFieldType<T>(at);
#endif
		var split_items = this[at].Split(Constants.File.ItemSeparator);

		if (split_items is [""])
			return [];

		var result = new T[split_items.Length];

		foreach (var i in result.Length)
			result[i] = T.Parse(split_items[i], null);

		return result;
	}

	public void Array<T>(TEnum at, params T[] values)
		where T : IParsable<T>
	{
#if FILE_CHECKS
		CheckFieldAttribute(at, typeof(EnumFileArrayAttribute<>).Name);
		CheckFieldType<T>(at);
#endif
		var result = new string[values.Length];

		foreach (var i in result.Length)
			result[i] = values[i].ToString() ?? "";

		this[at] = string.Join(Constants.File.ItemSeparator, result);
	}

	public IEnumerator<string> GetEnumerator()
	{
		foreach (var i in EnumLength)
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
