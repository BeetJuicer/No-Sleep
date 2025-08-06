using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class AudioClipData
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(-3f, 3f)]
        public float pitch = 1f;
        public bool loop = false;
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource1;
    [SerializeField] private AudioSource bgmSource2;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM Settings")]
    [SerializeField] private List<AudioClipData> bgmClips = new List<AudioClipData>();
    [SerializeField] private float bgmFadeDuration = 2f;
    [Range(0f, 1f)]
    [SerializeField] private float bgmMasterVolume = 1f;

    [Header("SFX Settings")]
    [SerializeField] private List<AudioClipData> sfxClips = new List<AudioClipData>();
    [Range(0f, 1f)]
    [SerializeField] private float sfxMasterVolume = 1f;
    [SerializeField] private int maxSfxSources = 10;

    // Private variables
    private Dictionary<string, AudioClipData> bgmDict = new Dictionary<string, AudioClipData>();
    private Dictionary<string, AudioClipData> sfxDict = new Dictionary<string, AudioClipData>();
    private List<AudioSource> sfxSources = new List<AudioSource>();
    private AudioSource currentBgmSource;
    private AudioSource nextBgmSource;
    private bool isFading = false;
    private string currentBgmName = "";

    // Singleton pattern
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioManager()
    {
        // Create audio sources if not assigned
        if (bgmSource1 == null)
        {
            bgmSource1 = gameObject.AddComponent<AudioSource>();
            bgmSource1.loop = true;
            bgmSource1.playOnAwake = false;
        }

        if (bgmSource2 == null)
        {
            bgmSource2 = gameObject.AddComponent<AudioSource>();
            bgmSource2.loop = true;
            bgmSource2.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        // Set initial BGM source
        currentBgmSource = bgmSource1;
        nextBgmSource = bgmSource2;

        // Create SFX sources pool
        CreateSfxSourcesPool();

        // Build dictionaries
        BuildAudioDictionaries();
    }

    private void CreateSfxSourcesPool()
    {
        sfxSources.Clear();
        sfxSources.Add(sfxSource);

        for (int i = 1; i < maxSfxSources; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            sfxSources.Add(newSource);
        }
    }

    private void BuildAudioDictionaries()
    {
        bgmDict.Clear();
        sfxDict.Clear();

        foreach (var bgm in bgmClips)
        {
            if (!string.IsNullOrEmpty(bgm.name) && bgm.clip != null)
            {
                bgmDict[bgm.name] = bgm;
            }
        }

        foreach (var sfx in sfxClips)
        {
            if (!string.IsNullOrEmpty(sfx.name) && sfx.clip != null)
            {
                sfxDict[sfx.name] = sfx;
            }
        }
    }

    #region BGM Methods

    public void PlayBGM(string bgmName, bool fade = true)
    {
        if (!bgmDict.ContainsKey(bgmName))
        {
            Debug.LogWarning($"BGM '{bgmName}' not found!");
            return;
        }

        if (currentBgmName == bgmName && currentBgmSource.isPlaying)
        {
            return; // Already playing this BGM
        }

        AudioClipData bgmData = bgmDict[bgmName];

        if (fade && currentBgmSource.isPlaying && !isFading)
        {
            StartCoroutine(FadeBGM(bgmData, bgmName));
        }
        else
        {
            PlayBGMDirect(bgmData, bgmName);
        }
    }

    private void PlayBGMDirect(AudioClipData bgmData, string bgmName)
    {
        currentBgmSource.clip = bgmData.clip;
        currentBgmSource.volume = bgmData.volume * bgmMasterVolume;
        currentBgmSource.pitch = bgmData.pitch;
        currentBgmSource.loop = bgmData.loop;
        currentBgmSource.Play();
        currentBgmName = bgmName;
    }

    private IEnumerator FadeBGM(AudioClipData newBgmData, string newBgmName)
    {
        isFading = true;
        float fadeTimer = 0f;
        float originalVolume = currentBgmSource.volume;

        // Setup next BGM source
        nextBgmSource.clip = newBgmData.clip;
        nextBgmSource.volume = 0f;
        nextBgmSource.pitch = newBgmData.pitch;
        nextBgmSource.loop = newBgmData.loop;
        nextBgmSource.Play();

        // Fade out current, fade in next
        while (fadeTimer < bgmFadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float progress = fadeTimer / bgmFadeDuration;

            currentBgmSource.volume = Mathf.Lerp(originalVolume, 0f, progress);
            nextBgmSource.volume = Mathf.Lerp(0f, newBgmData.volume * bgmMasterVolume, progress);

            yield return null;
        }

        // Finalize fade
        currentBgmSource.Stop();
        currentBgmSource.volume = originalVolume;
        nextBgmSource.volume = newBgmData.volume * bgmMasterVolume;

        // Swap sources
        AudioSource temp = currentBgmSource;
        currentBgmSource = nextBgmSource;
        nextBgmSource = temp;

        currentBgmName = newBgmName;
        isFading = false;
    }

    public void StopBGM(bool fade = true)
    {
        if (fade && currentBgmSource.isPlaying)
        {
            StartCoroutine(FadeOutBGM());
        }
        else
        {
            currentBgmSource.Stop();
            currentBgmName = "";
        }
    }

    private IEnumerator FadeOutBGM()
    {
        float fadeTimer = 0f;
        float originalVolume = currentBgmSource.volume;

        while (fadeTimer < bgmFadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float progress = fadeTimer / bgmFadeDuration;
            currentBgmSource.volume = Mathf.Lerp(originalVolume, 0f, progress);
            yield return null;
        }

        currentBgmSource.Stop();
        currentBgmSource.volume = originalVolume;
        currentBgmName = "";
    }

    public void PauseBGM()
    {
        currentBgmSource.Pause();
    }

    public void ResumeBGM()
    {
        currentBgmSource.UnPause();
    }

    public void SetBGMVolume(float volume)
    {
        bgmMasterVolume = Mathf.Clamp01(volume);
        if (currentBgmSource.isPlaying && !isFading)
        {
            string currentBgm = currentBgmName;
            if (!string.IsNullOrEmpty(currentBgm) && bgmDict.ContainsKey(currentBgm))
            {
                currentBgmSource.volume = bgmDict[currentBgm].volume * bgmMasterVolume;
            }
        }
    }

    #endregion

    #region SFX Methods

    public void PlaySFX(string sfxName)
    {
        if (!sfxDict.ContainsKey(sfxName))
        {
            Debug.LogWarning($"SFX '{sfxName}' not found!");
            return;
        }

        AudioClipData sfxData = sfxDict[sfxName];
        AudioSource availableSource = GetAvailableSfxSource();

        if (availableSource != null)
        {
            availableSource.clip = sfxData.clip;
            availableSource.volume = sfxData.volume * sfxMasterVolume;
            availableSource.pitch = sfxData.pitch;
            availableSource.loop = sfxData.loop;
            availableSource.Play();
        }
        else
        {
            Debug.LogWarning("No available SFX audio source!");
        }
    }

    public void PlaySFXOneShot(string sfxName)
    {
        if (!sfxDict.ContainsKey(sfxName))
        {
            Debug.LogWarning($"SFX '{sfxName}' not found!");
            return;
        }

        AudioClipData sfxData = sfxDict[sfxName];
        sfxSource.PlayOneShot(sfxData.clip, sfxData.volume * sfxMasterVolume);
    }

    public void StopAllSFX()
    {
        foreach (var source in sfxSources)
        {
            source.Stop();
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxMasterVolume = Mathf.Clamp01(volume);
    }

    private AudioSource GetAvailableSfxSource()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null; // All sources are busy
    }

    #endregion

    #region Utility Methods

    public bool IsBGMPlaying()
    {
        return currentBgmSource.isPlaying;
    }

    public string GetCurrentBGM()
    {
        return currentBgmName;
    }

    public void SetMasterVolume(float bgmVol, float sfxVol)
    {
        SetBGMVolume(bgmVol);
        SetSFXVolume(sfxVol);
    }

    public void AddBGM(string name, AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = true)
    {
        AudioClipData newBgm = new AudioClipData
        {
            name = name,
            clip = clip,
            volume = volume,
            pitch = pitch,
            loop = loop
        };
        
        bgmClips.Add(newBgm);
        bgmDict[name] = newBgm;
    }

    public void AddSFX(string name, AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = false)
    {
        AudioClipData newSfx = new AudioClipData
        {
            name = name,
            clip = clip,
            volume = volume,
            pitch = pitch,
            loop = loop
        };
        
        sfxClips.Add(newSfx);
        sfxDict[name] = newSfx;
    }

    #endregion

    #region Editor Methods (for testing)
    
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void RefreshDictionaries()
    {
        BuildAudioDictionaries();
    }

    #endregion
}