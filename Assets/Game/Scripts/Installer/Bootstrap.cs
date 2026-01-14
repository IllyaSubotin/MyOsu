using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    private StateMachine _stateMachine;
    private ISaveLoadManager _saveLoadManager;
    
    [Inject]
    private void Contract(StateMachine stateMachine, ISaveLoadManager saveLoadManager)
    {
        _stateMachine = stateMachine;
        _saveLoadManager = saveLoadManager;
    }

    private void Awake()
    {
        _saveLoadManager.LoadGame();
        
        _stateMachine.ChangeState<MainMenuState>();   
    }    
}
