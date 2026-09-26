using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Flow.Signals;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>Binds the production game flow: the level from the player's progress, the reward, the pause menu, the result popups, and navigation.</summary>
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private LevelCatalog _catalog;
        [SerializeField] private LevelRewardConfig _reward;

        public override void InstallBindings()
        {
            Container.BindInstance(_catalog);
            Container.BindInstance(_reward);
            Container.Bind<ILevelProvider>().To<LevelProvider>().AsSingle();
            Container.Bind<LevelData>().FromResolveGetter<ILevelProvider>(provider => provider.CurrentLevel).AsSingle();

            Container.DeclareSignal<PauseRequestedSignal>();
            Container.DeclareSignal<ResumeRequestedSignal>();
            Container.DeclareSignal<RestartRequestedSignal>();
            Container.DeclareSignal<HomeRequestedSignal>();
            Container.DeclareSignal<NextLevelRequestedSignal>();

            Container.Bind<LevelCompletion>().AsSingle();
            Container.BindInterfacesTo<LevelResultPresenter>().AsSingle();
            Container.BindInterfacesTo<PausePresenter>().AsSingle();
            Container.BindInterfacesTo<LevelNavigation>().AsSingle();
        }
    }
}
