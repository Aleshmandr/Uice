using UnityEngine;

namespace Uice.Examples
{
    public class MyUIController : MonoBehaviour
    {
        [SerializeField] private UIFrame uiFrame;
        [SerializeField] private MyWindow myWindowPrefab;

        private void Start()
        {
            uiFrame.RegisterView(Instantiate(myWindowPrefab));

            var myContext = new MyContext
            {
                Header = "Window Header",
                Text = "Window Text",
                //Items = new ObservableCollection<MyItemData>(new []{new MyItemData("Item 1"), new MyItemData("Item 2")})
            };
            uiFrame.ShowWindow<MyWindow>().WithContext(myContext).Execute();
        }
    }
}
