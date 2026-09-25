using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RollicCase.Systems.SceneManagement;
using Zenject;

namespace RollicCase.Meta.Splash
{
    /// <summary>Shows the splash for its minimum duration, then opens the map.</summary>
    public sealed class SplashFlow : IInitializable, IDisposable
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly SceneConfig _sceneConfig;
        private readonly SplashConfig _splashConfig;
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        public SplashFlow(ISceneLoader sceneLoader, SceneConfig sceneConfig, SplashConfig splashConfig)
        {
            _sceneLoader = sceneLoader;
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
            await UniTask.Delay(TimeSpan.FromSeconds(_splashConfig.MinimumDisplaySeconds), cancellationToken: cancellationToken);
            await _sceneLoader.LoadAsync(_sceneConfig.MapScene, cancellationToken);
        }
    }
}
