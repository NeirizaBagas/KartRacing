using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    [Header("Audio Sources")]
    public AudioSource bgmSource; // Untuk memutar BGM
    public AudioSource sfxSource; // Untuk memutar SFX
    public AudioSource uiSource;

    [Header("Audio Clips")]
    public AudioClip[] bgmClips; // Daftar BGM
    public AudioClip[] sfxClips; // Daftar SFX
    public AudioClip[] uiClips; 

    private void Awake()
    {
        // Implementasi Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Agar AudioManager tidak dihancurkan saat pindah scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method untuk memutar BGM berdasarkan index
    public void PlayBGM(int index)
    {
        if (index >= 0 && index < bgmClips.Length)
        {
            bgmSource.clip = bgmClips[index];
            bgmSource.Play();
        }
        else
        {
            Debug.LogError("Index BGM tidak valid: " + index);
        }
    }

    // Method untuk memutar SFX berdasarkan index
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
        else
        {
            Debug.LogError("Index SFX tidak valid: " + index);
        }
    }

    public void PlayUI(int index)
    {
        if (index >= 0 && index < uiClips.Length)
        {
            uiSource.PlayOneShot(uiClips[index]);
        }
        else
        {
            Debug.LogError("Index SFX tidak valid: " + index);
        }
    }

    // Method untuk menghentikan BGM
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // Method untuk menghentikan SFX
    public void StopSFX()
    {
        sfxSource.Stop();
    }

    public void StopUI()
    {
        uiSource.Stop();
    }

    // Method untuk mengatur volume BGM
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    // Method untuk mengatur volume SFX
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void SetUIVolume(float volume)
    {
        uiSource.volume = volume;
    }
}