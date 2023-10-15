using UnityEngine;

namespace Uice.Examples
{
    public class MySecondContextSetter : MonoBehaviour
    {
        [SerializeField] private ContextComponent contextComponent;
        
        private void Awake()
        {
            var context = new MySecondContext
            {
                Text =
                {
                    Value = "Text Text"
                }
            };
            contextComponent.Context = context;
        }
    }
}
