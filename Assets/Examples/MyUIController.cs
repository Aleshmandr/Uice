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
                Header =
                {
                    Value = "Window Header"
                },
                Text =
                {
                    Value = "Window Text"
                },
                Items = new ObservableCollection<MyItemContext>(new[] { new MyItemContext("Item 1"), new MyItemContext("Item 2") }),
                TestItem =
                {
                    Value = new MyItemContext("Item Panel")
                }
            };
            uiFrame.ShowWindow<MyWindow>().WithContext(myContext).Execute();
        }
    }
}
