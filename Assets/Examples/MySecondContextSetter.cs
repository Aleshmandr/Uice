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
                Text = "Text Text"
            };
            contextComponent.Context = context;
        }
    }
}
