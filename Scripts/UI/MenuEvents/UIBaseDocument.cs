using UnityEngine;
using UnityEngine.UIElements;

/**
 * This abstract class will ensure that all UI visual trees have the ability to
 * return the visual tree, get document elements, subscribe events to document elements,
 * and unsubsribe these events. 
 */
public abstract class UIBaseDocument
{
    public abstract VisualTreeAsset GetMenu(UIManager uiManager);
    public abstract void GetElements(UIManager uiManager, UIDocument menu);
    public abstract void SubscribeEvents(UIManager uiManager);
    public abstract void UnsubscribeEvents(UIManager uiManager);
}
