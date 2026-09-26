using System;
using UnityEngine;

namespace RollicCase.Gameplay.Data
{
    /// <summary>Saved door: the side it sits on, its first cell along that side, its length in cells, and its color.</summary>
    [Serializable]
    public sealed class DoorData
    {
        [SerializeField] private BoardSide _side;
        [SerializeField] private int _start;
        [SerializeField] private int _length;
        [SerializeField] private BlockColor _color;

        public DoorData(BoardSide side, int start, int length, BlockColor color)
        {
            _side = side;
            _start = start;
            _length = length;
            _color = color;
        }

        public BoardSide Side => _side;
        public int Start => _start;
        public int Length => _length;
        public BlockColor Color => _color;
    }
}
