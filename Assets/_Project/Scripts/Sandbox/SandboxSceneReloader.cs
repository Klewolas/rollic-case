using System;
using RollicCase.Gameplay.Flow.Signals;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Zenject;

namespace RollicCase.Sandbox
{
    /// <summary>Reloads Game_Sandbox on restart; the scene is not in the build, so it is loaded by path in Play mode.</summary>
    public sealed class SandboxSceneReloader : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;

        public SandboxSceneReloader(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<RestartRequestedSignal>(Reload);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<RestartRequestedSignal>(Reload);
        }

        private void Reload()
        {
            EditorSceneManager.LoadSceneInPlayMode(SceneManager.GetActiveScene().path, new LoadSceneParameters(LoadSceneMode.Single));
        }
    }
}
