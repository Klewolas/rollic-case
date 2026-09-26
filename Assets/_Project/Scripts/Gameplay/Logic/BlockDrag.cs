using System;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Turns a continuous drag into cell moves on the session and detects exits through matching doors.</summary>
    public sealed class BlockDrag
    {
        private const float MaxFraction = 0.5f;

        private readonly LevelSession _session;
        private readonly float _exitThreshold;

        private BlockModel _block;
        private Vector2Int _start;

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
        }

        /// <summary>Moves the block toward the start cell plus the offset in cells and returns where to draw it.</summary>
        public BlockDragResult Update(Vector2 offset)
        {
            if (_session.State != LevelState.Playing)
            {
                return BlockDragResult.At(_block.Position);
            }

            Vector2 target = _start + offset;
            MoveToward(Vector2Int.RoundToInt(target));

            Vector2Int position = _block.Position;
            Vector2 residual = target - position;
            bool isHorizontal = Mathf.Abs(residual.x) >= Mathf.Abs(residual.y);
            float push = isHorizontal ? residual.x : residual.y;

            if (Mathf.Abs(push) >= _exitThreshold)
            {
                BoardSide side = GetPushSide(isHorizontal, push);

                if (_session.TryExit(_block, side))
                {
                    _block = null;
                    return BlockDragResult.Exited(position, side);
                }
            }

            float fractionX = GetFreeFraction(Vector2Int.right, residual.x);
            float fractionY = GetFreeFraction(Vector2Int.up, residual.y);

            return Mathf.Abs(fractionX) >= Mathf.Abs(fractionY)
                ? BlockDragResult.At(new Vector2(position.x + fractionX, position.y))
                : BlockDragResult.At(new Vector2(position.x, position.y + fractionY));
        }

        /// <summary>Ends the drag and returns the cell the block rests on.</summary>
        public Vector2Int End()
        {
            Vector2Int position = _block.Position;
            _block = null;
            return position;
        }

        private void MoveToward(Vector2Int target)
        {
            Vector2Int remaining = target - _block.Position;

            while (remaining != Vector2Int.zero)
            {
                var stepX = new Vector2Int(Math.Sign(remaining.x), 0);
                var stepY = new Vector2Int(0, Math.Sign(remaining.y));
                bool prefersX = Math.Abs(remaining.x) >= Math.Abs(remaining.y);
                Vector2Int primary = prefersX ? stepX : stepY;
                Vector2Int secondary = prefersX ? stepY : stepX;

                if (!TryStep(primary) && !TryStep(secondary))
                {
                    return;
                }

                remaining = target - _block.Position;
            }
        }

        private bool TryStep(Vector2Int direction)
        {
            return direction != Vector2Int.zero && _session.Move(_block, direction, 1) == 1;
        }

        private float GetFreeFraction(Vector2Int axis, float residual)
        {
            float fraction = Mathf.Clamp(residual, -MaxFraction, MaxFraction);

            if (fraction == 0f)
            {
                return 0f;
            }

            Vector2Int direction = fraction > 0f ? axis : -axis;
            return _session.Board.GetFreeDistance(_block, direction) > 0 ? fraction : 0f;
        }

        private static BoardSide GetPushSide(bool isHorizontal, float push)
        {
            if (isHorizontal)
            {
                return push > 0f ? BoardSide.Right : BoardSide.Left;
            }

            return push > 0f ? BoardSide.Top : BoardSide.Bottom;
        }
    }
}
