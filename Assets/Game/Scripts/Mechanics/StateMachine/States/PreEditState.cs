using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Zenject;

public class PreEditState : State
{
    private bool isLevelChoose = false;

    private PreEditScreen _screen;
    private ISaveLoadManager _saveLoadManager;
    private StateMachine _stateMachine;
    private IMusicImporter _musicImporter;
    private IBackgroundImporter _backgroundImporter;
    private IEditLevelChooseController _editLevelChooseController;

    [Inject]
    private void Construct(PreEditScreen preEditScreen, StateMachine stateMachine, ISaveLoadManager saveLoadManager, 
                            IMusicImporter musicImporter, IBackgroundImporter backgroundImporter, IEditLevelChooseController editLevelChooseController)
    {
        _screen = preEditScreen;
        _editLevelChooseController = editLevelChooseController;
        _saveLoadManager = saveLoadManager;
        _musicImporter = musicImporter;
        _backgroundImporter = backgroundImporter;
        _stateMachine = stateMachine;
    }

    public override void Enter()
    {
        _screen.Show();
        
        _editLevelChooseController.isNewLevel = true;

        _screen.startEditButton.onClick.AddListener(() =>
        {
            if(isLevelChoose)
            {
                _editLevelChooseController.SaveCurrentLevel();

                if (_editLevelChooseController.isNewLevel)
                {
                    if(_musicImporter.MusicPath == null || _backgroundImporter.BackgroundPath == null)
                    return;
                }
            }
            else
            {
                if(_musicImporter.MusicPath == null || _backgroundImporter.BackgroundPath == null)
                return;
            }

            _stateMachine.ChangeState<EditModeState>();
        });
        
        _screen.chooseLevelButton.onClick.AddListener(() =>
        {
            _screen.levelChoosePanel.SetActive(true);
            _editLevelChooseController.Initialize();
            isLevelChoose = true;
            
        });
        
        _screen.chooseSongButton.onClick.AddListener(() =>
        {
            _musicImporter.ImportMusic();
        });
        
        _screen.chooseBackgroundButton.onClick.AddListener(() =>
        {
            _backgroundImporter.ImportBackground();
        });
    }

    public override void Exit()
    {
        _screen.startEditButton.onClick.RemoveAllListeners();
        _screen.chooseLevelButton.onClick.RemoveAllListeners();
        _screen.chooseSongButton.onClick.RemoveAllListeners();
        _screen.chooseBackgroundButton.onClick.RemoveAllListeners();
        
        _screen.levelChoosePanel.SetActive(false);

        _screen.Hide();
    }
}
