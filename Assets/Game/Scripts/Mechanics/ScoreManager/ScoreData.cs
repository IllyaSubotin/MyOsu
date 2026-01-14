using System;

[Serializable]
public class ScoreData
{
    public int maxScore;
    public int score;

    public int maxCombo;
    public int comboCounter;

    public int perfectHits;
    public int goodHits;
    public int okHits;
    public int missedHits;
}