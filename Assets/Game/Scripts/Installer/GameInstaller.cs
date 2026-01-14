using Zenject;

public class GameInstaller : MonoInstaller
{
    public Bootstrap bootstrap;

    public MainMenuScreen mainMenuScreen;
    public LevelChooseScreen levelChooseScreen;
    public GameplayScreen gameplayScreen;
    public EditModeScreen editModeScreen;
    public PreEditScreen preEditScreen;

    public override void InstallBindings()
    {
        Container.Bind<StateMachine>().AsSingle();
        Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
        Container.Bind<ISaveLoadManager>().To<SaveLoadManager>().AsSingle();
        
        Container.Bind<IAudioTimer>().To<AudioTimer>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IScoreManager>().To<ScoreManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IHealthManager>().To<HealthManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<INodeManager>().To<NodeManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IHitDetection>().To<HitDetection>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IEditModeController>().To<EditModeController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ILevelChooseController>().To<LevelChooseController>().FromComponentInHierarchy().AsSingle();

        Container.BindInstance(mainMenuScreen).AsSingle();
        Container.BindInstance(levelChooseScreen).AsSingle();
        Container.BindInstance(gameplayScreen).AsSingle();
        Container.BindInstance(editModeScreen).AsSingle();
        Container.BindInstance(preEditScreen).AsSingle();

        Container.Bind<MainMenuState>().AsSingle();
        Container.Bind<LevelChooseState>().AsSingle();
        Container.Bind<GameplayState>().AsSingle();
        Container.Bind<EditModeState>().AsSingle();
        Container.Bind<PreEditState>().AsSingle();

        Container.Bind<GameplayController>().FromComponentInHierarchy().AsSingle();

        Container.BindInstance(bootstrap).AsSingle();

    }
}