using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class VFXHelpers : MonoBehaviour
{
    // exemplo de fade para SpriteRenderer
    public static IEnumerator FadeSprite(SpriteRenderer rend, float targetAlpha, float duration)
    {
        float start = rend.color.a;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float a = Mathf.Lerp(start, targetAlpha, elapsed / duration);
            Color c = rend.color; c.a = a; rend.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }
        Color cc = rend.color; cc.a = targetAlpha; rend.color = cc;
    }

    // scale animation
    public static IEnumerator ScaleTo(Transform t, Vector3 targetScale, float duration)
    {
        Vector3 start = t.localScale;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            t.localScale = Vector3.Lerp(start, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        t.localScale = targetScale;
    }
}
