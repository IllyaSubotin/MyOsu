using System.Collections;
using Zenject;
using UnityEngine;
using System;
using TMPro;

public class ScoreManager : MonoBehaviour, IScoreManager   
{
    public ScoreData scoreData{ get; private set; } = new ScoreData();
    
    public TMP_Text ScoreText;
    public TMP_Text maxComboText;
    public TMP_Text CurComboText;

    public void RegisterHit(HitResult hitResult)
    {
        switch(hitResult)
        {
            case HitResult.Perfect:
                scoreData.score += 300 * scoreData.comboCounter;
                scoreData.perfectHits++;
                scoreData.comboCounter++;
                scoreData.maxCombo = Math.Max(scoreData.maxCombo, scoreData.comboCounter);
                UIUpdate();
                break;

            case HitResult.Good:
                scoreData.score += 100 * scoreData.comboCounter;
                scoreData.goodHits++;
                scoreData.comboCounter++;
                scoreData.maxCombo = Math.Max(scoreData.maxCombo, scoreData.comboCounter);
                UIUpdate();
                break;

            case HitResult.Ok:
                scoreData.score += 50 * scoreData.comboCounter;
                scoreData.okHits++;
                scoreData.comboCounter++;
                scoreData.maxCombo = Math.Max(scoreData.maxCombo, scoreData.comboCounter);
                UIUpdate();
                break;

            case HitResult.Miss:
                scoreData.score += 0;
                scoreData.missedHits++;
                scoreData.comboCounter = 0;
                UIUpdate();
                break;
        }
    }

    private void UIUpdate()
    {
        var text = "Score: " + scoreData.score.ToString("D9");
        ScoreText.text = text;

        text = "Max Combo: " + scoreData.maxCombo.ToString("D5");
        maxComboText.text = text;

        text = "Cur. Combo: " + scoreData.comboCounter.ToString("D5");
        CurComboText.text = text;
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
