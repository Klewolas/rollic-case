using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Runtime state of one block: its color, shape, features, and current position on the board.</summary>
    public sealed class BlockModel
    {
        private readonly Vector2Int[] _cells;
        private readonly RectInt _localBounds;
        private readonly List<BlockFeature> _features = new List<BlockFeature>();
        private readonly List<IBlockMoveRule> _moveRules = new List<IBlockMoveRule>();
        private readonly List<IBlockExitListener> _exitListeners = new List<IBlockExitListener>();

        public BlockModel(int id, BlockColor color, Vector2Int position, IReadOnlyList<Vector2Int> cells,
            IReadOnlyList<BlockFeature> features = null)
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

            for (int i = 0; features != null && i < features.Count; i++)
            {
                AddFeature(features[i]);
            }
        }

        public int Id { get; }
        public BlockColor Color { get; }
        public Vector2Int Position { get; internal set; }

        /// <summary>Cells relative to the position.</summary>
        public IReadOnlyList<Vector2Int> Cells => _cells;

        /// <summary>Board-space rectangle that encloses every cell.</summary>
        public RectInt Bounds => new RectInt(Position + _localBounds.position, _localBounds.size);

        /// <summary>Behaviors added to this block; empty for a plain block.</summary>
        public IReadOnlyList<BlockFeature> Features => _features;

        /// <summary>Returns whether every feature of the block allows a move in the direction.</summary>
        public bool CanMove(Vector2Int direction)
        {
            for (int i = 0; i < _moveRules.Count; i++)
            {
                if (!_moveRules[i].CanMove(direction))
                {
                    return false;
                }
            }

            return true;
        }

        internal void NotifyBlockExited(BlockModel block)
        {
            for (int i = 0; i < _exitListeners.Count; i++)
            {
                _exitListeners[i].HandleBlockExited(block);
            }
        }

        private void AddFeature(BlockFeature feature)
        {
            _features.Add(feature);

            if (feature is IBlockMoveRule moveRule)
            {
                _moveRules.Add(moveRule);
            }

            if (feature is IBlockExitListener exitListener)
            {
                _exitListeners.Add(exitListener);
            }
        }
    }
}
