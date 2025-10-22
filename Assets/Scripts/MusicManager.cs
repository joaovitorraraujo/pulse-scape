using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float bpm = 120f;
    public double startDelay = 0.2; 
    public double dspSongStartTime { get; private set; } 

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    public void PlaySong(float offsetSeconds = 0f)
    {
        double startDsp = AudioSettings.dspTime + startDelay;
        audioSource.PlayScheduled(startDsp);
        dspSongStartTime = startDsp - offsetSeconds;
    }

    
    public double BeatToDsp(double beat)
    {
        double secondsPerBeat = 60.0 / bpm;
        return dspSongStartTime + beat * secondsPerBeat;
    }
}
