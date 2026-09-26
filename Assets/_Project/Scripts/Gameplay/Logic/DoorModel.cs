using RollicCase.Gameplay.Data;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>A door on a board side through which blocks of its color can exit.</summary>
    public sealed class DoorModel
    {
        public DoorModel(BoardSide side, int start, int length, BlockColor color)
        {
            Side = side;
            Start = start;
            Length = length;
            Color = color;
        }

        public BoardSide Side { get; }
        public int Start { get; }
        public int Length { get; }
        public BlockColor Color { get; }

        /// <summary>Last cell index along the side that the door covers.</summary>
        public int End => Start + Length - 1;
    }
}
