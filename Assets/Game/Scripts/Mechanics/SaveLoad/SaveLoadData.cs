using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveLoadData
{
    public List<BeatmapData> beatmapDatas;
    public List<MapStatistics> mapStatisticsDatas;  
}

[Serializable]
public class BeatmapData
{
    public int beatmapID;
    public string beatmapName;
    public List<NodeData> nodeInfos;
    public string audioPath;
    public string backgroundImagePath;
}

[Serializable]
public class NodeData
{
    public float xPercent;
    public float yPercent;

    public float spawntime;
    public float approachTime;
    public float size;
}

[Serializable]
public class MapStatistics
{    
    public int beatmapID;

    public int highScore;
    public int maxCombo;
    public float bestAccuracy;

    public int missedHits;
    public int okHits;
    public int goodHits;
    public int perfectHits;
}