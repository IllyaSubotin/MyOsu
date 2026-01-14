using System.Collections;
using System.Collections.Generic;
using Zenject;

public class PreEditState : State
{
    private PreEditScreen _screen;
    private StateMachine _stateMachine;

    [Inject]
    private void Construct(PreEditScreen preEditScreen, StateMachine stateMachine)
    {
        _screen = preEditScreen;
        _stateMachine = stateMachine;
    }

    public override void Enter()
    {
        _screen.startEditButton.onClick.AddListener(() =>
        {
            _stateMachine.ChangeState<EditModeState>();
        });
        
        _screen.chooseLevelButton.onClick.AddListener(() =>
        {
            
        });
        
        _screen.chooseSongButton.onClick.AddListener(() =>
        {
            
        });
        
        _screen.chooseBackgroundButton.onClick.AddListener(() =>
        {
            
        });
    }

    public override void Exit()
    {
        
    }
}
