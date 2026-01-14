using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScoreManager
{
    ScoreData scoreData { get; }
    void RegisterHit(HitResult hitResult);
    void ScoreReset();
}
