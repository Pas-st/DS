using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundSoundVolume : MonoBehaviour
{
    public AudioClip musicClip;   // 👈 hier reinziehen

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (musicClip == null)
        {
            Debug.LogError("Keine Musik zugewiesen!");
            return;
        }

        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        audioSource.Play();
    }
}