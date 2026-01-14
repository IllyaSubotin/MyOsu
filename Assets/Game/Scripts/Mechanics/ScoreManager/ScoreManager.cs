using System.Collections;
using Zenject;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour, IScoreManager   
{
    public ScoreData scoreData{ get; private set; } = new ScoreData();

    public void RegisterHit(HitResult hitResult)
    {
        switch(hitResult)
        {
            case HitResult.Perfect:
                scoreData.score += 300 * scoreData.comboCounter;
                scoreData.perfectHits++;
                scoreData.comboCounter++;
                break;

            case HitResult.Good:
                scoreData.score += 100 * scoreData.comboCounter;
                scoreData.goodHits++;
                scoreData.comboCounter++;
                break;

            case HitResult.Ok:
                scoreData.score += 50 * scoreData.comboCounter;
                scoreData.okHits++;
                scoreData.comboCounter++;
                break;

            case HitResult.Miss:
                scoreData.score += 0;
                scoreData.missedHits++;
                scoreData.comboCounter = 0;
                break;
        }
    }

    
    public void ScoreReset()
    {
        scoreData.score = 0;
        scoreData.maxScore = 0;

        scoreData.comboCounter = 0;
        scoreData.maxCombo = 0;

        scoreData.perfectHits = 0;
        scoreData.goodHits = 0;
        scoreData.okHits = 0;
        scoreData.missedHits = 0;
    }

}
