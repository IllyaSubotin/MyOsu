using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditModeState : State
{
    private EditModeScreen _screen;
    private StateMachine _stateMachine;
    private ISaveLoadManager _saveLoadManager;
    private IEditModeController _editModeController;
    private IAudioTimer _audioTimer;

    public EditModeState(EditModeScreen screen, StateMachine stateMachine, ISaveLoadManager saveLoadManager, 
                            IEditModeController editModeController, IAudioTimer audioTimer)
    {
        _screen = screen;
        _stateMachine = stateMachine;
        _saveLoadManager = saveLoadManager;
        _editModeController = editModeController;
        _audioTimer = audioTimer;
    }

    public override void Enter()
    {
        _screen.Show();
        _audioTimer.StartTimer();

        _screen.stopButton.onClick.AddListener(() =>{ _audioTimer.PauseTimer();});
        _screen.playButton.onClick.AddListener(() =>{ _audioTimer.ResumeTimerForward();});
        _screen.playBackButton.onClick.AddListener(() =>{ _audioTimer.ResumeTimerBackwards();});
        _screen.playFastButton.onClick.AddListener(() =>{ _audioTimer.FastPlayForward(2f);});
        _screen.playBackFastButton.onClick.AddListener(() =>{ _audioTimer.FastPlayBackwards(2f);});

        _screen.saveButton.onClick.AddListener(() =>
        {
            BeatmapData beatmapData = _editModeController.beatmapDatas;
            _saveLoadManager.beatmapDatas.Add(new BeatmapData()
            {
                beatmapID = beatmapData.beatmapID,
                beatmapName = beatmapData.beatmapName,
                nodeInfos = new List<NodeData>(beatmapData.nodeInfos)
            });
            _saveLoadManager.mapStatisticsDatas.Add(new MapStatistics());
            _saveLoadManager.SaveGame();
        });

        _screen.exitButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState<MainMenuState>();
        });
        
    }

    public override void Exit()
    {
        _screen.stopButton.onClick.RemoveAllListeners();
        _screen.playButton.onClick.RemoveAllListeners();
        _screen.playFastButton.onClick.RemoveAllListeners();
        _screen.playBackButton.onClick.RemoveAllListeners();
        _screen.playBackFastButton.onClick.RemoveAllListeners();

        _screen.saveButton.onClick.RemoveAllListeners();
        //_screen.loadButton.onClick.RemoveAllListeners();
        _screen.exitButton.onClick.RemoveAllListeners();

        _editModeController.ClearAllNodes();
        _audioTimer.StopTimer();
        _screen.Hide();
    }
}
