using System;
using System.Collections.Generic;
using Uice.Utils;
using UnityEngine;

namespace Uice
{
	public static class ContextComponentTree
	{
		private static readonly Dictionary<Transform, ContextComponentTreeNode> NodesByTransform;
		private static readonly Dictionary<ContextComponent, ContextComponentTreeNode> NodesByComponent;

		static ContextComponentTree()
		{
			NodesByTransform = new Dictionary<Transform, ContextComponentTreeNode>();
			NodesByComponent = new Dictionary<ContextComponent, ContextComponentTreeNode>();
		}

		public static void Register(ContextComponent component)
		{
			if (NodesByComponent.ContainsKey(component) == false)
			{
				AddComponent(component);
			}
		}

		public static void Unregister(ContextComponent component)
		{
			if (NodesByComponent.TryGetValue(component, out ContextComponentTreeNode node))
			{
				node.Components.Remove(component.Id);
				NodesByComponent.Remove(component);

				if (node.Components.Count == 0)
				{
					foreach (ContextComponentTreeNode current in node.Children)
					{
						current.SetParentNode(node.ParentNode);
					}

					NodesByTransform.Remove(component.transform);
				}
			}
		}

		public static void Move(ContextComponent component)
		{
			if (NodesByComponent.TryGetValue(component, out ContextComponentTreeNode node))
			{
				Transform transform = component.transform;

				if (node.ParentObject != transform.parent)
				{
					ReparentNode(transform, node);
				}
			}
			else
			{
				Register(component);
			}
		}

		public static ContextComponent FindBindableComponent(BindingPath path, Type targetType, Transform context)
		{
			ContextComponent result = null;
			Transform currentTransform = context;
			ContextComponentTreeNode currentNode;

			while (result == null && (currentNode = GetNextNodeTowardsRoot(currentTransform)) != null)
			{
				if (string.IsNullOrEmpty(path.ComponentId))
				{
					using (var components = currentNode.Components.Values.GetEnumerator())
					{
						while (result == null && components.MoveNext())
						{
							if (CanBeBound(components.Current, path.PropertyName, targetType))
							{
								result = components.Current?.Component;
							}
						}
					}
				}
				else if (currentNode.Components.TryGetValue(path.ComponentId, out var componentInfo)
					&& CanBeBound(componentInfo, path.PropertyName, targetType))
				{
					result = componentInfo.Component;
				}

				currentTransform = currentNode.Transform.parent;
			}

			return result;
		}

		public static object Bind(BindingPath path, ContextComponent component)
		{
			Register(component);
			ContextComponentTreeNode node = NodesByComponent[component];
			return node.Components[component.Id].GetProperty(path.PropertyName);
		}

		private static void AddComponent(ContextComponent component)
		{
			Transform transform = component.transform;

			if (NodesByTransform.TryGetValue(transform, out var node) == false)
			{
				node = new ContextComponentTreeNode(transform);
				ReparentNode(transform, node);
				NodesByTransform[transform] = node;
			}

			node.AddComponent(component);
			NodesByComponent.Add(component, node);
		}

		private static void ReparentNode(Transform transform, ContextComponentTreeNode node)
		{
			ContextComponentTreeNode parentNode = FindParentNode(transform);
			node.SetParentNode(parentNode);
			node.ParentObject = transform.parent;
		}

		private static ContextComponentTreeNode FindParentNode(Transform transform)
		{
			Transform parentTransform = transform.parent;
			ContextComponentTreeNode parentNode = null;

			while (parentTransform && NodesByTransform.TryGetValue(parentTransform, out parentNode) == false)
			{
				parentTransform = parentTransform.parent;
			}

			return parentNode;
		}

		private static bool CanBeBound(ContextComponentInfo componentInfo, string propertyName, Type targetType)
		{
			object property = componentInfo.GetProperty(propertyName);
			return property != null && BindingUtils.CanBeBound(property.GetType(), targetType);
		}

		private static ContextComponentTreeNode GetNextNodeTowardsRoot(Transform context)
		{
			ContextComponentTreeNode result = null;
			Transform currentTransform = context;

			while (currentTransform != null && NodesByTransform.TryGetValue(currentTransform, out result))
			{
				currentTransform = currentTransform.parent;
			}

			return result;
		}
	}
}
