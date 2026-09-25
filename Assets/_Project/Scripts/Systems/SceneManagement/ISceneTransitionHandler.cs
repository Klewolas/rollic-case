using System.Threading;
using Cysharp.Threading.Tasks;

namespace RollicCase.Systems.SceneManagement
{
    /// <summary>Work that must finish before the active scene is replaced.</summary>
    public interface ISceneTransitionHandler
    {
        /// <summary>Runs before the scene loader replaces the active scene.</summary>
        UniTask OnBeforeSceneChangeAsync(CancellationToken cancellationToken);
    }
}
