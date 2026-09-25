using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace RollicCase.Systems.SceneManagement
{
    /// <summary>Runs the transition handlers, then loads the scene; requests during a load are ignored.</summary>
    public sealed class SceneLoader : ISceneLoader
    {
        private readonly List<ISceneTransitionHandler> _transitionHandlers;
        private bool _isLoading;

        public SceneLoader(List<ISceneTransitionHandler> transitionHandlers)
        {
            _transitionHandlers = transitionHandlers;
        }

        public async UniTask LoadAsync(string sceneName, CancellationToken cancellationToken)
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;

            try
            {
                for (int i = 0; i < _transitionHandlers.Count; i++)
                {
                    await _transitionHandlers[i].OnBeforeSceneChangeAsync(cancellationToken);
                }

                await SceneManager.LoadSceneAsync(sceneName).ToUniTask(cancellationToken: cancellationToken);
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
