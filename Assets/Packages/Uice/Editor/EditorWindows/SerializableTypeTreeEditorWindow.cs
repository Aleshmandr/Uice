using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Uice.Editor
{
    public class SerializableTypeTreeEditorWindow : EditorWindow
    {
        private static readonly GUIStyle WindowStyle = new GUIStyle("window")
        {
            padding = new RectOffset(4, 4, 4, 4)
        };

        private Action<string> selectionCallback;
        private Vector2 scrollPos;
        private SerializableTypeTreeView treeView;

        private string searchTerm;

        public static void Show(Rect buttonRect, IEnumerable<string> options, Action<string> selectionCallback)
        {
            var window = CreateInstance<SerializableTypeTreeEditorWindow>();
            window.InitOptionsTree(options);
            window.selectionCallback = selectionCallback;

            Rect windowRect = buttonRect;
            windowRect.position = GUIUtility.GUIToScreenPoint(buttonRect.position);

            float width = buttonRect.width;
            float height = 300;
            Vector2 windowSize = new Vector2(width, height);

            window.ShowAsDropDown(windowRect, windowSize);
        }

        private void InitOptionsTree(IEnumerable<string> options)
        {
            var treeViewState = new TreeViewState();
            treeView = new SerializableTypeTreeView(treeViewState, options);
        }

        private void Update()
        {
            Repaint();
        }

        public void OnGUI()
        {
            string selection;

            using (var windowRect = new EditorGUILayout.VerticalScope(WindowStyle))
            {
                if (Event.current.type == EventType.Repaint)
                {
                    WindowStyle.Draw(windowRect.rect, true, true, true, true);
                }

                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    searchTerm = EditorGUILayout.TextField(searchTerm, EditorStyles.toolbarSearchField);
                }

                treeView.searchString = searchTerm;
                treeView.OnGUI(new Rect(4, 25, windowRect.rect.width - 8, windowRect.rect.height - 29));

                selection = treeView.GetSelectedItem();
            }

            if (string.IsNullOrEmpty(selection))
            {
                return;
            }

            Debug.Log($"Selected in tree: {selection}");
            selectionCallback?.Invoke(selection);
            Close();
        }
    }
}
