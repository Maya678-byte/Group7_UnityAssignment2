using UnityEngine;
using UnityEngine.UI;

public class AudioSingleton : MonoBehaviour
{
    public static AudioSingleton Instance { get; private set; }

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.5f;
    [Range(0f, 1f)] public float sfxVolume = 0.5f;

    private const string MusicKey = "music_volume";
    private const string SfxKey = "sfx_volume";

    [Header("SFX Sources")]
    public AudioSource jump;
    public AudioSource pause;
    public AudioSource unpause;
    public AudioSource button;
    public AudioSource goal;
    public AudioSource gameover;
    public AudioSource smash;

    [Header("Music Sources")]
    public AudioSource menu;
    public AudioSource normal;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved volumes
        musicVolume = PlayerPrefs.GetFloat(MusicKey, 0.5f);
        sfxVolume = PlayerPrefs.GetFloat(SfxKey, 0.5f);

        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        // SFX
        if (jump) jump.volume = sfxVolume;
        if (pause) pause.volume = sfxVolume;
        if (unpause) unpause.volume = sfxVolume;
        if (button) button.volume = sfxVolume;
        if (goal) goal.volume = sfxVolume;
        if (gameover) gameover.volume = sfxVolume;
        if (smash) smash.volume = sfxVolume;

        // Music
        if (menu) menu.volume = musicVolume;
        if (normal) normal.volume = musicVolume;
    }

    // Backwards compatible: if your UI still calls ChangeVolume, it will control BOTH.
    public void ChangeVolume(Slider slider)
    {
        float v = slider.value;
        musicVolume = v;
        sfxVolume = v;

        PlayerPrefs.SetFloat(MusicKey, musicVolume);
        PlayerPrefs.SetFloat(SfxKey, sfxVolume);
        PlayerPrefs.Save();

        ApplyVolumes();
    }

    public void ChangeMusicVolume(Slider slider)
    {
        musicVolume = slider.value;
        PlayerPrefs.SetFloat(MusicKey, musicVolume);
        PlayerPrefs.Save();

        // Only update music sources
        if (menu) menu.volume = musicVolume;
        if (normal) normal.volume = musicVolume;
    }

    public void ChangeSFXVolume(Slider slider)
    {
        sfxVolume = slider.value;
        PlayerPrefs.SetFloat(SfxKey, sfxVolume);
        PlayerPrefs.Save();

        // Only update SFX sources
        if (jump) jump.volume = sfxVolume;
        if (pause) pause.volume = sfxVolume;
        if (unpause) unpause.volume = sfxVolume;
        if (button) button.volume = sfxVolume;
        if (goal) goal.volume = sfxVolume;
        if (gameover) gameover.volume = sfxVolume;
        if (smash) smash.volume = sfxVolume;
    }

    // --- Play methods (SFX use sfxVolume, Music use musicVolume) ---

    public void PlayJump()
    {
        if (!jump) return;
        jump.volume = sfxVolume;
        jump.Play();
    }

    public void PlayPause()
    {
        if (!pause) return;
        pause.volume = sfxVolume;
        pause.Play();
    }

    public void PlayUnPause()
    {
        if (!unpause) return;
        unpause.volume = sfxVolume;
        unpause.Play();
    }

    public void PlayButton()
    {
        if (!button) return;
        button.volume = sfxVolume;
        button.Play();
    }

    public void PlayMenu()
    {
        PauseMusic();
        if (!menu) return;
        menu.volume = musicVolume;
        menu.loop = true;
        menu.Play();
    }

    public void PlayNormal()
    {
        PauseMusic();
        if (!normal) return;
        normal.volume = musicVolume;
        normal.loop = true;
        normal.Play();
    }

    public void PlayGoal()
    {
        if (!goal) return;
        goal.volume = sfxVolume;
        goal.Play();
    }

    public void PlayGameOver()
    {
        if (!gameover) return;
        gameover.volume = sfxVolume;
        gameover.Play();
    }

    public void PlaySmash()
    {
        if (!smash) return;
        smash.volume = sfxVolume;
        smash.Play();
    }

    public void PauseMusic()
    {
        if (normal) normal.Stop();
        if (menu) menu.Stop();
    }

    // Optional helpers (nice if you want to set from code without Sliders)
    public void SetMusicVolume(float value01)
    {
        musicVolume = Mathf.Clamp01(value01);
        PlayerPrefs.SetFloat(MusicKey, musicVolume);
        PlayerPrefs.Save();
        if (menu) menu.volume = musicVolume;
        if (normal) normal.volume = musicVolume;
    }

    public void SetSFXVolume(float value01)
    {
        sfxVolume = Mathf.Clamp01(value01);
        PlayerPrefs.SetFloat(SfxKey, sfxVolume);
        PlayerPrefs.Save();
        if (jump) jump.volume = sfxVolume;
        if (pause) pause.volume = sfxVolume;
        if (unpause) unpause.volume = sfxVolume;
        if (button) button.volume = sfxVolume;
        if (goal) goal.volume = sfxVolume;
        if (gameover) gameover.volume = sfxVolume;
        if (smash) smash.volume = sfxVolume;
    }
}