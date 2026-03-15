using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance { get; private set; }

    [Header("Pluie")]
    public ParticleSystem rainParticles;   // assigner le ParticleSystem de pluie
    public AudioSource rainAudio;          // son de pluie (loop)

    [Header("Fog (Unity builtin)")]
    public bool useUnityFog = true;
    public float fogTargetDensity = 0.02f; // valeur cible pour la pluie
    public float fogClearDensity = 0.0005f; // valeur pour ciel clair
    public float fogLerpSpeed = 0.8f;

    [Header("Lumière")]
    public Light directionalLight;
    public float lightClearIntensity = 1.0f;
    public float lightRainIntensity = 0.5f;

    Coroutine rainCoroutine;
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        // état initial
        if (rainParticles != null) rainParticles.Stop();
        if (rainAudio != null) rainAudio.Stop();

        if (useUnityFog)
        {
            RenderSettings.fog = true; // on active le fog global (tu peux le désactiver et utiliser cube faux fog)
            RenderSettings.fogDensity = fogClearDensity;
        }
        if (directionalLight != null) directionalLight.intensity = lightClearIntensity;
    }

    public void StartRain(float fadeTime = 1.0f)
    {
        if (rainCoroutine != null) StopCoroutine(rainCoroutine);
        rainCoroutine = StartCoroutine(RainRoutine(true, fadeTime));
    }

    public void StopRain(float fadeTime = 1.0f)
    {
        if (rainCoroutine != null) StopCoroutine(rainCoroutine);
        rainCoroutine = StartCoroutine(RainRoutine(false, fadeTime));
    }

    IEnumerator RainRoutine(bool start, float fadeTime)
    {
        float t = 0f;
        float startFog = RenderSettings.fogDensity;
        float targetFog = start ? fogTargetDensity : fogClearDensity;

        float startLight = directionalLight != null ? directionalLight.intensity : 0f;
        float targetLight = start ? lightRainIntensity : lightClearIntensity;

        // play/stop particles & audio at the beginning or end depending on desired timing
        if (start)
        {
            if (rainParticles != null) rainParticles.Play();
            if (rainAudio != null) rainAudio.Play();
        }

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeTime);
            RenderSettings.fogDensity = Mathf.Lerp(startFog, targetFog, alpha * fogLerpSpeed);
            if (directionalLight != null) directionalLight.intensity = Mathf.Lerp(startLight, targetLight, alpha);
            yield return null;
        }

        RenderSettings.fogDensity = targetFog;
        if (directionalLight != null) directionalLight.intensity = targetLight;

        if (!start)
        {
            if (rainParticles != null) rainParticles.Stop();
            if (rainAudio != null) rainAudio.Stop();
        }
        rainCoroutine = null;
    }
}
