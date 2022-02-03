using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    private static SceneLoader _instance;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += CheckHost;
    }

    //When starting as host only load scene
    public void StartAsHost()
    {
        SceneManager.LoadScene("Fish Lab");
    }

    //If you are a client, connect as client and load the same scene as host 
    public void StartAsClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void ExitToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Main Menu");
    }

    //When a new scene is loaded, if you are the host start as host. 
    private void CheckHost(Scene scene, LoadSceneMode loadMode)
    {
        if (!NetworkManager.Singleton.IsClient && scene.name != "Main Menu")
            NetworkManager.Singleton.StartHost();
    }
}
