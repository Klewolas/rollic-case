using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Stores data as files under the persistent data path and replaces them atomically.</summary>
    public sealed class FileDataStorage : IDataStorage
    {
        private readonly string _directory;
        private readonly string _temporarySuffix;
        private readonly object _fileLock = new object();

        public FileDataStorage(PlayerDataConfig config)
        {
            _directory = Path.Combine(Application.persistentDataPath, config.DirectoryName);
            _temporarySuffix = config.TemporaryFileSuffix;
        }

        public string Read(string key)
        {
            lock (_fileLock)
            {
                string path = GetPath(key);

                if (File.Exists(path))
                {
                    return File.ReadAllText(path);
                }

                string temporaryPath = path + _temporarySuffix;
                return File.Exists(temporaryPath) ? File.ReadAllText(temporaryPath) : null;
            }
        }

        public UniTask<string> ReadAsync(string key, CancellationToken cancellationToken)
        {
            return UniTask.RunOnThreadPool(() => Read(key), cancellationToken: cancellationToken);
        }

        public void Write(string key, string content)
        {
            lock (_fileLock)
            {
                Directory.CreateDirectory(_directory);
                string path = GetPath(key);
                string temporaryPath = path + _temporarySuffix;
                File.WriteAllText(temporaryPath, content);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                File.Move(temporaryPath, path);
            }
        }

        public UniTask WriteAsync(string key, string content, CancellationToken cancellationToken)
        {
            return UniTask.RunOnThreadPool(() => Write(key, content), cancellationToken: cancellationToken);
        }

        private string GetPath(string key)
        {
            return Path.Combine(_directory, key);
        }
    }
}
