using Zenject;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GameplayState : State
{
    private GameplayScreen _screen;
    private StateMachine _stateMachine;
    private IHealthManager _healthManager;
    private INodeManager _nodeManager;
    private IAudioTimer _audioTimer;
    private IScoreManager _scoreManager;
    private ISaveLoadManager _saveLoadManager;
    private IBackgroundImporter _backgroundImporter;
    private IMusicImporter _musicImporter;
    
    [Inject]
    private void Construct(GameplayScreen screen, StateMachine stateMachine, IHealthManager healthManager, 
                            INodeManager nodeManager, IAudioTimer audioTimer, IScoreManager scoreManager, 
                            ISaveLoadManager saveLoadManager, IMusicImporter musicImporter, IBackgroundImporter backgroundImporter)
    {
        _screen = screen;
        _stateMachine = stateMachine;
        _healthManager = healthManager;
        _backgroundImporter = backgroundImporter;
        _nodeManager = nodeManager;
        _audioTimer = audioTimer;
        _scoreManager = scoreManager;
        _musicImporter = musicImporter;
        _saveLoadManager = saveLoadManager;
    } 

    public override void Enter()
    {
        _screen.Show();
        _screen.GameplayCanvas.SetActive(true);
        
        _ = LoadAudioClip(_saveLoadManager.beatmapDatas[_saveLoadManager.currentLevelIndex].audioPath);
        _screen.backgroundImage.sprite = _backgroundImporter.LoadBackground(_saveLoadManager.beatmapDatas[_saveLoadManager.currentLevelIndex].backgroundImagePath);

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

    private async Task LoadAudioClip(string path)
    {
        AudioClip clip = await _musicImporter.LoadAsync(path);
        _audioTimer.SetAudioClip(clip);

        _audioTimer.StartTimer();
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
