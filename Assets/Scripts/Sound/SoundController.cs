using UnityEngine;

public class SoundController : SingletonDontDestroy<SoundController>
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    private void Start()
    {
        OnMusicChange();
        OnSfxChange();
        Observer.MusicChanged += OnMusicChange;
        Observer.SoundChanged += OnSfxChange;
    }


    public void OnMusicChange()
    {
        musicAudioSource.mute = !Data.MusicBgState;
    }

    public void OnSfxChange()
    {
        sfxAudioSource.mute = !Data.SfxState;
    }

    public void PlaySfx(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            sfxAudioSource.PlayOneShot(audioClip);
        }
        else
        {
            Debug.Log("Can't found audio clip");
        }
    }

    public void PlayMusicBackground(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            musicAudioSource.clip = audioClip;
            musicAudioSource.Play();
        }
        else
        {
            Debug.Log("Can't found audio clip");
        }
    }

    public void PauseMusicBackground()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.Pause();
        }
    }

    public void StopMusicBackground()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
    }

    public void StopSfx()
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.Stop();
        }
    }

    public void PauseSfx()
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.Pause();
        }
    }
}