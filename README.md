**<h3>Reflex Based Menu System</h3>**
Reflex is used as a dependency injector for this project. More details: https://github.com/gustavopsantos/Reflex<br/>
Unitask is used for async tasks. More details: https://github.com/Cysharp/UniTask<br/>
<br/>
MenuSystem class provides two generic apis to Load and UnLoad Menu.

To Load a Menu use 

```c#
Unitask MenuSystem.LoadMenu<TData>(string, TData, bool) where TData : IMenuData { ... }
```

To UnLoad a Menu use

```c#
MenuSystem.UnLoadMenu(string assetName) { ... }
MenuSystem.UnLoadMenu(Menu menu) { ... }
```

For more information see Assets.Menu.Example.Runtime Namespace <br/>

Just open ExampleScene and tap run button. To Push Mock Menus on screen search for MenuContainer GameObject under MenuCanvas. Look for Editor buttons below "MenuPushExample" component to push Mock Widgets. 
