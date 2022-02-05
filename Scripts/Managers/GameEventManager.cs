using System.Collections.Generic;
using UnityEngine;

//This class allows global events that any other class can invoke or subscribe to 
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

    #region Gave Event Accessors

    public GameEvent GetGameEvent(string name)
    {
        GameEvent gameEvent;
        _gameEventContainers.TryGetValue(name, out gameEvent);
        return gameEvent;
    }

    #endregion
}
