using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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

    //When a new scene is loaded, if you are the host start as host. 
    private void OnLevelWasLoaded(int level)
    {
        if (!NetworkManager.Singleton.IsClient)
            NetworkManager.Singleton.StartHost();
    }
}
