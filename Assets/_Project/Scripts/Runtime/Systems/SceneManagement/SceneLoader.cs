using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace RollicCase.Systems.SceneManagement
{
    /// <summary>Loads scenes through the SceneManager and ignores requests while a load is running.</summary>
    public sealed class SceneLoader : ISceneLoader
    {
        private bool _isLoading;

        public async UniTask LoadAsync(string sceneName, CancellationToken cancellationToken)
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;

            try
            {
                await SceneManager.LoadSceneAsync(sceneName).ToUniTask(cancellationToken: cancellationToken);
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
