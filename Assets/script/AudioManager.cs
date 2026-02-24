using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class NamedClip
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
    }

    [System.Serializable]
    public class SceneMusicEntry
    {
        public string sceneName;
        public int buildIndex = -1;
        public string musicId;
        [Range(0f, 1f)] public float volume = 1f;
        public bool loop = true;
        public float fadeDuration = 0.25f;
    }

    public static AudioManager Instance { get; private set; }

    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Clip Library")]
    public List<NamedClip> musicClips = new List<NamedClip>();
    public List<NamedClip> sfxClips = new List<NamedClip>();

    [Header("Auto Scene Music")]
    public bool autoPlaySceneMusic = true;
    public string defaultMusicId;
    public List<SceneMusicEntry> sceneMusic = new List<SceneMusicEntry>();

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private readonly Dictionary<string, NamedClip> musicLookup = new Dictionary<string, NamedClip>();
    private readonly Dictionary<string, NamedClip> sfxLookup = new Dictionary<string, NamedClip>();
    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        EnsureSources();
        BuildLookupTables();
        ApplyVolumes();

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (autoPlaySceneMusic)
        {
            ApplySceneMusic(SceneManager.GetActiveScene());
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!autoPlaySceneMusic)
        {
            return;
        }

        ApplySceneMusic(scene);
    }

    private void ApplySceneMusic(Scene scene)
    {
        SceneMusicEntry match = null;

        for (int i = 0; i < sceneMusic.Count; i++)
        {
            SceneMusicEntry entry = sceneMusic[i];
            if (entry == null || string.IsNullOrEmpty(entry.musicId))
            {
                continue;
            }

            bool byName = !string.IsNullOrEmpty(entry.sceneName) && entry.sceneName == scene.name;
            bool byIndex = entry.buildIndex >= 0 && entry.buildIndex == scene.buildIndex;

            if (byName || byIndex)
            {
                match = entry;
                break;
            }
        }

        if (match != null)
        {
            PlayMusicById(match.musicId, match.loop, match.fadeDuration, match.volume);
            return;
        }

        if (!string.IsNullOrEmpty(defaultMusicId))
        {
            PlayMusicById(defaultMusicId);
        }
    }

    private void EnsureSources()
    {
        if (musicSource == null)
        {
            musicSource = FindOrCreateSource("MusicSource", true);
        }

        if (sfxSource == null)
        {
            sfxSource = FindOrCreateSource("SfxSource", false);
        }
    }

    private AudioSource FindOrCreateSource(string childName, bool loop)
    {
        Transform existingChild = transform.Find(childName);
        GameObject sourceObj = existingChild != null ? existingChild.gameObject : null;

        if (sourceObj == null)
        {
            sourceObj = new GameObject(childName);
            sourceObj.transform.SetParent(transform, false);
        }

        AudioSource source = sourceObj.GetComponent<AudioSource>();
        if (source == null)
        {
            source = sourceObj.AddComponent<AudioSource>();
        }

        source.playOnAwake = false;
        source.loop = loop;
        return source;
    }

    private void BuildLookupTables()
    {
        musicLookup.Clear();
        sfxLookup.Clear();

        for (int i = 0; i < musicClips.Count; i++)
        {
            NamedClip entry = musicClips[i];
            if (entry != null && !string.IsNullOrEmpty(entry.id) && entry.clip != null)
            {
                musicLookup[entry.id] = entry;
            }
        }

        for (int i = 0; i < sfxClips.Count; i++)
        {
            NamedClip entry = sfxClips[i];
            if (entry != null && !string.IsNullOrEmpty(entry.id) && entry.clip != null)
            {
                sfxLookup[entry.id] = entry;
            }
        }
    }

    public void ApplyVolumes()
    {
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public void PlayMusicById(string id, bool loop = true, float fadeDuration = 0.25f, float volumeScale = 1f)
    {
        if (string.IsNullOrEmpty(id)) return;
        NamedClip entry;
        if (!musicLookup.TryGetValue(id, out entry) || entry.clip == null)
        {
            return;
        }

        float targetVolume = Mathf.Clamp01(entry.volume * musicVolume * volumeScale);
        PlayMusic(entry.clip, targetVolume, loop, fadeDuration);
    }

    public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true, float fadeDuration = 0.25f)
    {
        if (clip == null || musicSource == null) return;

        volume = Mathf.Clamp01(volume);

        if (musicSource.clip == clip && musicSource.isPlaying)
        {
            musicSource.loop = loop;
            musicSource.volume = volume;
            return;
        }

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        if (!musicSource.isPlaying || musicSource.clip == null || fadeDuration <= 0f)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = volume;
            musicSource.Play();
            return;
        }

        fadeRoutine = StartCoroutine(FadeMusicRoutine(clip, volume, loop, fadeDuration));
    }

    public void StopMusic(float fadeDuration = 0.2f)
    {
        if (musicSource == null || !musicSource.isPlaying) return;

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        if (fadeDuration <= 0f)
        {
            musicSource.Stop();
            return;
        }

        fadeRoutine = StartCoroutine(FadeOutRoutine(fadeDuration));
    }

    private IEnumerator FadeMusicRoutine(AudioClip nextClip, float targetVolume, bool loop, float duration)
    {
        float startVolume = musicSource.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        musicSource.clip = nextClip;
        musicSource.loop = loop;
        musicSource.Play();

        t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(0f, targetVolume, t / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
        fadeRoutine = null;
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = musicSource.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = musicVolume;
        fadeRoutine = null;
    }

    public void PlaySfxById(string id, float volumeScale = 1f)
    {
        if (string.IsNullOrEmpty(id) || sfxSource == null) return;

        NamedClip entry;
        if (!sfxLookup.TryGetValue(id, out entry) || entry.clip == null)
        {
            return;
        }

        float finalVolume = Mathf.Clamp01(entry.volume * sfxVolume * volumeScale);
        sfxSource.PlayOneShot(entry.clip, finalVolume);
    }

    public void PlayButtonClick()
    {
        PlaySfxById("ui_click");
    }

    public void PlayCorrect()
    {
        PlaySfxById("correct");
    }

    public void PlayWrong()
    {
        PlaySfxById("wrong");
    }
}