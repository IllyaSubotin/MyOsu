using System.Collections;
using Zenject;
using UnityEngine;

public class LevelChooseState : State
{
    private ILevelChooseController _levelChooseController;
    private LevelChooseScreen _screen;
    private StateMachine _stateMachine;
    private ISaveLoadManager _saveLoadManager;

    [Inject]
    private void Constract(StateMachine stateMachine, ILevelChooseController levelChooseController, 
                            LevelChooseScreen levelChooseScreen, ISaveLoadManager saveLoadManager)
    {
        _stateMachine = stateMachine;
        _screen = levelChooseScreen;
        _levelChooseController = levelChooseController;
        _saveLoadManager = saveLoadManager;
    } 

    public override void Enter()
    {
        _screen.Show();

        _screen.StartButton.onClick.AddListener(() =>
        {
            _levelChooseController.SaveCurrentLevel();
            _stateMachine.ChangeState<GameplayState>(); 
        });
        
        _levelChooseController.Initialize();
    }

    public override void Exit()
    {
        _screen.StartButton.onClick.RemoveAllListeners();

        _screen.Hide();
    }
}
