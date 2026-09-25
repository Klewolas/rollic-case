using System.Threading;
using Cysharp.Threading.Tasks;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Runs pending saves on the next frame, so that all changes of one frame share one write.</summary>
    public sealed class NextFrameSaveScheduler : ISaveScheduler
    {
        public UniTask WaitForSaveAsync(CancellationToken cancellationToken)
        {
            return UniTask.NextFrame(cancellationToken);
        }
    }
}
