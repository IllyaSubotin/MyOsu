using System.Collections;
using Zenject;
using UnityEngine;
using System.Collections.Generic;

public class SaveLoadManager : ISaveLoadManager
{
    public List<BeatmapData> beatmapDatas{ get; set; } = new List<BeatmapData>();
    public List<MapStatistics> mapStatisticsDatas{ get; set; } = new List<MapStatistics>();
    public int currentLevelIndex{get; set;}

    private SaveLoadData _saveLoadData = new SaveLoadData();

    private ISaveLoadService _saveLoadService;

    [Inject]
    private void Construct(ISaveLoadService saveLoadService)
    {
        _saveLoadService = saveLoadService;
    }

    public void SaveGame()
    {
        _saveLoadData.beatmapDatas = this.beatmapDatas;
        _saveLoadData.mapStatisticsDatas = this.mapStatisticsDatas;

        _saveLoadService.Save(_saveLoadData);
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        _saveLoadData = _saveLoadService.Load();

        this.beatmapDatas = _saveLoadData.beatmapDatas;
        this.mapStatisticsDatas = _saveLoadData.mapStatisticsDatas;

        if (this.beatmapDatas == null)
            this.beatmapDatas = new List<BeatmapData>();

        if (this.mapStatisticsDatas == null)
            this.mapStatisticsDatas = new List<MapStatistics>();
        Debug.Log("Game Loaded");
    }
}
