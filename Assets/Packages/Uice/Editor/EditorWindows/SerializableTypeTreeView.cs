using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor.IMGUI.Controls;

namespace Uice.Editor
{
    public class SerializableTypeTreeView : TreeView
    {
        private readonly IReadOnlyDictionary<Type, string> values;

        public SerializableTypeTreeView(TreeViewState state, IReadOnlyDictionary<Type, string> values) : base(state)
        {
            this.values = values.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var items = new List<TreeViewItem>();
            StringBuilder leafPath = new StringBuilder();

            if (values != null)
            {
                foreach (var pair in values)
                {
                    var pathAttribute = (PathAttribute)Attribute.GetCustomAttribute(pair.Key, typeof(PathAttribute));
                    var fullPath = pathAttribute == null ? pair.Value : pathAttribute.Path + '/' + pair.Value;

                    var splitPath = fullPath.Split('/');
                    int depth = -1;
                    leafPath.Clear();
                    foreach (string pathTreeLeaf in splitPath)
                    {
                        leafPath.Append(pathTreeLeaf);
                        
                        depth++;
                        int hash = leafPath.ToString().GetHashCode();
                        if (items.Any(item => item.id == hash))
                        {
                            continue;
                        }
                        items.Add(new TreeViewItem(id: hash, depth: depth, displayName: pathTreeLeaf));
                    }
                }
            }

            SetupParentsAndChildrenFromDepths(root, items);
            return root;
        }

        protected override bool CanMultiSelect(TreeViewItem item)
        {
            return false;
        }

        protected override void SelectionChanged(IList<int> selectedIds)
        {
            base.SelectionChanged(selectedIds);
            int selectedId = selectedIds[0];
            SetExpanded(selectedId, !IsExpanded(selectedId));
        }

        protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
        {
            return base.DoesItemMatchSearch(item, search) && IsSelectableValue(item.displayName);
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
