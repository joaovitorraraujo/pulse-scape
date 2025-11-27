using System.Collections;
using UnityEngine;

public class TimedObstacle : MonoBehaviour
{
    public Vector2 pointA = Vector2.zero;
    public Vector2 pointB = Vector2.zero;
    public float timeToStart = 0f;
    public float timeToDelete = 2f;
    public float speed = 1f;
    public bool destroyAtEnd = true;

    void Start()
    {
        StartCoroutine(LifecycleRoutine());
    }

    IEnumerator LifecycleRoutine()
    {
        // Aguardar o timeToStart respeitando PAUSE
        float startTimer = 0f;
        while (startTimer < timeToStart)
        {
            if (!LevelDirector.GamePaused)
                startTimer += Time.deltaTime;

            yield return null;
        }

        // movimento A → B
        Vector3 start = new Vector3(pointA.x, pointA.y, 0f);
        Vector3 end = new Vector3(pointB.x, pointB.y, 0f);
        transform.position = start;

        float elapsed = 0f;
        float duration = Mathf.Max(0.0001f, timeToDelete);

        while (elapsed < duration)
        {
            if (!LevelDirector.GamePaused)
            {
                float t = elapsed / duration;
                transform.position = Vector3.Lerp(start, end, t);

                elapsed += Time.deltaTime;
            }

            yield return null;
        }

        transform.position = end;

        if (destroyAtEnd)
            Destroy(gameObject);
    }

    public void Init(Vector2 a, Vector2 b, float startDelay, float duration, float spd = 0f)
    {
        pointA = a;
        pointB = b;
        timeToStart = startDelay;
        timeToDelete = duration;
        speed = spd;

        transform.position = new Vector3(pointA.x, pointA.y, 0f);
    }
}
