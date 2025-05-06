using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    public Slider musicSlider, sfxSlider;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        PlayMusic("Music");   
        LoadSoundSettings();
        
        musicSource.mute = true;
    }

    public void LoadSoundSettings()
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        {
            musicSource.volume = data.musicVolume;
            sfxSource.volume = data.sfxVolume;
            musicSource.mute = data.musicMuted;
            sfxSource.mute = data.sfxMuted;

            if (musicSlider) musicSlider.value = data.musicVolume;
            if (sfxSlider) sfxSlider.value = data.sfxVolume;
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = System.Array.Find(musicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = System.Array.Find(sfxSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        sfxSource.PlayOneShot(s.clip);
    }

    public void SetMusicVolume()
    {
        musicSource.volume = musicSlider.value;
    }

    public void SetSFXVolume()
    {
        sfxSource.volume = sfxSlider.value;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    
}

