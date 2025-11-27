using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPos;

    void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }

    public void Shake(float intensity = 0.15f, float duration = 0.2f)
    {
        StartCoroutine(ShakeRoutine(intensity, duration));
    }

    IEnumerator ShakeRoutine(float intensity, float duration)
    {
        float t = 0;
        while (t < duration)
        {
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * intensity;
            t += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
