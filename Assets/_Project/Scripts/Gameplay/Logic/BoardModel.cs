using System;
using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Grid occupancy of the blocks, with the movement and exit rules.</summary>
    public sealed class BoardModel
    {
        private const float Epsilon = 1e-4f;

        private readonly BlockModel[] _occupancy;
        private readonly List<BlockModel> _blocks;
        private readonly List<DoorModel> _doors;

        public BoardModel(int width, int height, IReadOnlyList<BlockModel> blocks, IReadOnlyList<DoorModel> doors)
        {
            Width = width;
            Height = height;
            _occupancy = new BlockModel[width * height];
            _blocks = new List<BlockModel>(blocks);
            _doors = new List<DoorModel>(doors);

            for (int i = 0; i < _blocks.Count; i++)
            {
                Occupy(_blocks[i]);
            }
        }

        public int Width { get; }
        public int Height { get; }
        public IReadOnlyList<BlockModel> Blocks => _blocks;
        public IReadOnlyList<DoorModel> Doors => _doors;

        /// <summary>Returns the block that covers the cell, or null when the cell is empty or outside the board.</summary>
        public BlockModel GetBlockAt(Vector2Int cell)
        {
            return IsInside(cell) ? _occupancy[ToIndex(cell)] : null;
        }

        /// <summary>Returns how far the block, drawn at a continuous position in cells, can slide in the direction before it touches a wall or another block.</summary>
        public float GetFreeTravel(BlockModel block, Vector2 position, Vector2Int direction)
        {
            if (!block.CanMove(direction))
            {
                return 0f;
            }

            float travel = float.MaxValue;

            for (int i = 0; i < block.Cells.Count; i++)
            {
                travel = Mathf.Min(travel, GetCellTravel(block, position + block.Cells[i], direction));
            }

            return travel;
        }

        /// <summary>Returns whether the block fits at the position without leaving the board or overlapping another block.</summary>
        public bool CanPlace(BlockModel block, Vector2Int position)
        {
            for (int i = 0; i < block.Cells.Count; i++)
            {
                if (IsBlocked(block, position + block.Cells[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Moves the block to the position; the position must be free.</summary>
        public void Place(BlockModel block, Vector2Int position)
        {
            if (!CanPlace(block, position))
            {
                throw new InvalidOperationException($"Block {block.Id} cannot be placed at {position}.");
            }

            Fill(block, null);
            block.Position = position;
            Fill(block, block);
        }

        /// <summary>Returns the door the block can exit through on the side, or null when it cannot exit there.</summary>
        public DoorModel FindExitDoor(BlockModel block, BoardSide side)
        {
            RectInt bounds = block.Bounds;

            if (!IsFlush(bounds, side) || !block.CanMove(side.ToDirection()))
            {
                return null;
            }

            int first = side.IsHorizontal() ? bounds.xMin : bounds.yMin;
            int last = (side.IsHorizontal() ? bounds.xMax : bounds.yMax) - 1;

            for (int i = 0; i < _doors.Count; i++)
            {
                DoorModel door = _doors[i];

                if (door.Side == side && door.Color == block.Color && door.Start <= first && last <= door.End)
                {
                    return door;
                }
            }

            return null;
        }

        /// <summary>Takes the block off the board and frees its cells.</summary>
        public void Remove(BlockModel block)
        {
            Fill(block, null);
            _blocks.Remove(block);
        }

        private void Occupy(BlockModel block)
        {
            for (int i = 0; i < block.Cells.Count; i++)
            {
                Vector2Int cell = block.Position + block.Cells[i];

                if (!IsInside(cell))
                {
                    throw new ArgumentException($"Block {block.Id} covers {cell}, which is outside the board.");
                }

                if (_occupancy[ToIndex(cell)] != null)
                {
                    throw new ArgumentException($"Block {block.Id} overlaps another block at {cell}.");
                }

                _occupancy[ToIndex(cell)] = block;
            }
        }

        private float GetCellTravel(BlockModel block, Vector2 cellCorner, Vector2Int direction)
        {
            bool isHorizontal = direction.x != 0;
            int step = isHorizontal ? direction.x : direction.y;
            float across = isHorizontal ? cellCorner.y : cellCorner.x;
            float along = isHorizontal ? cellCorner.x : cellCorner.y;
            int firstLane = Mathf.FloorToInt(across + Epsilon);
            int lastLane = Mathf.CeilToInt(across + 1f - Epsilon) - 1;

            float leadingEdge = step > 0 ? along + 1f : along;
            int line = step > 0 ? Mathf.CeilToInt(leadingEdge - Epsilon) : Mathf.FloorToInt(leadingEdge + Epsilon) - 1;

            while (!IsLineBlocked(block, line, firstLane, lastLane, isHorizontal))
            {
                line += step;
            }

            return step > 0 ? line - leadingEdge : leadingEdge - (line + 1);
        }

        private bool IsLineBlocked(BlockModel block, int line, int firstLane, int lastLane, bool isHorizontal)
        {
            for (int lane = firstLane; lane <= lastLane; lane++)
            {
                if (IsBlocked(block, isHorizontal ? new Vector2Int(line, lane) : new Vector2Int(lane, line)))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsBlocked(BlockModel block, Vector2Int cell)
        {
            if (!IsInside(cell))
            {
                return true;
            }

            BlockModel occupant = _occupancy[ToIndex(cell)];
            return occupant != null && occupant != block;
        }

        private void Fill(BlockModel block, BlockModel value)
        {
            for (int i = 0; i < block.Cells.Count; i++)
            {
                _occupancy[ToIndex(block.Position + block.Cells[i])] = value;
            }
        }

        private bool IsFlush(RectInt bounds, BoardSide side)
        {
            switch (side)
            {
                case BoardSide.Bottom: return bounds.yMin == 0;
                case BoardSide.Top: return bounds.yMax == Height;
                case BoardSide.Left: return bounds.xMin == 0;
                case BoardSide.Right: return bounds.xMax == Width;
                default: throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
        }

        private bool IsInside(Vector2Int cell)
        {
            return LevelGeometry.IsInside(Width, Height, cell);
        }

        private int ToIndex(Vector2Int cell)
        {
            return cell.y * Width + cell.x;
        }
    }
}
