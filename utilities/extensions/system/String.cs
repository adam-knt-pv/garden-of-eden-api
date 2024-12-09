namespace pathmage.KnightmareEngine;

partial class Extensions
{
	public static string ToDashCase(this string item) => item.ToSnakeCase().Replace('_', '-');
}
