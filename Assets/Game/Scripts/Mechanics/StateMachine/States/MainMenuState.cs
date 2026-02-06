using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainMenuState : State
{
    private MainMenuScreen _screen;
    private StateMachine _stateMachine;

    [Inject]
    private void Constract(MainMenuScreen screen, StateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _screen = screen;
    } 

    public override void Enter()
    {
        _screen.Show();

        _screen.PlayButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState<LevelChooseState>();
        });
        
        _screen.EditModeButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState<PreEditState>();
        });

        _screen.SettingButton.onClick.AddListener(() =>
        {
            //_stateMachine.ChangeState<SettingState>();
        });
        
        _screen.ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public override void Exit()
    {
        _screen.PlayButton.onClick.RemoveAllListeners();
        _screen.EditModeButton.onClick.RemoveAllListeners();
        _screen.SettingButton.onClick.RemoveAllListeners();
        _screen.ExitButton.onClick.RemoveAllListeners();
        
        _screen.Hide();
    }
}
