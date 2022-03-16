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
    #endregion

    #region Public Accessors
    public GameEvent exitGame { get; private set; }
    public GameEvent exitMenu { get; private set; }
    public GameEvent fishBite { get; private set; }
    public GameEvent fishHooked { get; private set; }
    #endregion

    #region Private Mutator
    [SerializeField] private GameEvent _exitGame;
    [SerializeField] private GameEvent _exitMenu;
    [SerializeField] private GameEvent _fishBite;
    [SerializeField] private GameEvent _fishHooked;
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
        exitGame = _exitGame;
        exitMenu = _exitMenu;
        fishBite = _fishBite;
        fishHooked = _fishHooked;
    }
    #endregion
}
