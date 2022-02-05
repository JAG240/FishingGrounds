using System;

//Subscribe to this classes event to receive when it is invoked 
public class GameEventListener
{
    public event Action invokedEvent;

    public void Invoke()
    {
        invokedEvent?.Invoke();
    }

    public GameEventListener(GameEvent gameEvent)
    {
        gameEvent.Subscribe(this);
    }
}
