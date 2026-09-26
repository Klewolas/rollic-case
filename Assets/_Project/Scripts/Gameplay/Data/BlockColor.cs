using UnityEngine;

namespace RollicCase.Gameplay.Data
{
    /// <summary>A block and door color; blocks exit only through doors of the same color asset.</summary>
    [CreateAssetMenu(fileName = "SO_BlockColor_", menuName = "RollicCase/Gameplay/Block Color")]
    public sealed class BlockColor : ScriptableObject
    {
        [SerializeField] private Color _displayColor = Color.white;

        /// <summary>Flat color used wherever the color is shown without a material, such as the level editor.</summary>
        public Color DisplayColor => _displayColor;
    }
}
