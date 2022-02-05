using System.Collections.Generic;
using UnityEngine;

//This class allows global events that any other class can invoke or subscribe to 
public class GameEventManager : MonoBehaviour
{
    #region Vars
    private static GameEventManager _instance;
    public static GameEventManager Instance { get { return _instance; } }
    private Dictionary<string, GameEventContainer> _gameEventContainers = new Dictionary<string, GameEventContainer>();
    [SerializeField] private GameEventContainer[] _gameEvents;
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
        foreach(GameEventContainer container in _gameEvents)
        {
            _gameEventContainers.Add(container.name, new GameEventContainer(container.name, container.gameEvent));
        }
    }
    #endregion

    #region Game Event Accessors
    public GameEvent GetGameEvent(string name)
    {
        GameEventContainer container;
        _gameEventContainers.TryGetValue(name, out container);
        return container._gameEvent;
    }

    public GameEventListener GetGameEventListener(string name)
    {
        GameEventContainer container;
        _gameEventContainers.TryGetValue(name, out container);
        return container._gameEventListener;
    }
    #endregion
}
