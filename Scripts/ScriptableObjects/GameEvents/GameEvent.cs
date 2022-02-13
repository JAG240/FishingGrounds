using System;
using UnityEngine;

/**
 * This is a scriptable object class that will be used to create new global game events in the inspector.
 * It only contains an event that can be easily invoked and subscribed to.
 */
[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event", order = 52)]
public class GameEvent : ScriptableObject
{
    public event Action invokedEvent;

    public void InvokeEvent()
    {
        invokedEvent?.Invoke();
    }
}
