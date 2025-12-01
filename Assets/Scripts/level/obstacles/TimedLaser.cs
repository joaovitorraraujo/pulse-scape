using System.Collections;
using UnityEngine;

public class TimedLaser : MonoBehaviour
{
    public GameObject warningPrefab;     // o desenho fraco do laser
    public float warningDuration = 0.8f; // tempo antes do laser real aparecer
    public float laserDuration = 1.2f;   // tempo ativo do laser real

    private GameObject warningInstance;

    [System.Obsolete]
    void Start()
    {
        StartCoroutine(LaserFlow());
    }

    IEnumerator LaserFlow()
    {
        // 1. Cria o aviso no mesmo lugar/ângulo
        warningInstance = Instantiate(warningPrefab, transform.position, transform.rotation);

        // Deixa o laser real invisível enquanto o aviso aparece
        SetLaserVisible(false);

        // 2. Espera o tempo do aviso
        float t = 0;
        while (t < warningDuration)
        {
            if (!LevelDirector.GamePaused)
                t += Time.deltaTime;
            yield return null;
        }

        // 3. Some o aviso
        Destroy(warningInstance);

        // 4. Ativa o laser real
        SetLaserVisible(true);

        // 5. Espera o laser durar
        t = 0;
        while (t < laserDuration)
        {
            if (!LevelDirector.GamePaused)
                t += Time.deltaTime;
            yield return null;
        }

        // 6. Destroi laser no final
        Destroy(gameObject);
    }

    void SetLaserVisible(bool state)
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = state;

        foreach (var c in GetComponentsInChildren<Collider2D>())
            c.enabled = state;
    }
}
