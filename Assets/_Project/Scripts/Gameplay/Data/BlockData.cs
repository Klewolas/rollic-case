using System;
using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Gameplay.Data
{
    /// <summary>Saved block: its color, its origin cell, the cells it covers relative to the origin, and its features.</summary>
    [Serializable]
    public sealed class BlockData
    {
        [SerializeField] private BlockColor _color;
        [SerializeField] private Vector2Int _origin;
        [SerializeField] private List<Vector2Int> _cells;
        [SerializeReference] private List<BlockFeatureData> _features;

        public BlockData(BlockColor color, Vector2Int origin, IEnumerable<Vector2Int> cells,
            IEnumerable<BlockFeatureData> features = null)
        {
            _color = color;
            _origin = origin;
            _cells = new List<Vector2Int>(cells);
            _features = features == null ? new List<BlockFeatureData>() : new List<BlockFeatureData>(features);
        }

        public BlockColor Color => _color;
        public Vector2Int Origin => _origin;
        public IReadOnlyList<Vector2Int> Cells => _cells;

        /// <summary>Settings of the features that change how this block behaves; empty for a plain block.</summary>
        public IReadOnlyList<BlockFeatureData> Features => _features;

        /// <summary>Returns a copy of the block at another origin.</summary>
        public BlockData WithOrigin(Vector2Int origin)
        {
            return new BlockData(_color, origin, _cells, _features);
        }

        /// <summary>Returns a copy of the block with other cells.</summary>
        public BlockData WithCells(IEnumerable<Vector2Int> cells)
        {
            return new BlockData(_color, _origin, cells, _features);
        }
    }
}
