using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Loads, caches, and saves one model with coalesced, serialized writes.</summary>
    public sealed class ModelProvider<TModel> : IModelProvider<TModel>, IPersistentModel, IDisposable where TModel : class
    {
        private readonly string _key;
        private readonly Func<TModel> _createDefault;
        private readonly IDataStorage _storage;
        private readonly IDataSerializer _serializer;
        private readonly ISaveScheduler _saveScheduler;
        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);
        private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

        private TModel _model;
        private bool _isDirty;
        private bool _isSaveScheduled;

        public ModelProvider(string key, Func<TModel> createDefault, IDataStorage storage, IDataSerializer serializer, ISaveScheduler saveScheduler)
        {
            _key = key;
            _createDefault = createDefault;
            _storage = storage;
            _serializer = serializer;
            _saveScheduler = saveScheduler;
        }

        public TModel Model => _model ??= Parse(_storage.Read(_key));

        public async UniTask LoadAsync(CancellationToken cancellationToken)
        {
            if (_model != null)
            {
                return;
            }

            string content = await _storage.ReadAsync(_key, cancellationToken);
            _model ??= Parse(content);
        }

        public void MarkDirty()
        {
            _isDirty = true;

            if (_isSaveScheduled)
            {
                return;
            }

            _isSaveScheduled = true;
            SaveWhenScheduledAsync(_cancellation.Token).Forget();
        }

        public async UniTask FlushAsync(CancellationToken cancellationToken)
        {
            await _writeLock.WaitAsync(cancellationToken);

            try
            {
                if (!_isDirty)
                {
                    return;
                }

                _isDirty = false;

                try
                {
                    await _storage.WriteAsync(_key, _serializer.Serialize(_model), cancellationToken);
                }
                catch
                {
                    _isDirty = true;
                    throw;
                }
            }
            finally
            {
                _writeLock.Release();
            }
        }

        public void Flush()
        {
            if (!_isDirty)
            {
                return;
            }

            _isDirty = false;
            _storage.Write(_key, _serializer.Serialize(_model));
        }

        public void Dispose()
        {
            _cancellation.Cancel();
            _cancellation.Dispose();
            _writeLock.Dispose();
        }

        private async UniTaskVoid SaveWhenScheduledAsync(CancellationToken cancellationToken)
        {
            await _saveScheduler.WaitForSaveAsync(cancellationToken);
            _isSaveScheduled = false;
            await FlushAsync(cancellationToken);
        }

        private TModel Parse(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return _createDefault();
            }

            try
            {
                return _serializer.Deserialize<TModel>(content) ?? _createDefault();
            }
            catch (ArgumentException exception)
            {
                Debug.LogWarning($"Stored data '{_key}' is invalid and was replaced with defaults: {exception.Message}");
                return _createDefault();
            }
        }
    }
}
