using System.Threading;
using Cysharp.Threading.Tasks;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Loading and flushing of one stored model.</summary>
    public interface IPersistentModel
    {
        /// <summary>Loads the model if it is not loaded yet.</summary>
        UniTask LoadAsync(CancellationToken cancellationToken);

        /// <summary>Writes pending changes without blocking the main thread.</summary>
        UniTask FlushAsync(CancellationToken cancellationToken);

        /// <summary>Writes pending changes immediately.</summary>
        void Flush();
    }
}
