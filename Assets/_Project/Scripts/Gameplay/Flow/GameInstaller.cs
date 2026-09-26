using RollicCase.Gameplay.Data;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>Binds the production game flow: the level to play comes from the catalog and the player's progress.</summary>
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private LevelCatalog _catalog;

        public override void InstallBindings()
        {
            Container.BindInstance(_catalog);
            Container.Bind<ILevelProvider>().To<LevelProvider>().AsSingle();
            Container.Bind<LevelData>().FromResolveGetter<ILevelProvider>(provider => provider.CurrentLevel).AsSingle();
        }
    }
}
