using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Menus
    [SerializeField] private UIDocument _menu;
    public VisualTreeAsset mainMenu;
    public VisualTreeAsset pauseMenu;
    #endregion

    #region Vars
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    [HideInInspector] public SceneLoader sceneLoader;
    private MenuBaseDocumentLogic _currentMenu;
    private MenuBaseDocumentLogic _mainMenuDocLogic = new MainMenuDocumentLogic();
    #endregion

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
        LoadMenu(_mainMenuDocLogic);
    }

    private void LoadSceneDefaultMenu(Scene scene, LoadSceneMode loadMode)
    {
        if (_currentMenu != null)
            UnloadMenu();

        if (scene.name == "Main Menu")
            LoadMenu(_mainMenuDocLogic);
    }

    public void LoadMenu(MenuBaseDocumentLogic menuBase)
    {
        if (_currentMenu != null)
            UnloadMenu();

        _menu.visualTreeAsset = menuBase.GetMenu(this);
        menuBase.GetElements(this, _menu);
        menuBase.SubscribeEvents(this);
        _currentMenu = menuBase;
    }

    public void UnloadMenu()
    {
        if (_menu == null)
            return;

        _menu.visualTreeAsset = null;
        _currentMenu.UnsubscribeEvents(this);
        _currentMenu = null;
    }
}
