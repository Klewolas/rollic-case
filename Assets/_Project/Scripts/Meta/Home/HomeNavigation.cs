using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RollicCase.Systems.SceneManagement;
using Zenject;

namespace RollicCase.Meta.Home
{
    /// <summary>Loads the Game scene when the player asks to play.</summary>
    public sealed class HomeNavigation : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ISceneLoader _sceneLoader;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        public HomeNavigation(SignalBus signalBus, ISceneLoader sceneLoader)
        {
            _signalBus = signalBus;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PlayRequestedSignal>(LoadGame);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PlayRequestedSignal>(LoadGame);
            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private void LoadGame()
        {
            _sceneLoader.LoadAsync(SceneNames.Game, _cancellation.Token).Forget();
        }
    }
}
