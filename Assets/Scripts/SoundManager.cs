using UnityEngine;

/// Singleton SoundManager for handling background music and sound effects.
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource; // Background music
    public AudioSource sfxSource;   // Sound effects

    [Header("Audio Clips")]
    public AudioClip money;
    public AudioClip chipSound;
    public AudioClip mainTheme;
    public AudioClip rouletteSound;

    private void Awake()
    {
        // Singleton pattern with safety
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Check AudioSources are assigned
        if (musicSource == null || sfxSource == null)
        {
            Debug.LogError("SoundManager: AudioSources not assigned in inspector!");
        }

        // Auto-play main theme
        if (mainTheme != null)
            PlayMusic(mainTheme);
    }

    /// Play background music clip. Will restart only if clip changed.
    public void PlayMusic(AudioClip music)
    {
        if (music == null || musicSource == null)
            return;

        if (musicSource.clip != music)
        {
            musicSource.clip = music;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    /// Play SFX clip (overwrites last SFX). Use PlaySFXOneShot for overlapping sounds.
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.clip = clip;
        sfxSource.Play();
    }

    /// Play SFX clip as one shot (allows overlapping).
    public void PlaySFXOneShot(AudioClip clip)
    {
        if (clip == null || sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    /// Stop all music.
    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }
}
