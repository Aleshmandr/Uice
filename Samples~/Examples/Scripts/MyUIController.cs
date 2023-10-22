using UnityEngine;

namespace Uice.Examples
{
    public class MyUIController : MonoBehaviour
    {
        [SerializeField] private MyWindow windowPrefab;
        [SerializeField] private UIFrame uiFrame;

        private void Start()
        {
            uiFrame.RegisterView(Instantiate(windowPrefab));
            
            var myContext = new MyViewModel
            {
                Header =
                {
                    Value = "Window Header"
                },
                Text =
                {
                    Value = "Window Text"
                },
                Items = new ObservableCollection<MyItemViewModel>(new[] { new MyItemViewModel("Item 1"), new MyItemViewModel("Item 2") }),
                TestItem =
                {
                    Value = new MyItemViewModel("Item Panel")
                }
            };

            uiFrame.ShowWindow<MyWindow>().WithViewModel(myContext).Execute();
        }
    }
}
