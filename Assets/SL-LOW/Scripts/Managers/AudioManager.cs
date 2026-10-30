using UnityEngine;

public class AudioManager : MonoBehaviour
{
    internal static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _musicEnabled = PlayerPrefs.GetInt(PrefKeyMusic, 1) == 1;
        _sfxEnabled   = PlayerPrefs.GetInt(PrefKeysfx,   1) == 1;
        _musicVolume  = PlayerPrefs.GetFloat(PrefKeyMusicVol, 0.5f);
        _sfxVolume    = PlayerPrefs.GetFloat(PrefKeySfxVol,   1.0f);

        ApplyMusicVolume();
        ApplySfxVolume();
    }

    private const string PrefKeyMusic    = "audio_music_enabled";
    private const string PrefKeysfx      = "audio_sfx_enabled";
    private const string PrefKeyMusicVol = "audio_music_volume";
    private const string PrefKeySfxVol   = "audio_sfx_volume";

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgMusicSource;
    [SerializeField] private AudioSource uiSource;
    [SerializeField] private AudioSource wheelSegmentSource;
    [SerializeField] private AudioSource reserveSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip clipGameMainBg;
    [SerializeField] private AudioClip clipBetPlusMinus;
    [SerializeField] private AudioClip clipMaxBetReached;
    [SerializeField] private AudioClip clipPrimaryActionButton;
    [SerializeField] private AudioClip clipGeneralButtonClick;
    [SerializeField] private AudioClip clipPopupOpenClose;
    [SerializeField] private AudioClip clipAutoplayPanelOpen;
    [SerializeField] private AudioClip clipFreeSpinBg;
    [SerializeField] private AudioClip clipSpinningReel;
    [SerializeField] private AudioClip clipNormalWin;
    [SerializeField] private AudioClip clipReelStop;

    private bool _musicEnabled = true;
    private bool _sfxEnabled   = true;
    private float _musicVolume = 0.5f;
    private float _sfxVolume   = 1.0f;

    internal bool MusicEnabled => _musicEnabled;
    internal bool SfxEnabled   => _sfxEnabled;
    internal float MusicVolume => _musicVolume;
    internal float SfxVolume   => _sfxVolume;

    internal void SetMusicEnabled(bool on)
    {
        _musicEnabled = on;
        PlayerPrefs.SetInt(PrefKeyMusic, on ? 1 : 0);
        PlayerPrefs.Save();
        ApplyMusicVolume();
    }

    internal void SetSfxEnabled(bool on)
    {
        _sfxEnabled = on;
        PlayerPrefs.SetInt(PrefKeysfx, on ? 1 : 0);
        PlayerPrefs.Save();
        ApplySfxVolume();
    }

    internal void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(PrefKeyMusicVol, _musicVolume);
        PlayerPrefs.Save();
        ApplyMusicVolume();
    }

    internal void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(PrefKeySfxVol, _sfxVolume);
        PlayerPrefs.Save();
        ApplySfxVolume();
    }

    private void ApplyMusicVolume()
    {
        if (bgMusicSource == null) return;
        bgMusicSource.volume = _musicEnabled ? _musicVolume : 0f;
    }

    private void ApplySfxVolume()
    {
        float v = _sfxEnabled ? _sfxVolume : 0f;
        if (uiSource           != null) uiSource.volume           = v;
        if (wheelSegmentSource != null) wheelSegmentSource.volume = v;
        if (reserveSource      != null) reserveSource.volume      = v;
    }

    /// <summary>
    /// Uses UI source (AudioSource 2). If busy/playing, falls back to reserve source (AudioSource 4).
    /// </summary>
    private void PlayUISound(AudioClip clip)
    {
        if (!_sfxEnabled || clip == null) return;

        if (uiSource != null && !uiSource.isPlaying)
        {
            uiSource.PlayOneShot(clip);
        }
        else if (reserveSource != null)
        {
            reserveSource.PlayOneShot(clip);
        }
        else if (uiSource != null)
        {
            uiSource.PlayOneShot(clip);
        }
    }

    private void PlayLoop(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null) return;
        source.clip   = clip;
        source.loop   = true;
        source.volume = _musicEnabled ? _musicVolume : 0f;
        source.Play();
    }

    private void StopSource(AudioSource source)
    {
        if (source == null) return;
        source.Stop();
        source.loop = false;
    }

    // 1. Game Main BG
    internal void PlayBgMusic()
    {
        if (bgMusicSource == null || clipGameMainBg == null) return;
        if (bgMusicSource.isPlaying && bgMusicSource.clip == clipGameMainBg) return;

        bgMusicSource.clip   = clipGameMainBg;
        bgMusicSource.loop   = true;
        bgMusicSource.volume = _musicEnabled ? _musicVolume : 0f;
        bgMusicSource.Play();
    }

    internal void PlayMainBg() => PlayBgMusic();

    internal void StopBgMusic()
    {
        StopSource(bgMusicSource);
    }

    // 2. Bet Plus / Bet Minus (one for both)
    internal void PlayBetPlusMinus()
    {
        PlayUISound(clipBetPlusMinus);
    }

    internal void PlayBetPlus()  => PlayBetPlusMinus();
    internal void PlayBetMinus() => PlayBetPlusMinus();

    // 3. Max Bet Reached
    internal void PlayMaxBetReached()
    {
        PlayUISound(clipMaxBetReached);
    }

    // 4. Spin / Stop / Take / AutoplayStop / WheelStart Btn Sound
    internal void PlayPrimaryActionButton()
    {
        PlayUISound(clipPrimaryActionButton != null ? clipPrimaryActionButton : clipGeneralButtonClick);
    }

    internal void PlaySpinStart()    => PlayPrimaryActionButton();
    internal void PlaySpinStop()     => PlayPrimaryActionButton();
    internal void PlayTakeButton()   => PlayPrimaryActionButton();
    internal void PlayAutoplayStop() => PlayPrimaryActionButton();
    internal void PlayWheelStart()   => PlayPrimaryActionButton();

    // 5. General Button Click
    internal void PlayButton()
    {
        PlayUISound(clipGeneralButtonClick);
    }

    internal void PlayGeneralButtonClick() => PlayButton();

    // 6. Popup Open Close Sound
    internal void PlayPopupOpenClose()
    {
        PlayUISound(clipPopupOpenClose != null ? clipPopupOpenClose : clipGeneralButtonClick);
    }

    internal void PlayPopupClose() => PlayPopupOpenClose();
    internal void PlayPopupOpen()  => PlayPopupOpenClose();

    // 7. Autoplay Panel Open Sound
    internal void PlayAutoplayPanelOpen()
    {
        PlayUISound(clipAutoplayPanelOpen != null ? clipAutoplayPanelOpen : clipPopupOpenClose);
    }

    // 8. FreeSpin BG (loop while free spin)
    internal void PlayFreeSpinBg()
    {
        if (clipFreeSpinBg == null) return;
        PlayLoop(bgMusicSource, clipFreeSpinBg);
    }

    // 9. Spinning Reel Loop
    internal void PlaySpinningReel()
    {
        if (!_sfxEnabled || wheelSegmentSource == null || clipSpinningReel == null) return;
        if (wheelSegmentSource.isPlaying && wheelSegmentSource.clip == clipSpinningReel) return;

        wheelSegmentSource.clip = clipSpinningReel;
        wheelSegmentSource.loop = true;
        wheelSegmentSource.volume = _sfxVolume;
        wheelSegmentSource.Play();
    }

    internal void StopSpinningReel()
    {
        if (wheelSegmentSource != null && wheelSegmentSource.clip == clipSpinningReel)
        {
            StopSource(wheelSegmentSource);
        }
    }

    // 10. Normal Win
    internal void PlayNormalWin()
    {
        PlayUISound(clipNormalWin);
    }

    // 11. Slot Reel Column Stop Sound
    internal void PlayReelStop()
    {
        if (!_sfxEnabled || clipReelStop == null) return;

        if (wheelSegmentSource != null)
            wheelSegmentSource.PlayOneShot(clipReelStop);
        else
            PlayUISound(clipReelStop);
    }

    private bool isForceMuted = false;

    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        AudioListener.volume = forceMute ? 0f : 1f;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        SetMuteAll(!hasFocus);
    }

    private void OnApplicationPause(bool isPaused)
    {
        SetMuteAll(isPaused);
    }
}
