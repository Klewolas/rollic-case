using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using RollicCase.Systems.PlayerData;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Storage that keeps content in memory and counts writes.</summary>
    public sealed class InMemoryDataStorage : IDataStorage
    {
        private readonly Dictionary<string, string> _contents = new Dictionary<string, string>();

        public int WriteCount { get; private set; }

        /// <summary>Stores content without counting it as a write.</summary>
        public void Seed(string key, string content)
        {
            _contents[key] = content;
        }

        public string Read(string key)
        {
            return _contents.TryGetValue(key, out string content) ? content : null;
        }

        public UniTask<string> ReadAsync(string key, CancellationToken cancellationToken)
        {
            return UniTask.FromResult(Read(key));
        }

        public void Write(string key, string content)
        {
            _contents[key] = content;
            WriteCount++;
        }

        public UniTask WriteAsync(string key, string content, CancellationToken cancellationToken)
        {
            Write(key, content);
            return UniTask.CompletedTask;
        }
    }
}
