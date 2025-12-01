using System.Collections;
using UnityEngine;

public class ItemFloat : MonoBehaviour
{
    [Header("Flutuação")]
    public float floatSpeed = 1.5f;
    private Vector2 randomDirection;

    [Header("Spawn FX")]
    public float spawnFadeTime = 0.3f;
    public float spawnScaleTime = 0.3f;
    public float initialScale = 0.3f;

    [Header("Auto-Destruição")]
    public float lifeTime = 6f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void Start()
    {
        // direção aleatória
        randomDirection = Random.insideUnitCircle.normalized;

        // efeitos visuais
        StartCoroutine(SpawnFX());
        StartCoroutine(AutoDestroy());
    }

    void Update()
    {
        transform.position += (Vector3)randomDirection * floatSpeed * Time.deltaTime;
    }

    IEnumerator SpawnFX()
    {
        float t = 0f;
        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        Vector3 finalScale = transform.localScale;  // escala original do prefab
        Vector3 startScale = finalScale * initialScale;

        transform.localScale = startScale;

        while (t < 1f)
        {
            t += Time.deltaTime / spawnScaleTime;

            // fade-in
            float alpha = Mathf.Lerp(0f, 1f, t);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // scale para a escala verdadeira
            transform.localScale = Vector3.Lerp(startScale, finalScale, t);

            yield return null;
        }
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(lifeTime);

        // fade out suave antes de sumir
        float t = 0f;
        Color startColor = sr.color;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.3f; // tempo do fade out
            float a = Mathf.Lerp(1f, 0f, t);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, a);
            yield return null;
        }

        Destroy(gameObject);
    }
}
