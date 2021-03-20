using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    AudioSource audioSource;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat(TagManager.SoundVolume);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void UpdateSoundVolume(float amount)
    {
        float volume = PlayerPrefs.GetFloat(TagManager.SoundVolume);
        volume = Mathf.Clamp01(volume + amount);
        PlayerPrefs.SetFloat(TagManager.SoundVolume, volume);
        audioSource.volume = volume;
    }
}
