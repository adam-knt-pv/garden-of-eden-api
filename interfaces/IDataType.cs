namespace pathmage.KnightmareEngine;

public interface IDataType<out TSelf>
	where TSelf : IDataType<TSelf>
{
	string ToString();

	static abstract TSelf FromString(string str);
}
