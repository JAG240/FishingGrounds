using UnityEngine;

//Use this scriptable object to create unqiue global game events 
[CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event", order = 52)]
public class GameEvent : ScriptableObject
{
    //Each Game event will have only one listener, other scripts can subscribe to the listener's event
    private GameEventListener _listener;

    public void Invoke()
    {
        _listener.Invoke();
    }

    public void Subscribe(GameEventListener eventListener) 
    {
        _listener = eventListener;
    }

    public void UnSubscribe()
    {
        _listener = null;
    }
}
