using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

/**
 * This class has the only reference to the UI Document for displaying visual tree assets.
 * It will run UIBaseDocuments logic to set the UI visible, gather elements, subscribe actions
 * to UI elements, and unsubsribe these events when unloaded. 
 */
public class UIManager : MonoBehaviour
{
    #region UI Documents
    [SerializeField] private UIDocument _menu;
    public VisualTreeAsset mainMenu;
    public VisualTreeAsset pauseMenu;
    #endregion

    #region Vars
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    [HideInInspector] public SceneLoader sceneLoader { get; private set; }
    private UIBaseDocument _currentMenu;
    private UIBaseDocument _mainMenuDocLogic = new MainMenuDocumentLogic();
    #endregion

    #region Monobehaviour
    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this.gameObject);
        sceneLoader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += LoadSceneDefaultMenu;
        LoadUIDocument(_mainMenuDocLogic);
    }
    #endregion

    #region Menu Logic
    private void LoadSceneDefaultMenu(Scene scene, LoadSceneMode loadMode)
    {
        if (_currentMenu != null)
            UnloadUIDocument();

        if (scene.name == "Main Menu")
            LoadUIDocument(_mainMenuDocLogic);
    }

    public void LoadUIDocument(UIBaseDocument menuBase)
    {
        if (_currentMenu != null)
            UnloadUIDocument();

        _menu.visualTreeAsset = menuBase.GetMenu(this);
        menuBase.GetElements(this, _menu);
        menuBase.SubscribeEvents(this);
        _currentMenu = menuBase;
    }

    public void UnloadUIDocument()
    {
        if (_currentMenu == null || _menu == null)
            return;

        _menu.visualTreeAsset = null;
        _currentMenu.UnsubscribeEvents(this);
        _currentMenu = null;
    }
    #endregion
}
