using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class allows global events that any class can invoke or subscribe to. 
 * Should only be used when an event can trigger action in many other classes 
 * and they should not be coupled. 
*/
public class GameEventManager : MonoBehaviour
{
    #region Vars
    private static GameEventManager _instance;
    public static GameEventManager Instance { get { return _instance; } }
    private Dictionary<string, GameEvent> _gameEventContainers = new Dictionary<string, GameEvent>();
    [SerializeField] private GameEvent[] _gameEvents;
    #endregion

    #region Monobehaviour
    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        foreach(GameEvent gameEvent in _gameEvents)
        {
            _gameEventContainers.Add(gameEvent.name, gameEvent);
        }
    }
    #endregion

    #region Game Event Accessor
    public GameEvent GetGameEvent(string name)
    {
        GameEvent gameEvent;

        if (_gameEventContainers.TryGetValue(name, out gameEvent))
            return gameEvent;
        else
            Debug.LogException(new NullReferenceException($"cannot find game event with name {name}"));

        return null;
    }
    #endregion
}
