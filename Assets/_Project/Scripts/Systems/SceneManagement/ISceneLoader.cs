using System.Threading;
using Cysharp.Threading.Tasks;

namespace RollicCase.Systems.SceneManagement
{
    /// <summary>Loads scenes asynchronously.</summary>
    public interface ISceneLoader
    {
        /// <summary>Replaces the active scene with the given scene.</summary>
        UniTask LoadAsync(string sceneName, CancellationToken cancellationToken);
    }
}
