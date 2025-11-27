using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JSABProgressBar : MonoBehaviour
{
    [Header("References")]
    public MusicManager musicManager;
    public Image fill;    // MUST be Image.Type = Filled (Horizontal, Left)
    public RectTransform glow; // RectTransform of the glow image (child of Background)

    [Header("Pulse")]
    public float pulseScale = 1.15f;   // small scale multiplier for glow
    public float pulseSpeed = 12f;     // how quickly pulse fades

    private float smoothScale = 1f;
    private float smoothVelocity = 0f;

    

    [Header("Colors")]
    public Color mainColor = new Color(0.2f, 0.7f, 1f);
    public Color glowColor = new Color(0.4f, 1f, 1f);

    RectTransform fillParentRect; // the background rect that defines full width

    void Start()
    {
        if (fill == null) Debug.LogError("Fill Image not assigned (set Image.Type = Filled, Horizontal).");
        if (glow == null) Debug.LogError("Glow RectTransform not assigned.");

        // assume fill is child of a Background rect, get that parent rect for width calculations
        fillParentRect = fill.transform.parent as RectTransform;
        if (fillParentRect == null) fillParentRect = fill.rectTransform;

        // init colors and fill
        fill.color = new Color(mainColor.r, mainColor.g, mainColor.b, 0f);
        if (glow.TryGetComponent<Image>(out var gImg))
            gImg.color = new Color(glowColor.r, glowColor.g, glowColor.b, 0f);

        fill.fillAmount = 0f;
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        if (!musicManager || musicManager.audioSource == null || musicManager.audioSource.clip == null) return;

        float current = musicManager.audioSource.time;
        float total = musicManager.audioSource.clip.length;
        float progress = Mathf.Clamp01(current / total);

        // Smoothly interpolate the visible fill
        fill.fillAmount = Mathf.Lerp(fill.fillAmount, progress, 12f * Time.deltaTime);

        // move glow according to current fill amount
        UpdateGlowPosition(progress);

        // small pulse on beat (scale the glow slightly)
        PulseOnBeat();
    }

    void UpdateGlowPosition(float progress)
    {
        if (fillParentRect == null || glow == null) return;

        float parentWidth = fillParentRect.rect.width;
        // anchoredPosition.x is left-based (since anchor is left), compute x in local coords:
        float x = Mathf.Lerp(0f, parentWidth, progress);

        // move glow's anchoredPosition (y preserved)
        Vector2 anchored = glow.anchoredPosition;
        anchored.x = x;
        glow.anchoredPosition = anchored;

        // garante que glow fique em cima visualmente
        glow.SetAsLastSibling();
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < 1f)
        {
            float a = Mathf.SmoothStep(0f, 1f, t);
            var c = fill.color; c.a = a; fill.color = c;
            if (glow.TryGetComponent<Image>(out var gImg))
            {
                var gc = gImg.color;
                gc.a = a * 0.6f;
                gImg.color = gc;
            }
            t += Time.deltaTime * 1.6f;
            yield return null;
        }
    }

    // Simple pulse that triggers every beat (approx)
    void PulseOnBeat()
    {
        float bpm = musicManager.bpm;
        float current = musicManager.audioSource.time;

        float beat = current / (60f / bpm);
        float fractional = beat - Mathf.Floor(beat);

        float pulseValue = 1f - Mathf.Clamp01(fractional * pulseSpeed);

        float targetScale = Mathf.Lerp(1f, pulseScale, pulseValue);

        // Suaviza a transição entre o valor atual e o desejado
        smoothScale = Mathf.SmoothDamp(smoothScale, targetScale, ref smoothVelocity, 0.05f);

        glow.localScale = new Vector3(smoothScale, smoothScale, 1f);
    }

    // optional: call this to set colors at runtime
    public void SetColors(Color main, Color gl)
    {
        mainColor = main; glowColor = gl;
        if (fill != null) fill.color = main;
        if (glow != null && glow.TryGetComponent<Image>(out var gImg)) gImg.color = gl;
    }
}
