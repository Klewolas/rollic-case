using System.Threading;
using Cysharp.Threading.Tasks;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Reads and writes raw player data by key.</summary>
    public interface IDataStorage
    {
        /// <summary>Returns the stored content, or null when nothing is stored.</summary>
        string Read(string key);

        /// <summary>Returns the stored content without blocking the main thread, or null when nothing is stored.</summary>
        UniTask<string> ReadAsync(string key, CancellationToken cancellationToken);

        /// <summary>Stores the content and blocks until it is written.</summary>
        void Write(string key, string content);

        /// <summary>Stores the content without blocking the main thread.</summary>
        UniTask WriteAsync(string key, string content, CancellationToken cancellationToken);
    }
}
