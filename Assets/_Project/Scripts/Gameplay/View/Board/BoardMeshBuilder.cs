using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;
using UnityEngine;

namespace RollicCase.Gameplay.View.Board
{
    /// <summary>Combines the ground tiles, the rim, and each door into single meshes.</summary>
    public sealed class BoardMeshBuilder
    {
        private const float HalfCell = BoardSpace.CellSize * 0.5f;
        private const float HalfRim = BoardSpace.RimWidth * 0.5f;
        private const float AlongSide = 0f;
        private const float AcrossSide = 90f;
        private const float TopRightCorner = 0f;
        private const float BottomRightCorner = 90f;
        private const float BottomLeftCorner = 180f;
        private const float TopLeftCorner = 270f;

        private readonly BoardViewConfig _config;

        public BoardMeshBuilder(BoardViewConfig config)
        {
            _config = config;
        }

        /// <summary>Returns one mesh with a ground tile under every cell.</summary>
        public Mesh BuildGround(int width, int height)
        {
            var parts = new List<CombineInstance>(width * height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    parts.Add(Part(_config.GroundTile, BoardSpace.CellCenterToWorld(new Vector2Int(x, y)), Quaternion.identity));
                }
            }

            return Combine(parts, "Ground");
        }

        /// <summary>Returns one mesh with the rim walls and corners, leaving gaps where doors are.</summary>
        public Mesh BuildRim(BoardModel board)
        {
            var parts = new List<CombineInstance>();
            AddSideWalls(parts, board, BoardSide.Bottom, board.Width);
            AddSideWalls(parts, board, BoardSide.Top, board.Width);
            AddSideWalls(parts, board, BoardSide.Left, board.Height);
            AddSideWalls(parts, board, BoardSide.Right, board.Height);

            float right = board.Width * BoardSpace.CellSize + HalfRim;
            float top = board.Height * BoardSpace.CellSize + HalfRim;
            parts.Add(RimPart(_config.Corner, new Vector3(right, 0f, top), TopRightCorner));
            parts.Add(RimPart(_config.Corner, new Vector3(right, 0f, -HalfRim), BottomRightCorner));
            parts.Add(RimPart(_config.Corner, new Vector3(-HalfRim, 0f, -HalfRim), BottomLeftCorner));
            parts.Add(RimPart(_config.Corner, new Vector3(-HalfRim, 0f, top), TopLeftCorner));
            return Combine(parts, "Rim");
        }

        /// <summary>Returns one mesh with a door piece on every cell the door spans.</summary>
        public Mesh BuildDoor(BoardModel board, DoorModel door)
        {
            var parts = new List<CombineInstance>(door.Length);

            for (int i = door.Start; i <= door.End; i++)
            {
                parts.Add(RimPart(_config.Door, GetRimCellCenter(board, door.Side, i), YawOf(door.Side)));
            }

            return Combine(parts, "Door");
        }

        /// <summary>Returns one mesh with an arrow on top of every door cell, pointing out of the board.</summary>
        public Mesh BuildDoorArrows(BoardModel board, DoorModel door)
        {
            var parts = new List<CombineInstance>(door.Length);
            Vector2Int outward = door.Side.ToDirection();
            Quaternion rotation = Quaternion.LookRotation(-new Vector3(outward.x, 0f, outward.y)) * BoardSpace.RimPieceUpright;
            Bounds arrow = _config.DoorArrow.bounds;
            float doorTop = -_config.Door.bounds.min.y;
            Vector3 arrowBase = rotation * new Vector3(arrow.center.x, arrow.max.y, arrow.center.z);

            for (int i = door.Start; i <= door.End; i++)
            {
                Vector3 position = GetRimCellCenter(board, door.Side, i) + Vector3.up * doorTop - arrowBase;
                parts.Add(Part(_config.DoorArrow, position, rotation));
            }

            return Combine(parts, "DoorArrows");
        }

        private void AddSideWalls(List<CombineInstance> parts, BoardModel board, BoardSide side, int length)
        {
            Vector3 along = side.IsHorizontal() ? Vector3.right : Vector3.forward;

            for (int i = 0; i < length; i++)
            {
                if (HasDoor(board, side, i))
                {
                    continue;
                }

                Vector3 center = GetRimCellCenter(board, side, i);
                parts.Add(RimPart(_config.Wall, center - along * HalfRim, YawOf(side)));
                parts.Add(RimPart(_config.Wall, center + along * HalfRim, YawOf(side)));
            }
        }

        private static Vector3 GetRimCellCenter(BoardModel board, BoardSide side, int index)
        {
            float along = index * BoardSpace.CellSize + HalfCell;

            switch (side)
            {
                case BoardSide.Bottom: return new Vector3(along, 0f, -HalfRim);
                case BoardSide.Top: return new Vector3(along, 0f, board.Height * BoardSpace.CellSize + HalfRim);
                case BoardSide.Left: return new Vector3(-HalfRim, 0f, along);
                default: return new Vector3(board.Width * BoardSpace.CellSize + HalfRim, 0f, along);
            }
        }

        private static bool HasDoor(BoardModel board, BoardSide side, int index)
        {
            for (int i = 0; i < board.Doors.Count; i++)
            {
                DoorModel door = board.Doors[i];

                if (door.Side == side && door.Start <= index && index <= door.End)
                {
                    return true;
                }
            }

            return false;
        }

        private static float YawOf(BoardSide side)
        {
            return side.IsHorizontal() ? AlongSide : AcrossSide;
        }

        private static CombineInstance RimPart(Mesh mesh, Vector3 position, float yaw)
        {
            return Part(mesh, position, Quaternion.Euler(0f, yaw, 0f) * BoardSpace.RimPieceUpright);
        }

        private static CombineInstance Part(Mesh mesh, Vector3 position, Quaternion rotation)
        {
            return new CombineInstance
            {
                mesh = mesh,
                transform = Matrix4x4.TRS(position, rotation, Vector3.one)
            };
        }

        private static Mesh Combine(List<CombineInstance> parts, string name)
        {
            var mesh = new Mesh { name = name };
            mesh.CombineMeshes(parts.ToArray(), true, true);
            return mesh;
        }
    }
}
