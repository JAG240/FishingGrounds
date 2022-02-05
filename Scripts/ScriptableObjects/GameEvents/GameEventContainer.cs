[System.Serializable]
public class GameEventContainer
{

    #region Public Vars
    //Can be set in the inspector as this class is searlized
    public string name;
    public GameEvent gameEvent;
    public GameEventListener gameEventListener;
    #endregion

    #region Private Vars
    //privately set variables can only be set on intialization from the public vars about for protection
    public string _name { get; private set; }
    public GameEvent _gameEvent { get; private set; }
    public GameEventListener _gameEventListener { get; private set; }
    #endregion

    #region Constructors
    public GameEventContainer(string name, GameEvent gameEvent)
    {
        _name = name;
        _gameEvent = gameEvent;
        _gameEventListener = new GameEventListener(gameEvent);
    }
    #endregion
}