using UnityEngine;
using UnityEngine.UIElements;
public abstract class MenuBaseDocumentLogic
{
    public abstract VisualTreeAsset GetMenu(UIManager uiManager);
    public abstract void GetElements(UIManager uiManager, UIDocument menu);
    public abstract void SubscribeEvents(UIManager uiManager);
    public abstract void UnsubscribeEvents(UIManager uiManager);
}
