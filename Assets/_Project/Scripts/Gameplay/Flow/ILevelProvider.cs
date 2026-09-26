using RollicCase.Gameplay.Data;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>Tells the game which level to play.</summary>
    public interface ILevelProvider
    {
        /// <summary>1-based number shown to the player; it keeps counting after the catalog loops.</summary>
        int CurrentLevelNumber { get; }

        LevelData CurrentLevel { get; }
    }
}
