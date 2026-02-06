using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioTimer
{
    event Action<float>  OnSpeedChange;

    float speedMultiplier{ get; }
    float audioTime { get; }
    float audioProgress {get; }

    void SetAudioClip(AudioClip clip);
    void StartTimer();
    void StopTimer();
    void PauseTimer();

    void ResumeTimerForward();
    void ResumeTimerBackwards();

    void FastPlayForward(float speedMultiplier = 2f);
    void FastPlayBackwards(float speedMultiplier = 2f);
}
