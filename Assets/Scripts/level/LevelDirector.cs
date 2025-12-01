using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelEvent
{
    // EVENTOS NORMAIS
    public float beat = 0f;
    public GameObject prefab;
    public Vector2 pointA;
    public Vector2 pointB;
    public float timeToStart = 1f;
    public float duration = 1f;
    public float speed = 0f;
    public bool mirrorX = false;

    // BULLET HELL
    public BulletSpawner bulletSpawner;
    public List<BulletPattern> bulletPatterns = new List<BulletPattern>();
}

public class LevelDirector : MonoBehaviour
{
    public static LevelDirector Instance;

    private double pausedDSPTimeOffset = 0;
    private double pausedAtDSP = 0;

    public MusicManager musicManager;

    public List<LevelEvent> events = new List<LevelEvent>();

    public static bool GamePaused = false;


    void Awake()
    {
        Instance = this;
    }

    [System.Obsolete]
    void Start()
    {
        if (musicManager == null)
            musicManager = FindObjectOfType<MusicManager>();

        musicManager.PlaySong(0f);

        // ---------------------------------------------------------
        // Agenda todos os eventos: Obstáculos + Bullet Patterns
        // ---------------------------------------------------------
        foreach (var ev in events)
        {
            double eventDsp = musicManager.BeatToDsp(ev.beat);

            // Obstáculos normais
            double spawnDsp = eventDsp - ev.timeToStart;
            StartCoroutine(SpawnObstacleAtDsp(spawnDsp, ev));

            // Bullet Patterns
            foreach (var pattern in ev.bulletPatterns)
            {
                double bulletDsp = musicManager.BeatToDsp(pattern.beat);
                StartCoroutine(SpawnPatternAtDsp(bulletDsp, ev, pattern));
            }
        }
    }


    // ===========================================
    // SPAWN DE OBSTÁCULOS
    // ===========================================
    IEnumerator SpawnObstacleAtDsp(double spawnDsp, LevelEvent ev)
    {
        while (AudioSettings.dspTime - pausedDSPTimeOffset < spawnDsp)
        {
            while (GamePaused)
                yield return null;

            yield return null;
        }

        SpawnObstacle(ev);
    }

    void SpawnObstacle(LevelEvent ev)
    {
        if (ev.prefab == null) return;

        Vector3 spawnPos = new Vector3(ev.pointA.x, ev.pointA.y, 0f);
        GameObject go = Instantiate(ev.prefab, spawnPos, Quaternion.identity);

        // espelhamento opcional
        if (ev.mirrorX)
        {
            var p = go.transform.position;
            p.x = -p.x;
            go.transform.position = p;
        }

        var to = go.GetComponent<TimedObstacle>();
        if (to != null)
        {
            to.Init(ev.pointA, ev.pointB, ev.timeToStart, ev.duration, ev.speed);
        }
    }


    // ===========================================
    // SPAWN DE BULLET PATTERNS
    // ===========================================
    [System.Obsolete]
    IEnumerator SpawnPatternAtDsp(double dspTarget, LevelEvent ev, BulletPattern pattern)
    {
        while (AudioSettings.dspTime - pausedDSPTimeOffset < dspTarget)
        {
            while (GamePaused)
                yield return null;

            yield return null;
        }

        if (ev.bulletSpawner != null)
            ev.bulletSpawner.ExecutePattern(pattern);
    }


    // ===========================================
    // CONTROLE DE PAUSA REAL (DSP)
    // ===========================================
    public void OnPause()
    {
        pausedAtDSP = AudioSettings.dspTime;
    }

    public void OnResume()
    {
        double now = AudioSettings.dspTime;
        pausedDSPTimeOffset += (now - pausedAtDSP);
    }
}
