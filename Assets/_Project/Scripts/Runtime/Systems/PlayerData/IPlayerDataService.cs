using System.Threading;
using Cysharp.Threading.Tasks;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Coordinates loading of all player data models.</summary>
    public interface IPlayerDataService
    {
        /// <summary>Loads every player data model.</summary>
        UniTask LoadAllAsync(CancellationToken cancellationToken);
    }
}
