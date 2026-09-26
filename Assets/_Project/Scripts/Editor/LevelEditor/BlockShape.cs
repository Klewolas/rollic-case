using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor
{
    /// <summary>A block shape offered in the level editor palette; the level stores the cells, not the shape.</summary>
    [CreateAssetMenu(fileName = "SO_Shape_", menuName = "RollicCase/Level Editor/Block Shape")]
    public sealed class BlockShape : ScriptableObject
    {
        [Tooltip("Cells of the shape. (0, 0) is the cell you click when you place it.")]
        [SerializeField] private List<Vector2Int> _cells = new List<Vector2Int> { Vector2Int.zero };

        public IReadOnlyList<Vector2Int> Cells => _cells;
    }
}
