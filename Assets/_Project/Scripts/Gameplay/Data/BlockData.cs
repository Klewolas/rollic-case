using System;
using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Gameplay.Data
{
    /// <summary>Saved block: its color, its origin cell, and the cells it covers relative to the origin.</summary>
    [Serializable]
    public sealed class BlockData
    {
        [SerializeField] private BlockColor _color;
        [SerializeField] private Vector2Int _origin;
        [SerializeField] private List<Vector2Int> _cells;

        public BlockData(BlockColor color, Vector2Int origin, IEnumerable<Vector2Int> cells)
        {
            _color = color;
            _origin = origin;
            _cells = new List<Vector2Int>(cells);
        }

        public BlockColor Color => _color;
        public Vector2Int Origin => _origin;
        public IReadOnlyList<Vector2Int> Cells => _cells;
    }
}
