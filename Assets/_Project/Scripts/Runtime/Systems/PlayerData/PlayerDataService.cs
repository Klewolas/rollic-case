using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using RollicCase.Systems.SceneManagement;
using UnityEngine;
using Zenject;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Loads all models and flushes them before scene changes, on focus loss, and on quit.</summary>
    public sealed class PlayerDataService : IPlayerDataService, ISceneTransitionHandler, IInitializable, IDisposable
    {
        private readonly List<IPersistentModel> _models;

        public PlayerDataService(List<IPersistentModel> models)
        {
            _models = models;
        }

        public void Initialize()
        {
            Application.focusChanged += HandleFocusChanged;
            Application.quitting += FlushAll;
        }

        public void Dispose()
        {
            Application.focusChanged -= HandleFocusChanged;
            Application.quitting -= FlushAll;
        }

        public UniTask LoadAllAsync(CancellationToken cancellationToken)
        {
            var tasks = new UniTask[_models.Count];

            for (int i = 0; i < _models.Count; i++)
            {
                tasks[i] = _models[i].LoadAsync(cancellationToken);
            }

            return UniTask.WhenAll(tasks);
        }

        public UniTask OnBeforeSceneChangeAsync(CancellationToken cancellationToken)
        {
            var tasks = new UniTask[_models.Count];

            for (int i = 0; i < _models.Count; i++)
            {
                tasks[i] = _models[i].FlushAsync(cancellationToken);
            }

            return UniTask.WhenAll(tasks);
        }

        private void HandleFocusChanged(bool hasFocus)
        {
            if (!hasFocus)
            {
                FlushAll();
            }
        }

        private void FlushAll()
        {
            for (int i = 0; i < _models.Count; i++)
            {
                _models[i].Flush();
            }
        }
    }
}
