using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    [System.Obsolete]
    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>(); // usa o UIAudio
    }

    public void PlaySound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
