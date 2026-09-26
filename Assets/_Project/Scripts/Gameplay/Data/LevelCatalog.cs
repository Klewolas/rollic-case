using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Gameplay.Data
{
    /// <summary>The ordered levels the game plays.</summary>
    [CreateAssetMenu(fileName = "SO_LevelCatalog", menuName = "RollicCase/Gameplay/Level Catalog")]
    public sealed class LevelCatalog : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levels = new List<LevelData>();

        public IReadOnlyList<LevelData> Levels => _levels;

        /// <summary>Returns the position of the level in the play order, or -1 when it is not listed.</summary>
        public int IndexOf(LevelData level)
        {
            return _levels.IndexOf(level);
        }

        /// <summary>Appends the level to the end of the play order.</summary>
        public void Add(LevelData level)
        {
            _levels.Add(level);
        }
    }
}
