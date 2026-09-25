using System.Threading;
using Cysharp.Threading.Tasks;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Decides when a pending save runs, so that changes made close together share one write.</summary>
    public interface ISaveScheduler
    {
        /// <summary>Completes when the pending save should run.</summary>
        UniTask WaitForSaveAsync(CancellationToken cancellationToken);
    }
}
