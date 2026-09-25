using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using RollicCase.Systems.PlayerData;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Save scheduler that releases pending saves only when the test calls Tick.</summary>
    public sealed class ManualSaveScheduler : ISaveScheduler
    {
        private readonly List<UniTaskCompletionSource> _pending = new List<UniTaskCompletionSource>();

        public int PendingCount => _pending.Count;

        public UniTask WaitForSaveAsync(CancellationToken cancellationToken)
        {
            var source = new UniTaskCompletionSource();
            _pending.Add(source);
            return source.Task;
        }

        /// <summary>Releases every pending save, like the next frame would.</summary>
        public void Tick()
        {
            UniTaskCompletionSource[] released = _pending.ToArray();
            _pending.Clear();

            foreach (UniTaskCompletionSource source in released)
            {
                source.TrySetResult();
            }
        }
    }
}
