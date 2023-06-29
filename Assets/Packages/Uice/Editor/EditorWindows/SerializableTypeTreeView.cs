using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace Uice.Editor
{
    public class SerializableTypeTreeView : TreeView
    {
        private readonly IEnumerable<string> options;

        public SerializableTypeTreeView(TreeViewState state, IEnumerable<string> options) : base(state)
        {
            this.options = options;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            var items = new List<TreeViewItem>();

            if (options != null)
            {
                int optionId = 1; // 0 is Root
                foreach (string option in options)
                {
                    items.Add(new TreeViewItem(id: optionId, depth: 0, displayName: option));
                    optionId++;
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
    }
}
