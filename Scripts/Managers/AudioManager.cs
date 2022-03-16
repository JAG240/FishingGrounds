using UnityEngine;
using Unity.Netcode;

public class AudioManager : NetworkBehaviour
{
    [SerializeField] private AudioClip biteSound;
    private AudioSource _playerAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        _playerAudioSource = GetComponent<AudioSource>();

        if(!IsLocalPlayer)
        {
            GetComponent<AudioListener>().enabled = false;
            _playerAudioSource.enabled = false;
            enabled = false;
            return;
        }

        GameEventManager.Instance.fishBite.invokedEvent += PlayBiteSound;
    }

    void OnDisable()
    {
        GameEventManager.Instance.fishBite.invokedEvent -= PlayBiteSound;
    }

    private void PlayBiteSound()
    {
        _playerAudioSource.loop = false;
        _playerAudioSource.clip = biteSound;
        _playerAudioSource.Play();
    }
}
