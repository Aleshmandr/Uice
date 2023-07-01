using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Uice.Editor
{
    public class SerializableTypeTreeView : TreeView
    {
        private readonly IReadOnlyDictionary<Type, string> values;

        public SerializableTypeTreeView(TreeViewState state, IReadOnlyDictionary<Type, string> values) : base(state)
        {
            this.values = values;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var items = new List<TreeViewItem>();

            if (values != null)
            {
                int optionId = 1; // 0 is Root
                foreach (var pair in values)
                {
                    var pathAttribute = (PathAttribute)Attribute.GetCustomAttribute(pair.Key, typeof(PathAttribute));
                    var fullPath = pathAttribute == null ? pair.Value : pathAttribute.Path + '/' + pair.Value;

                    var splitPath = fullPath.Split('/');
                    int depth = -1;
                    foreach (string pathTreeLeaf in splitPath)
                    {
                        depth++;
                        int hash = fullPath.GetHashCode();
                        if (items.Any(item => item.id == hash))
                        {
                            continue;
                        }
                        items.Add(new TreeViewItem(id: optionId, depth: depth, displayName: pathTreeLeaf));
                        optionId = hash;
                    }
                }
            }

            SetupParentsAndChildrenFromDepths(root, items);
            return root;
        }

        public string GetSelectedItem()
        {
            var selectionItems = GetSelection();
            return selectionItems.Count > 0 ? FindItem(selectionItems[0], rootItem).displayName : string.Empty;
        }

        public bool IsSelectableValue(string selection)
        {
            return values.Values.Any(s => s == selection);
        }
    }
}
