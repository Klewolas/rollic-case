using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Runtime state of one block: its color, shape, and current position on the board.</summary>
    public sealed class BlockModel
    {
        private readonly Vector2Int[] _cells;
        private readonly RectInt _localBounds;

        public BlockModel(int id, BlockColor color, Vector2Int position, IReadOnlyList<Vector2Int> cells)
        {
            Id = id;
            Color = color;
            Position = position;
            _cells = new Vector2Int[cells.Count];

            for (int i = 0; i < cells.Count; i++)
            {
                _cells[i] = cells[i];
            }

            _localBounds = CellBounds.Calculate(_cells);
        }

        public int Id { get; }
        public BlockColor Color { get; }
        public Vector2Int Position { get; internal set; }

        /// <summary>Cells relative to the position.</summary>
        public IReadOnlyList<Vector2Int> Cells => _cells;

        /// <summary>Board-space rectangle that encloses every cell.</summary>
        public RectInt Bounds => new RectInt(Position + _localBounds.position, _localBounds.size);
    }
}
