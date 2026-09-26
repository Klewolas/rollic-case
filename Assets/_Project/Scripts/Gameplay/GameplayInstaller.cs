using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using RollicCase.Gameplay.View;
using RollicCase.Gameplay.View.Blocks;
using RollicCase.Gameplay.View.Board;
using RollicCase.Gameplay.View.Cameras;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay
{
    /// <summary>Binds the gameplay core shared by Game and Game_Sandbox; the scene's own installer binds which LevelData to play.</summary>
    public sealed class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private BoardViewConfig _config;
        [SerializeField] private BlockView _blockPrefab;
        [SerializeField] private DoorView _doorPrefab;

        [Header("Scene References")]
        [Tooltip("The scene camera; assigned on the scene's context prefab instance.")]
        [SerializeField] private Camera _camera;
        [Tooltip("The board in the scene; assigned on the scene's context prefab instance.")]
        [SerializeField] private BoardRootView _board;

        public override void InstallBindings()
        {
            Container.BindInstance(_config);
            Container.BindInstance(_camera);
            Container.BindInstance(_board);

            Container.Bind<BoardFactory>().AsSingle();
            Container.Bind<BoardModel>().FromMethod(CreateBoard).AsSingle();

            Container.Bind<BoardMeshBuilder>().AsSingle();
            Container.Bind<BlockMeshBuilder>().AsSingle();
            Container.BindFactory<BlockView, BlockView.Factory>().FromComponentInNewPrefab(_blockPrefab);
            Container.BindFactory<DoorView, DoorView.Factory>().FromComponentInNewPrefab(_doorPrefab);
            Container.BindInterfacesTo<LevelViewBuilder>().AsSingle();
            Container.BindInterfacesTo<CameraFramer>().AsSingle();
        }

        private static BoardModel CreateBoard(InjectContext context)
        {
            return context.Container.Resolve<BoardFactory>().Create(context.Container.Resolve<LevelData>());
        }
    }
}
