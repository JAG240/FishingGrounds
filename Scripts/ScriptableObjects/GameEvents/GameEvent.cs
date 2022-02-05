using System;
using UnityEngine;

//Use this scriptable object to create unqiue global game events 
[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event", order = 52)]
public class GameEvent : ScriptableObject
{
    //Each Game event will have only one listener, other scripts can subscribe to the listener's event
    public event Action invokedEvent;

    public void InvokeEvent()
    {
        invokedEvent?.Invoke();
    }
}
