using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public string currentMusic;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlaySceneMusic(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    private void PlaySceneMusic(string sceneName)
    {
        if (sceneName == "MainMenu")
        {
            PlayMusic("TitleScreenMusic");
        }
        else if (sceneName == "MainGame")
        {
            PlayMusic("BackgroundMusic");
        }
        else
        {
            musicSource.Stop();
        }
    }

    public void PlayMusic(string name)
    {
        if (currentMusic == name)
            return;

        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }

        currentMusic = name;
        musicSource.clip = s.clip;
        musicSource.Play();

    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public IEnumerator FadeToMusic(string name, float fadeTime)
    {
        float originalVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= Time.deltaTime / fadeTime;
            yield return null;
        }

        PlayMusic(name);

        while (musicSource.volume < 1)
        {
            musicSource.volume += Time.deltaTime / fadeTime;
            yield return null;
        }

        musicSource.volume = originalVolume;
    }

}