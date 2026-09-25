using System.Threading;
using Cysharp.Threading.Tasks;
using RollicCase.Systems.PlayerData;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Persistent model that counts how often it is loaded and flushed.</summary>
    public sealed class FakePersistentModel : IPersistentModel
    {
        public int LoadCount { get; private set; }
        public int FlushAsyncCount { get; private set; }

        public UniTask LoadAsync(CancellationToken cancellationToken)
        {
            LoadCount++;
            return UniTask.CompletedTask;
        }

        public UniTask FlushAsync(CancellationToken cancellationToken)
        {
            FlushAsyncCount++;
            return UniTask.CompletedTask;
        }

        public void Flush()
        {
        }
    }
}
