using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat(TagManager.MusicVolume);
    }

    public void UpdateMusicVolume(float amount)
    {
        float volume = PlayerPrefs.GetFloat(TagManager.MusicVolume);
        volume = Mathf.Clamp01(volume + amount);
        PlayerPrefs.SetFloat(TagManager.MusicVolume, volume);
        audioSource.volume = volume;
    }
}
