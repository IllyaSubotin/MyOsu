using Zenject;
using System.Collections.Generic;
using UnityEngine;

public class GameplayState : State
{
    private GameplayScreen _screen;
    private StateMachine _stateMachine;
    private IHealthManager _healthManager;
    private INodeManager _nodeManager;
    private IAudioTimer _audioTimer;
    private IScoreManager _scoreManager;
    private ISaveLoadManager _saveLoadManager;
    
    [Inject]
    private void Constract(GameplayScreen screen, StateMachine stateMachine, IHealthManager healthManager, 
                            INodeManager nodeManager, IAudioTimer audioTimer, IScoreManager scoreManager, ISaveLoadManager saveLoadManager)
    {
        _screen = screen;
        _stateMachine = stateMachine;
        _healthManager = healthManager;
        _nodeManager = nodeManager;
        _audioTimer = audioTimer;
        _scoreManager = scoreManager;
        _saveLoadManager = saveLoadManager;
    } 

    public override void Enter()
    {
        _screen.Show();
        _screen.GameplayCanvas.SetActive(true);

        _audioTimer.StartTimer();
        _scoreManager.ScoreReset();

        _healthManager.HealthReset();
        _healthManager.OnHealthEnded = () =>
        {
            _stateMachine.ChangeState<MainMenuState>();
        };

        _nodeManager.Initialize(_saveLoadManager.beatmapDatas[_saveLoadManager.currentLevelIndex].nodeInfos);
        _nodeManager.onEnded = () =>
        {
            _stateMachine.ChangeState<MainMenuState>();
        };
    }



    public override void Exit()
    {
        _healthManager.OnHealthEnded = null;
        _nodeManager.onEnded = null;
        
        _audioTimer.StopTimer();

        _screen.GameplayCanvas.SetActive(false);
        _screen.Hide();
    }
}
