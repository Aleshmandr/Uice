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
                Items = new ObservableCollection<MyItemContext>(new []{new MyItemContext("Item 1"), new MyItemContext("Item 2")}),
                TestItem = new MyItemContext("Item Panel")
            };
            uiFrame.ShowWindow<MyWindow>().WithContext(myContext).Execute();
        }
    }
}
