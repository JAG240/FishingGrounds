using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Vars
    private SceneLoader _sceneLoader;
    #endregion

    #region Main Menu Elements
    public Button _startHost;
    public Button _startClient;
    #endregion

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        _sceneLoader = GameObject.Find("SceneManager").GetComponent<SceneLoader>();
        SceneManager.sceneLoaded += GetUIElements;
    }

    private void GetUIElements(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name == "Main Menu")
            MainMenuUI();
        else
            HUDUI();
    }

    private void MainMenuUI()
    {
        var root = GameObject.Find("MainMenu").GetComponent<UIDocument>().rootVisualElement;
        _startHost = root.Q<Button>("StartHost");
        _startClient = root.Q<Button>("StartClient");

        _startHost.clicked += _sceneLoader.StartAsHost;
        _startClient.clicked += _sceneLoader.StartAsClient;
    }

    private void HUDUI()
    {
        if (_startHost != null)
            _startHost.clicked -= _sceneLoader.StartAsHost;
        if (_startClient != null)
            _startClient.clicked -= _sceneLoader.StartAsClient;
    }
}
