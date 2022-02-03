using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuDocumentLogic : MenuBaseDocumentLogic
{
    private Button _resume;
    private Button _exit;

    public override VisualTreeAsset GetMenu(UIManager uiManager)
    {
        return uiManager.pauseMenu;
    }

    public override void GetElements(UIManager uiManager, UIDocument menu)
    {
        var root = menu.rootVisualElement;
        _resume = root.Q<Button>("Resume");
        _exit = root.Q<Button>("ExitGame");
    }

    public override void SubscribeEvents(UIManager uiManager)
    {
        _exit.clicked += uiManager.sceneLoader.ExitToMainMenu;
    }

    public override void UnsubscribeEvents(UIManager uiManager)
    {
        _exit.clicked -= uiManager.sceneLoader.ExitToMainMenu;
    }
}
