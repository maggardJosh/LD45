using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private static AudioManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<AudioManager>();
            return _instance;
        }
    }

    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

    private bool MuteMusic
    {
        get
        {
            return PlayerPrefs.GetInt("MuteMusic", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("MuteMusic", value ? 1 : 0);
        }
    }

    void Awake()
    {
        _instance = this;
        musicAudioSource.volume = MuteMusic ? 0 : 1;
    }

    public static void PlayOneShot(AudioClip ac)
    {
        if (!Instance.sfxAudioSource.isPlaying)
            Instance.sfxAudioSource.pitch = Random.Range(.9f, 1.1f);
        Instance.sfxAudioSource.PlayOneShot(ac);
    }

    public void ToggleMusicSound()
    {
        MuteMusic = !MuteMusic;
        Instance.musicAudioSource.volume = MuteMusic ? 0 : 1;
    }

}
