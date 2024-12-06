using System.Collections.Frozen;

namespace pathmage.KnightmareEngine;

public interface Plugin<TSelf>
	where TSelf : Plugin<TSelf>
{
	public interface Scene { }

	public interface Scene<TScene> : Scene
		where TScene : Node, Scene<TScene> { }
}
