using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Keeps a dragged block under the finger on both axes, stops it flush against walls and other blocks, and detects exits through matching doors.</summary>
    public sealed class BlockDrag
    {
        private readonly LevelSession _session;
        private readonly float _exitThreshold;

        private BlockModel _block;
        private Vector2 _start;
        private Vector2 _position;

        /// <param name="exitThreshold">How many cells a flush block must be pushed past its wall to leave through a door.</param>
        public BlockDrag(LevelSession session, float exitThreshold)
        {
            _session = session;
            _exitThreshold = exitThreshold;
        }

        /// <summary>Starts dragging the block from its current cell.</summary>
        public void Begin(BlockModel block)
        {
            _block = block;
            _start = block.Position;
            _position = _start;
        }

        /// <summary>Slides the block toward the start cell plus the offset in cells and returns where to draw it.</summary>
        public BlockDragResult Update(Vector2 offset)
        {
            if (_session.State != LevelState.Playing)
            {
                return BlockDragResult.At(_position);
            }

            // The second horizontal pass lets the block slide around a corner it only cleared after moving vertically.
            Vector2 target = _start + offset;
            Slide(Vector2Int.right, target.x - _position.x);
            Slide(Vector2Int.up, target.y - _position.y);
            Slide(Vector2Int.right, target.x - _position.x);

            Vector2Int cell = Vector2Int.RoundToInt(_position);
            _session.TryPlace(_block, cell);

            Vector2 push = target - _position;
            bool isHorizontal = Mathf.Abs(push.x) >= Mathf.Abs(push.y);
            float amount = isHorizontal ? push.x : push.y;
            BoardSide side = GetPushSide(isHorizontal, amount);

            if (Mathf.Abs(amount) >= _exitThreshold && _session.TryExit(_block, side))
            {
                _block = null;
                return BlockDragResult.Exited(cell, side);
            }

            return BlockDragResult.At(_position);
        }

        /// <summary>Ends the drag and returns the cell the block settles into.</summary>
        public Vector2Int End()
        {
            Vector2Int cell = _block.Position;
            _block = null;
            return cell;
        }

        private void Slide(Vector2Int axis, float distance)
        {
            if (Mathf.Approximately(distance, 0f))
            {
                return;
            }

            Vector2Int direction = distance > 0f ? axis : -axis;
            float travel = Mathf.Min(Mathf.Abs(distance), _session.Board.GetFreeTravel(_block, _position, direction));
            _position += (Vector2)direction * travel;
        }

        private static BoardSide GetPushSide(bool isHorizontal, float amount)
        {
            if (isHorizontal)
            {
                return amount > 0f ? BoardSide.Right : BoardSide.Left;
            }

            return amount > 0f ? BoardSide.Top : BoardSide.Bottom;
        }
    }
}
