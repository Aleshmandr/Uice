using UnityEditor;

namespace Uice.Editor
{
	public static class MenuItemMethods
	{
		private const string CreateMenuBasePath = "Assets/Create/Uice/";

		[MenuItem(CreateMenuBasePath + "Window")]
		public static void CreateWindow()
		{
			ViewCreationWizardEditorWindow.ShowForWindow();
		}

		[MenuItem(CreateMenuBasePath + "Panel")]
		public static void CreatePanel()
		{
			ViewCreationWizardEditorWindow.ShowForPanel();
		}

		[MenuItem("GameObject/UI/Uice/Default UI Frame")]
		public static void CreateDefaultUIFrame()
		{
			UIFrameUtility.CreateDefaultUIFrame();
		}
	}
}
