namespace pathmage.KnightmareEngine;

public sealed class Knighturn : Plugin<Knighturn>;

public interface Plugin<TSelf>
	where TSelf : Plugin<TSelf> { }
