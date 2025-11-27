using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelEvent
{
    public float beat; // em beats (ex: 4.0)
    public GameObject prefab; // prefab a instanciar (Orb, Laser, etc)
    public Vector2 pointA; // posição inicial
    public Vector2 pointB; // posição final
    public float timeToStart = 1f; // quanto antes do beat precisa spawnar / delay no prefab
    public float duration = 1f; // duração da ação (timeToDelete)
    public float speed = 0f; // opcional
    public bool mirrorX = false; // opcional
}

public class LevelDirector : MonoBehaviour
{
    public static LevelDirector Instance;
    private double pausedDSPTimeOffset = 0;
    private double pausedAtDSP = 0;
    public MusicManager musicManager;
    public List<LevelEvent> events = new List<LevelEvent>();

    public static bool GamePaused = false;

    [System.Obsolete]
    void Awake()
    {
        Instance = this;
    }

    [System.Obsolete]
    void Start()
    {
        if (musicManager == null) musicManager = FindObjectOfType<MusicManager>();
        // Inicia a música (se ainda não iniciou)
        musicManager.PlaySong(0f);

        // Agenda todos os eventos
        foreach (var ev in events)
        {
            double eventDsp = musicManager.BeatToDsp(ev.beat);
            double spawnDsp = eventDsp - ev.timeToStart; // instanciar antes para que o prefab espere timeToStart e atue no beat
            StartCoroutine(SpawnAtDsp(spawnDsp, ev));
        }
    }

    IEnumerator SpawnAtDsp(double spawnDsp, LevelEvent ev)
    {
        // espera até o dspTime de spawn
        while (AudioSettings.dspTime - pausedDSPTimeOffset < spawnDsp)
        {
            while (GamePaused) // pausa REAL
                yield return null;
            yield return null;
        }

        SpawnEvent(ev);
    }

    void SpawnEvent(LevelEvent ev)
    {
        Vector3 spawnPos = new Vector3(ev.pointA.x, ev.pointA.y, 0f);
        GameObject go = Instantiate(ev.prefab, spawnPos, Quaternion.identity);

        if (ev.mirrorX)
        {
            var p = go.transform.position; p.x = -p.x; go.transform.position = p;
            ev.pointA.x = -ev.pointA.x;
            ev.pointB.x = -ev.pointB.x;
        }

        var to = go.GetComponent<TimedObstacle>();
        if (to != null)
        {
            to.Init(ev.pointA, ev.pointB, ev.timeToStart, ev.duration, ev.speed);
        }
    }

    public void OnPause()
    {
        pausedAtDSP = AudioSettings.dspTime;
    }

    public void OnResume()
    {
        double nowDSP = AudioSettings.dspTime;
        double pausedDuration = nowDSP - pausedAtDSP;

        pausedDSPTimeOffset += pausedDuration;
    }

}
