using System.Collections;
using UnityEngine;

public class TimedObstacle : MonoBehaviour
{
    public Vector2 pointA = Vector2.zero; // posição inicial
    public Vector2 pointB = Vector2.zero; // posição final
    public float timeToStart = 0f; // segundos até começar a ação após ser instanciado
    public float timeToDelete = 2f; // duração da ação (quanto tempo leva a ir de A -> B)
    public float speed = 1f; // alternativa: velocidade (unidades/s) -- usamos timeToDelete por simplicidade
    public bool destroyAtEnd = true;

    void Start()
    {
        // começa a rotina automaticamente ao instanciar
        StartCoroutine(LifecycleRoutine());
    }

    IEnumerator LifecycleRoutine()
    {
        // espera o "timeToStart" — LevelDirector costuma instanciar o prefab com spawnDsp = eventDsp - timeToStart,
        // mas manter este wait aqui torna o prefab autocontido e fácil de reutilizar
        if (timeToStart > 0f)
            yield return new WaitForSeconds(timeToStart);

        // aqui você pode ativar um efeito "antes" (por exemplo aparece, escala, etc.)
        // movimento linear de A -> B em timeToDelete segundos
        Vector3 start = new Vector3(pointA.x, pointA.y, 0f);
        Vector3 end = new Vector3(pointB.x, pointB.y, 0f);
        transform.position = start;

        float elapsed = 0f;
        float duration = Mathf.Max(0.0001f, timeToDelete);

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        // efeito final (ex: ativar blast), ou destruir
        if (destroyAtEnd)
            Destroy(gameObject);
    }

    // opcional: função pública para configurar dinamicamente após Instantiate
    public void Init(Vector2 a, Vector2 b, float startDelay, float duration, float spd = 0f)
    {
        pointA = a; pointB = b;
        timeToStart = startDelay;
        timeToDelete = duration;
        speed = spd;
        transform.position = new Vector3(pointA.x, pointA.y, 0f);
    }
}
