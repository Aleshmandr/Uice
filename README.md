# Uice
This project was originally a fork of the [jUIce project](https://github.com/MiguelCriado/jUice) by [Miguel Criado](https://github.com/MiguelCriado) with some fixes and improvements.

Uice is a MVVM UI framework built on top of [Unity's uGui](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/index.html) solution with [Mace MVVM framework](https://github.com/Aleshmandr/Mace).
It provides a series of systems and guidelines to boost your runtime UI development within Unity.

It aims to split the UI workflow into two distinct phases: technical and stylistic. This means that programmers and designers can cooperate together to achieve the final version of the UI. What this also means is that Uice requires some technical insight to be used; you'll need to code your ViewModels.

This project is inspired by the amazing [deVoid UI Framework](https://github.com/yankooliveira/uiframework) by Yanko Oliveira. 

## UIFrame
Uice's view hierarchy is organized below a root element called the UIFrame. It contains a series of layers to sort your views based in certain rules and acts as the service to handle the views' visibility. Before opening any view, it needs to be registered in the UIFrame.

```csharp
using Uice;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [SerializeField] private UiFrame uiFrame;
    [SerializeField] private MyView myView;

    private void Start()
    {
        // A view needs to be registered before being shown
        uiFrame.Register<MyView>(myView);
    }

    private void Update()
    {
        if (Input.KeyDown(KeyCode.Escape))
        {
            if (myView.IsVisible)
            {
                uiFrame.Hide<MyView>().Execute();
            }
            else
            {
                uiFrame.Show<MyView>().WithViewModel(new MyViewModel()).Execute();
            }
        }
    }
}
```

Your typical UiFrame will contain a set of layers that will define the order in which your views will be sorted. 

The default layer stack that Uice can automatically build for you would be something like this:
1. **Panel Layer** (Panels with no priority, usually HUD elements)
2. **Window Layer** (Regular windows)
3. **Priority Panel Layer** (Panels set to `Prioritary`, shown on top of regular windows)
4. **Priority Window Layer** (Popups)
5. **Overlay Panel Layer** (Panels shown on top of popups)
6. **Tutorial Panel Layer** (To overlay your UI with tutorial elements)
7. **Blocker Layer** (Highest priority windows, like loading screens or connectivity issues displayers)

Keep in mind that this is the default hierarchy. It should be enough for most of the games but you are free to arrange it in some other order or even create your custom layers to fit your game needs.

## View
There are two kinds of views in Uice: Windows and Panels. The main reason for this distinction is their overall behavior and conceptual meaning; windows are the focal element of information for the user and there can only be one of them focused at any given moment, whereas any number of panels can be open at the same time and alongside the current window so they work as containers for complementary info.

### Window
A window is the focal element of UI information at any given time (if any). They usually take up all the space available in the screen and, therefore, only one of them can be focused in a particular moment. They are stored in the history stack and will be automatically shown again with the same ViewModel when the next window in the stack is closed.

According to their behavior, there are two kinds of window: regular and popup. 

A **regular window** is your main source of dialog with the player. They usually take all the screen space and conform the menu tree of your game.

A **popup**, on the other hand, is a volatile kind of window that is supposed to be shown over the current displayed views (both windows and panels). They are automatically shown over a background shadow to occlude previous information.

Both of them can be enqueued so they are shown in order when the previous one is closed. 

### Panel
A panel is a block of UI information which is not bound to any particular window. There can be as many panels shown at the same time as your game needs and, because of that, they usually contain complementary information that can outlive windows after they are hidden.

## Transitions
A UI without subtle animations is like a muffin without topping. You'll want your views to be animated when they transition into a visible or invisible state. That's achieved with `Transition`s.

Transitions are `Component`s that can be attached to any `View` in order to define how will it behave when a `Show` or `Hide` operation is requested on it. `ShowTransition` and `HideTransition` are independent fields in the `View`'s editor, so you can set different behaviors based on the direction of the transition. If no transition is set for a particular operation, the View's GameObject will be just activated/deactivated when requested.  

There is a set of common transitions already included in Uice's codebase, but you can create your own if there's none that satisfy your UI/UX requirements.

## Bindings
Bindings are the last link the framework chain. They are responsible of providing updated information about changes in the ViewModel to the Unity Components that use them.

![image](https://github.com/Aleshmandr/Uice/assets/11294931/e2144732-e6aa-4be0-95b5-aa29a6f0b6e8)

You can read more about MVVM and bindings here [Mace MVVM](https://github.com/Aleshmandr/Mace)

## Additional Goodies
The framework also include a couple of subsystems that could be used in your non UI modules. 

### Tweening
There's also a simple tween library designed to mimic [DOTween](http://dotween.demigiant.com/) plugin, which is used to add many tweening features in the framework.
