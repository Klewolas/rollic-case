using RollicCase.Gameplay.Data;
using RollicCase.Systems.PlayerData.Core;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>Picks the catalog level for the player's progress and loops back to the first level after the last.</summary>
    public sealed class LevelProvider : ILevelProvider
    {
        private readonly LevelCatalog _catalog;
        private readonly ICoreHandler _core;

        public LevelProvider(LevelCatalog catalog, ICoreHandler core)
        {
            _catalog = catalog;
            _core = core;
        }

        public int CurrentLevelNumber => _core.CurrentLevelNumber;
        public LevelData CurrentLevel => _catalog.Levels[(CurrentLevelNumber - 1) % _catalog.Levels.Count];
    }
}
