using System.Diagnostics.CodeAnalysis;

namespace AdamKnight.KnightmareEngine.Extensions;

partial class Extensions
{
	const int pick_child_nodes_assumed_length = 16;
	const int find_child_nodes_assumed_length = pick_child_nodes_assumed_length * 2;

	public static void QueueFreeIfValid(this Node node)
	{
		if (!GodotObject.IsInstanceValid(node))
			return;

		node.QueueFree();
	}

	public static bool TryGetNode(
		this Node parent,
		NodePath path,
		[NotNullWhen(true)] out Node? node
	)
	{
		node = parent.GetNodeOrNull(path);
		return node != null;
	}

	public static bool TryGetNode<TNode>(
		this Node parent,
		NodePath path,
		[NotNullWhen(true)] out TNode? node
	)
		where TNode : Node
	{
		node = parent.GetNodeOrNull<TNode>(path);
		return node != null;
	}

	public static bool TryGetChild(this Node parent, int at, [NotNullWhen(true)] out Node? node)
	{
		node = parent.GetChildOrNull<Node>(at);
		return node != null;
	}

	public static bool TryGetChild<TNode>(
		this Node parent,
		int at,
		[NotNullWhen(true)] out TNode? node
	)
		where TNode : Node
	{
		node = parent.GetChildOrNull<TNode>(at);
		return node != null;
	}

	public static bool TryPickChild<TNode>(this Node parent, [NotNullWhen(true)] out TNode? node)
		where TNode : Node
	{
		foreach (var child in parent.GetChildren())
		{
			if (child is TNode t_child)
			{
				node = t_child;
				return true;
			}
		}

		node = null;
		return false;
	}

	public static bool TryPickChildThat(
		this Node parent,
		Func<Node, bool> filter,
		[NotNullWhen(true)] out Node? node
	)
	{
		foreach (var child in parent.GetChildren())
		{
			if (filter(child))
			{
				node = child;
				return true;
			}
		}

		node = null;
		return false;
	}

	public static bool TryPickChildThat<TNode>(
		this Node parent,
		Func<TNode, bool> filter,
		[NotNullWhen(true)] out TNode? node
	)
		where TNode : Node
	{
		foreach (var child in parent.GetChildren())
		{
			if (child is TNode t_child && filter(t_child))
			{
				node = t_child;
				return true;
			}
		}

		node = null;
		return false;
	}

	public static bool TryFindChild<TNode>(this Node parent, [NotNullWhen(true)] out TNode? node)
		where TNode : Node
	{
		return searchChildren(parent, out node);

		static bool searchChildren(Node parent, out TNode? node)
		{
			foreach (var child in parent.GetChildren())
			{
				if (child is TNode t_child)
				{
					node = t_child;
					return true;
				}

				if (child.GetChildCount() > 0 && searchChildren(child, out node))
					return true;
			}

			node = null;
			return false;
		}
	}

	public static bool TryFindChildThat(
		this Node parent,
		Func<Node, bool> filter,
		[NotNullWhen(true)] out Node? node
	)
	{
		return searchChildren(parent, out node, filter);

		static bool searchChildren(Node parent, out Node? node, Func<Node, bool> filter)
		{
			foreach (var child in parent.GetChildren())
			{
				if (filter(child))
				{
					node = child;
					return true;
				}

				if (child.GetChildCount() > 0 && searchChildren(child, out node, filter))
					return true;
			}

			node = null;
			return false;
		}
	}

	public static bool TryFindChildThat<TNode>(
		this Node parent,
		Func<TNode, bool> filter,
		[NotNullWhen(true)] out TNode? node
	)
		where TNode : Node
	{
		return searchChildren(parent, out node, filter);

		static bool searchChildren(Node parent, out TNode? node, Func<TNode, bool> filter)
		{
			foreach (var child in parent.GetChildren())
			{
				if (child is TNode t_child && filter(t_child))
				{
					node = t_child;
					return true;
				}

				if (child.GetChildCount() > 0 && searchChildren(child, out node, filter))
					return true;
			}

			node = null;
			return false;
		}
	}

	public static SetArray<TNode> PickChildren<TNode>(this Node parent)
		where TNode : Node
	{
		var output = SetArray<TNode>.New(pick_child_nodes_assumed_length);

		foreach (var child in parent.GetChildren())
		{
			if (child is TNode t_child)
				output.Append(t_child);
		}

		return output;
	}

	public static SetArray<TNode> FindChildren<TNode>(this Node parent)
		where TNode : Node
	{
		var output = SetArray<TNode>.New(find_child_nodes_assumed_length);

		searchChildren(parent);

		void searchChildren(Node node)
		{
			foreach (var child in node.GetChildren())
			{
				if (child is TNode t_child)
					output.Append(t_child);

				if (child.GetChildCount() > 0)
					searchChildren(child);
			}
		}

		return output;
	}

	public static SetArray<Node> PickChildrenThat(this Node parent, Func<Node, bool> filter)
	{
		var output = SetArray<Node>.New(pick_child_nodes_assumed_length);

		foreach (var child in parent.GetChildren())
		{
			if (filter(child))
				output.Append(child);
		}

		return output;
	}

	public static SetArray<TNode> PickChildrenThat<TNode>(this Node parent, Func<TNode, bool> filter)
		where TNode : Node
	{
		var output = SetArray<TNode>.New(pick_child_nodes_assumed_length);

		foreach (var child in parent.GetChildren())
		{
			if (child is TNode t_child && filter(t_child))
				output.Append(t_child);
		}

		return output;
	}

	public static SetArray<Node> FindChildrenThat(this Node parent, Func<Node, bool> filter)
	{
		var output = SetArray<Node>.New(find_child_nodes_assumed_length);

		searchChildren(parent);

		void searchChildren(Node node)
		{
			foreach (var child in node.GetChildren())
			{
				if (filter(child))
					output.Append(child);

				if (child.GetChildCount() > 0)
					searchChildren(child);
			}
		}

		return output;
	}

	public static SetArray<TNode> FindChildrenThat<TNode>(this Node parent, Func<TNode, bool> filter)
		where TNode : Node
	{
		var output = SetArray<TNode>.New(pick_child_nodes_assumed_length);

		searchChildren(parent);

		void searchChildren(Node node)
		{
			foreach (var child in node.GetChildren())
			{
				if (child is TNode t_child && filter(t_child))
					output.Append(t_child);

				if (child.GetChildCount() > 0)
					searchChildren(child);
			}
		}

		return output;
	}
}
