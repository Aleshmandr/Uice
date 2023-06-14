using System.Collections.Generic;
using UnityEngine;

namespace Uice
{
	public class ContextComponentTreeNode
	{
		public Transform Transform { get; }
		public Transform ParentObject { get; set; }
		public ContextComponentTreeNode ParentNode { get; private set; }
		public Dictionary<string, ContextComponentInfo> Components { get; }
		public LinkedList<ContextComponentTreeNode> Children { get; }

		public ContextComponentTreeNode(Transform transform)
		{
			Transform = transform;
			Components = new Dictionary<string, ContextComponentInfo>();
			Children = new LinkedList<ContextComponentTreeNode>();
		}

		public void SetParentNode(ContextComponentTreeNode parent)
		{
			ParentNode = parent;
			ParentNode?.Children.AddLast(this);
		}

		public void AddComponent(ContextComponent component)
		{
			Components[component.Id] = new ContextComponentInfo(component);
		}
	}
}
