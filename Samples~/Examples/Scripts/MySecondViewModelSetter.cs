using UnityEngine;

namespace Uice.Examples
{
    public class MySecondViewModelSetter : MonoBehaviour
    {
        [SerializeField] private ViewModelComponent viewModelComponent;
        
        private void Awake()
        {
            var context = new MySecondViewModel
            {
                Text =
                {
                    Value = "Text Text"
                }
            };
            viewModelComponent.ViewModel = context;
        }
    }
}
