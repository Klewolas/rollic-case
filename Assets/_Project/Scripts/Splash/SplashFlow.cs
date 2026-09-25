using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RollicCase.Systems.PlayerData;
using RollicCase.Systems.SceneManagement;
using Zenject;

namespace RollicCase.Splash
{
    /// <summary>Loads the player data while the splash shows for at least its minimum duration, then opens the map.</summary>
    public sealed class SplashFlow : IInitializable, IDisposable
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IPlayerDataService _playerDataService;
        private readonly SceneConfig _sceneConfig;
        private readonly SplashConfig _splashConfig;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        public SplashFlow(ISceneLoader sceneLoader, IPlayerDataService playerDataService, SceneConfig sceneConfig, SplashConfig splashConfig)
        {
            _sceneLoader = sceneLoader;
            _playerDataService = playerDataService;
            _sceneConfig = sceneConfig;
            _splashConfig = splashConfig;
        }

        public void Initialize()
        {
            RunAsync(_cancellation.Token).Forget();
        }

        public void Dispose()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
        }

        private async UniTaskVoid RunAsync(CancellationToken cancellationToken)
        {
            await UniTask.WhenAll(
                UniTask.Delay(TimeSpan.FromSeconds(_splashConfig.MinimumDisplaySeconds), cancellationToken: cancellationToken),
                _playerDataService.LoadAllAsync(cancellationToken));

            await _sceneLoader.LoadAsync(_sceneConfig.MapScene, cancellationToken);
        }
    }
}
