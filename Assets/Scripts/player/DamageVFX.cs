using System.Collections;
using UnityEngine;

public class DamageVFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    [Header("Flash Settings")]
    public Color hitColor = new Color(1f, 0.3f, 0.3f);
    public float flashDuration = 0.05f; 
    public int flashCount = 10;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Debug.Log("SpriteRenderer encontrado: " + sr);
        originalColor = sr.color;
    }

    public void PlayHitFlash()
    {
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            sr.color = hitColor;
            yield return new WaitForSeconds(flashDuration);

            sr.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
