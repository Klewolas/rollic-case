using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using RollicCase.Gameplay.View.Board;
using UnityEngine;

namespace RollicCase.Gameplay.View.Blocks
{
    /// <summary>Maps the finger on the screen to board cells, drives the block drag, and plays the result on the block view.</summary>
    public sealed class BlockDragPresenter : IBlockDragHandler
    {
        private readonly BlockDrag _drag;
        private readonly BoardModel _board;
        private readonly BoardRootView _root;
        private readonly Camera _camera;

        private BlockView _dragged;
        private Plane _dragPlane;
        private Vector2 _grabCell;

        public BlockDragPresenter(BlockDrag drag, BoardModel board, BoardRootView root, Camera camera)
        {
            _drag = drag;
            _board = board;
            _root = root;
            _camera = camera;
        }

        public void HandlePressed(BlockView block, Vector3 worldPoint)
        {
            if (_dragged != null)
            {
                return;
            }

            _dragged = block;
            _dragPlane = new Plane(_root.Blocks.up, worldPoint);
            _grabCell = WorldToCell(worldPoint);
            _drag.Begin(block.Model);
            block.SetGrabbed(true);
        }

        public void HandleDragged(BlockView block, Vector2 screenPosition)
        {
            if (block != _dragged)
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(screenPosition);

            if (!_dragPlane.Raycast(ray, out float distance))
            {
                return;
            }

            BlockDragResult result = _drag.Update(WorldToCell(ray.GetPoint(distance)) - _grabCell);

            if (result.HasExited)
            {
                _dragged = null;
                PlayExit(block, result);
                return;
            }

            block.MoveTo(result.Position);
        }

        public void HandleReleased(BlockView block)
        {
            if (block != _dragged)
            {
                return;
            }

            _dragged = null;
            block.SetGrabbed(false);
            block.SnapTo(_drag.End());
        }

        private Vector2 WorldToCell(Vector3 worldPoint)
        {
            Vector3 local = _root.Blocks.InverseTransformPoint(worldPoint);
            return new Vector2(local.x, local.z) / BoardSpace.CellSize;
        }

        private void PlayExit(BlockView block, BlockDragResult result)
        {
            Vector2Int direction = result.ExitSide.ToDirection();
            var exitAxis = new Vector3(Mathf.Abs(direction.x), 0f, Mathf.Abs(direction.y));
            Vector3 start = BoardSpace.CellToWorld(result.Position);
            float wall = direction.x + direction.y > 0 ? GetFarWallOffset(result.ExitSide) : 0f;

            Vector3 doorPosition = Vector3.Scale(start, Vector3.one - exitAxis) + exitAxis * wall;
            block.PlayExit(doorPosition, Vector3.one - exitAxis);
        }

        private float GetFarWallOffset(BoardSide side)
        {
            return (side.IsHorizontal() ? _board.Height : _board.Width) * BoardSpace.CellSize;
        }
    }
}
