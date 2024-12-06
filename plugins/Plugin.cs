using System.Collections.Frozen;
using System.Reflection;

namespace pathmage.KnightmareEngine;

public interface Plugin<TSelf>
	where TSelf : Plugin<TSelf>
{
	interface Scene { }

	interface Scene<TScene> : Scene
		where TScene : Node, Scene<TScene> { }
}
