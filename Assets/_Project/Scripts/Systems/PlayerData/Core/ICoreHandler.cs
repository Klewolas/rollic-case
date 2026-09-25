namespace RollicCase.Systems.PlayerData.Core
{
    /// <summary>Reads and advances the core player data.</summary>
    public interface ICoreHandler
    {
        /// <summary>The 1-based number of the level the player plays next.</summary>
        int CurrentLevelNumber { get; }

        /// <summary>Marks the current level as completed.</summary>
        void CompleteCurrentLevel();
    }
}
