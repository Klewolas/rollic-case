using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Gameplay.View.Blocks
{
    /// <summary>Chooses the modular piece and rotation for every cell quadrant from the block's own neighbors.</summary>
    public static class BlockPieceLayout
    {
        private const float QuadrantOffset = 0.25f;
        private const float CellCenter = 0.5f;
        private const float HalfCell = 0.5f;
        private const float FacingUp = 0f;
        private const float FacingRight = 90f;
        private const float FacingDown = 180f;
        private const float FacingLeft = 270f;

        private static readonly Vector2Int[] s_quadrants = { new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1) };

        /// <summary>Returns the pieces that together draw the block with rounded outer and filleted inner corners.</summary>
        public static List<BlockPiece> Build(IReadOnlyList<Vector2Int> cells)
        {
            var occupied = new HashSet<Vector2Int>(cells);
            var coveredQuadrants = new HashSet<(Vector2Int Cell, Vector2Int Quadrant)>();
            var pieces = new List<BlockPiece>();

            foreach (Vector2Int cell in cells)
            {
                foreach (Vector2Int quadrant in s_quadrants)
                {
                    if (IsConcave(occupied, cell, quadrant))
                    {
                        Vector2 vertex = new Vector2(cell.x + CellCenter, cell.y + CellCenter) + (Vector2)quadrant * HalfCell;
                        pieces.Add(new BlockPiece(BlockPieceType.InnerCorner, vertex, CornerYaw(quadrant)));
                        coveredQuadrants.Add((cell, quadrant));
                        coveredQuadrants.Add((cell + new Vector2Int(quadrant.x, 0), new Vector2Int(-quadrant.x, quadrant.y)));
                        coveredQuadrants.Add((cell + new Vector2Int(0, quadrant.y), new Vector2Int(quadrant.x, -quadrant.y)));
                    }
                }
            }

            foreach (Vector2Int cell in cells)
            {
                foreach (Vector2Int quadrant in s_quadrants)
                {
                    if (!coveredQuadrants.Contains((cell, quadrant)))
                    {
                        pieces.Add(CreateQuadrantPiece(occupied, cell, quadrant));
                    }
                }
            }

            return pieces;
        }

        private static BlockPiece CreateQuadrantPiece(HashSet<Vector2Int> occupied, Vector2Int cell, Vector2Int quadrant)
        {
            bool hasHorizontal = occupied.Contains(cell + new Vector2Int(quadrant.x, 0));
            bool hasVertical = occupied.Contains(cell + new Vector2Int(0, quadrant.y));
            Vector2 position = new Vector2(cell.x + CellCenter, cell.y + CellCenter) + (Vector2)quadrant * QuadrantOffset;

            if (hasHorizontal && hasVertical)
            {
                return new BlockPiece(BlockPieceType.Center, position, FacingUp);
            }

            if (hasHorizontal)
            {
                return new BlockPiece(BlockPieceType.Edge, position, quadrant.y > 0 ? FacingUp : FacingDown);
            }

            if (hasVertical)
            {
                return new BlockPiece(BlockPieceType.Edge, position, quadrant.x > 0 ? FacingRight : FacingLeft);
            }

            return new BlockPiece(BlockPieceType.OuterCorner, position, CornerYaw(quadrant));
        }

        private static bool IsConcave(HashSet<Vector2Int> occupied, Vector2Int cell, Vector2Int quadrant)
        {
            return occupied.Contains(cell + new Vector2Int(quadrant.x, 0))
                && occupied.Contains(cell + new Vector2Int(0, quadrant.y))
                && !occupied.Contains(cell + quadrant);
        }

        private static float CornerYaw(Vector2Int quadrant)
        {
            if (quadrant.x > 0)
            {
                return quadrant.y > 0 ? FacingUp : FacingRight;
            }

            return quadrant.y > 0 ? FacingLeft : FacingDown;
        }
    }
}
