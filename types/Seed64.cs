namespace pathmage.KnightmareEngine;

/// <summary>
/// 64-bit seed whose values are never 0.
/// </summary>
public struct Seed64
{
	public static Seed64 Null { get; } =
		new()
		{
			From = "",
			Left = 0,
			Right = 0,
		};

	public string From { get; set; }

	public int Left { get; set; }
	public int Right { get; set; }

	public static implicit operator Seed64(string text) => new(text);

	public Seed64()
		: this("") { }

	public Seed64(string text)
	{
		From = text;

		var full = text.ToInt64();

		if (long.TryParse(text, out var number))
		{
			From = number.ToString();

			full = number;
		}

		if (full == 0 || text == "")
		{
			full = 0;

			while (full == 0)
				full = Random.Shared.NextInt64(long.MinValue, long.MaxValue);

			From = full.ToString();
		}

		(Left, Right) = full.ToInt32();

		var r = new Random(Right);

		switch (Left)
		{
			case -1:
				Left = 0;

				while (Left == 0)
					Left = r.Next(1, int.MaxValue);

				Left = -Left;
				break;

			case 0:
				while (Left == 0)
					Left = r.Next(1, int.MaxValue);

				break;
		}
	}

	public static implicit operator string(Seed64 realm_seed) => realm_seed.ToString();

	public override string ToString() => From;
}
