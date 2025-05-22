using Mace;
using Uice;
using UnityEngine;

namespace Uice
{
    public class WidgetBinder : ComponentBinder
    {
        [SerializeField] private Widget widget;
        [SerializeField] private BindingInfo show = BindingInfo.Variable<bool>();
        private bool skipTransitionAnimation;

        protected override void Awake()
        {
            base.Awake();
            RegisterVariable<bool>(show).OnChanged(HandleVisibilityChange);
        }

        protected override void OnEnable()
        {
            skipTransitionAnimation = true;
            base.OnEnable();
        }
        
        private void HandleVisibilityChange(bool value)
        {
            if (value)
            {
                widget.Show();
            } else
            {
                widget.Hide(skipTransitionAnimation ? EmptyTransition.Transition : null);
            }
            skipTransitionAnimation = false;
        }

#if UNITY_EDITOR
        private void Reset()
        {
            if (widget == null)
            {
                widget = GetComponent<Widget>();
            }
        }
#endif
    }
}