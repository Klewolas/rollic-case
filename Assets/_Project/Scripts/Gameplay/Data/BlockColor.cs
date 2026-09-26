using UnityEngine;

namespace RollicCase.Gameplay.Data
{
    /// <summary>A block and door color; blocks exit only through doors of the same color asset.</summary>
    [CreateAssetMenu(fileName = "SO_BlockColor_", menuName = "RollicCase/Gameplay/Block Color")]
    public sealed class BlockColor : ScriptableObject
    {
        [SerializeField] private Color _displayColor = Color.white;

        /// <summary>The color blocks and doors are tinted with, in the game and in the level editor.</summary>
        public Color DisplayColor => _displayColor;
    }
}
