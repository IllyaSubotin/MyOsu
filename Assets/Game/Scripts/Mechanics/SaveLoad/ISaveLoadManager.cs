using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadManager 
{
    List<BeatmapData> beatmapDatas { get; set; }
    List<MapStatistics> mapStatisticsDatas { get; set; }
    int currentLevelIndex{ get; set; }

    void SaveGame();
    void LoadGame();
}
