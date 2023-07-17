using Uice.Utils;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Uice
{
    [RequireComponent(typeof(Image))]
    public class ImageBinder : GraphicBinder
    {
        [SerializeField] private BindingInfo sourceImage = BindingInfo.Variable<Sprite>();
        [SerializeField] private BindingInfo fillAmount = BindingInfo.Variable<float>();

        private Image imageComponent;

        protected override void Awake()
        {
            base.Awake();
            imageComponent = GetComponent<Image>();
            RegisterVariable<Sprite>(sourceImage).OnChanged(HandleSourceImageChange);
            RegisterVariable<float>(fillAmount).OnChanged(HandleFillAmountChange);
        }

        private void HandleFillAmountChange(float newValue)
        {
            imageComponent.fillAmount = newValue;
        }

        private void HandleSourceImageChange(Sprite newValue)
        {
            imageComponent.overrideSprite = newValue;
        }
        
#if UNITY_EDITOR
        [MenuItem("CONTEXT/Image/Add Binder")]
        private static void AddBinder(MenuCommand command)
        {
            Image context = (Image)command.context;
            context.GetOrAddComponent<ImageBinder>();
        }
#endif
    }
}
