using UnityEngine.UIElements;

public class MainMenuDocumentLogic : UIBaseDocument
{
    #region Vars
    private Button _startHost;
    private Button _startClient;
    #endregion

    #region Document Logic
    public override VisualTreeAsset GetMenu(UIManager uiManager)
    {
        return uiManager.mainMenu;
    }

    public override void GetElements(UIManager uiManager, UIDocument menu)
    {
        var root = menu.rootVisualElement;
        _startHost = root.Q<Button>("StartHost");
        _startClient = root.Q<Button>("StartClient");
    }

    public override void SubscribeEvents(UIManager uiManager)
    {
        _startHost.clicked += uiManager.sceneLoader.StartAsHost;
        _startClient.clicked += uiManager.sceneLoader.StartAsClient;
    }

    public override void UnsubscribeEvents(UIManager uiManager)
    {
        _startHost.clicked -= uiManager.sceneLoader.StartAsHost;
        _startClient.clicked -= uiManager.sceneLoader.StartAsClient;
    }
    #endregion
}
