using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RollicCase.Gameplay.Flow.Signals;
using RollicCase.Systems.SceneManagement;
using Zenject;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>Leaves the level on request: restart and next level reload Game, home loads Map.</summary>
    public sealed class LevelNavigation : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ISceneLoader _sceneLoader;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        public LevelNavigation(SignalBus signalBus, ISceneLoader sceneLoader)
        {
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<RestartRequestedSignal>(ReloadGame);
            _signalBus.Subscribe<NextLevelRequestedSignal>(ReloadGame);
            _signalBus.Subscribe<HomeRequestedSignal>(LoadHome);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<RestartRequestedSignal>(ReloadGame);
            _signalBus.Unsubscribe<NextLevelRequestedSignal>(ReloadGame);
            _signalBus.Unsubscribe<HomeRequestedSignal>(LoadHome);
            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private void ReloadGame()
        {
            _sceneLoader.LoadAsync(SceneNames.Game, _cancellation.Token).Forget();
        }

        private void LoadHome()
        {
            _sceneLoader.LoadAsync(SceneNames.Map, _cancellation.Token).Forget();
        }
    }
}
