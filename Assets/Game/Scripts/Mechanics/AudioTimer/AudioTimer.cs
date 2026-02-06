using System;
using Zenject;
using UnityEngine;
using UnityEngine.UI;

public class AudioTimer : MonoBehaviour , IAudioTimer
{
    public event Action<float> OnSpeedChange;
    public float audioTime { get; private set; }
    public float audioProgress {get; private set; }
    public float speedMultiplier {get; private set; } = 1f;

    public AudioSource audioSource;
    public Slider audioProgressBar;

    private bool isPlaying = false;
    private bool isStarted = false;


    private void Update()
    {
        if (isStarted == false) return;

        if (isPlaying)
        {
            audioTime += Time.deltaTime * speedMultiplier;
            
            audioProgress = audioTime / audioSource.clip.length;
            audioProgressBar.value = audioProgress;
            
            if (audioTime < 0f || audioTime > audioSource.clip.length) PauseTimer();;
        }
    }
    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void StartTimer()
    {
        isStarted = true;
        isPlaying = true;

        audioTime = 0f;
        speedMultiplier = 1f;

        audioSource.Play();
    }
    
    public void StopTimer()
    {
        isStarted = false;
        isPlaying = false;

        audioTime = 0f;
        audioProgress = audioTime / audioSource.clip.length;
        audioProgressBar.value = audioProgress;

        audioSource.Stop();
    }

    public void PauseTimer()
    {
        isPlaying = false;

        audioSource.Pause();
    }

    public void ResumeTimerForward()
    {
        isPlaying = true;

        speedMultiplier = 1f;
        OnSpeedChange?.Invoke(speedMultiplier);
        
        audioSource.UnPause();
    }

    public void ResumeTimerBackwards()
    {
        isPlaying = true;

        speedMultiplier = -1f;
        OnSpeedChange?.Invoke(speedMultiplier);

        audioSource.UnPause();
    }

    public void FastPlayForward(float speedMultiplier = 2f)
    {
        isPlaying = true;

        this.speedMultiplier = speedMultiplier;
        OnSpeedChange?.Invoke(this.speedMultiplier);

        audioSource.UnPause();
    }

    public void FastPlayBackwards(float speedMultiplier = 2f)
    {
        isPlaying = true;

        this.speedMultiplier = -speedMultiplier;
        OnSpeedChange?.Invoke(this.speedMultiplier);

        audioSource.UnPause();
    }



}
