using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BgmManager : MonoBehaviour
{
    public static BgmManager Instance { get; private set; }

    [Header("Start Settings")]
    [SerializeField] bool playOnStart = true;
    [SerializeField] AudioClip startClip;
    [SerializeField] float startVolume = 0.8f;
    [SerializeField] float fadeInSeconds = 0.5f;

    [Header("Per-Scene BGM (optional)")]
    [SerializeField] SceneBgm[] sceneBgms; // แมปซีน→เพลง (ถ้าอยากเปลี่ยนตามซีน)

    AudioSource src;
    Coroutine fadeCo;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        src = GetComponent<AudioSource>();
        src.loop = true;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void Start()
    {
        if (playOnStart && startClip != null)
            Play(startClip, startVolume, fadeInSeconds, true);
    }

    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        // ถ้าอยากให้เพลงเปลี่ยนตามซีน ให้ใส่รายการไว้ใน sceneBgms
        foreach (var m in sceneBgms)
        {
            if (m.sceneName == s.name && m.clip != null)
            {
                Play(m.clip, startVolume, 0.5f, true);
                break;
            }
        }
    }

    public void Play(AudioClip clip, float volume = 1f, float fadeSeconds = 0f, bool loop = true)
    {
        if (clip == null) return;
        if (src.clip == clip && src.isPlaying) return;

        src.loop = loop;

        if (fadeCo != null) StopCoroutine(fadeCo);
        if (fadeSeconds > 0f && src.isPlaying)
        {
            fadeCo = StartCoroutine(Crossfade(clip, volume, fadeSeconds));
        }
        else
        {
            src.clip = clip;
            src.volume = volume;
            src.Play();
        }
    }

    public void Stop(float fadeSeconds = 0.3f)
    {
        if (fadeCo != null) StopCoroutine(fadeCo);
        if (!src.isPlaying) return;
        if (fadeSeconds <= 0f) { src.Stop(); return; }
        fadeCo = StartCoroutine(FadeOut(fadeSeconds));
    }

    public void SetVolume(float v)
    {
        src.volume = Mathf.Clamp01(v);
    }

    System.Collections.IEnumerator Crossfade(AudioClip next, float targetVol, float t)
    {
        float t1 = 0f; float start = src.volume;
        while (t1 < t)
        {
            t1 += Time.deltaTime;
            src.volume = Mathf.Lerp(start, 0f, t1 / t);
            yield return null;
        }
        src.clip = next;
        src.Play();

        float t2 = 0f;
        while (t2 < t)
        {
            t2 += Time.deltaTime;
            src.volume = Mathf.Lerp(0f, targetVol, t2 / t);
            yield return null;
        }
        src.volume = targetVol;
        fadeCo = null;
    }

    System.Collections.IEnumerator FadeOut(float t)
    {
        float start = src.volume, e = 0f;
        while (e < t)
        {
            e += Time.deltaTime;
            src.volume = Mathf.Lerp(start, 0f, e / t);
            yield return null;
        }
        src.Stop();
        fadeCo = null;
    }

    [System.Serializable]
    public struct SceneBgm
    {
        public string sceneName;
        public AudioClip clip;
    }
}
