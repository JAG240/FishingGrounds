using UnityEngine.UIElements;

public class PauseMenuDocumentLogic : UIBaseDocument
{
    #region Vars
    private Button _resume;
    private Button _exit;
    #endregion

    #region Document Logic
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
        _resume.clicked += GameEventManager.Instance.GetGameEvent("ExitMenu").InvokeEvent;
        _exit.clicked += GameEventManager.Instance.GetGameEvent("ExitGame").InvokeEvent;
        _exit.clicked += uiManager.sceneLoader.ExitToMainMenu;
    }

    public override void UnsubscribeEvents(UIManager uiManager)
    {
        _resume.clicked -= GameEventManager.Instance.GetGameEvent("ExitMenu").InvokeEvent;
        _exit.clicked -= GameEventManager.Instance.GetGameEvent("ExitGame").InvokeEvent;
        _exit.clicked -= uiManager.sceneLoader.ExitToMainMenu;
    }
    #endregion
}
