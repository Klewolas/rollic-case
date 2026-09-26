using UnityEngine;

namespace RollicCase.Gameplay.View
{
    /// <summary>Built-in mapping between board cells and world space, fixed by the supplied model geometry.</summary>
    public static class BoardSpace
    {
        /// <summary>World size of one cell; the modular block pieces are one quarter of a cell.</summary>
        public const float CellSize = 2f;

        /// <summary>Width of the rim around the board in world units.</summary>
        public const float RimWidth = 1f;

        /// <summary>Block pieces are modeled lying on XY with their top facing -Z.</summary>
        public static readonly Quaternion BlockPieceUpright = Quaternion.Euler(90f, 0f, 0f);

        /// <summary>Rim and door pieces are exported upside down.</summary>
        public static readonly Quaternion RimPieceUpright = Quaternion.Euler(180f, 0f, 0f);

        /// <summary>Moves a quadrant piece so it rotates around its own center instead of its corner pivot.</summary>
        public static readonly Vector3 QuadrantPivotOffset = new Vector3(-0.5f, 0f, -0.5f);

        /// <summary>Returns the world position of the cell's lower-left corner.</summary>
        public static Vector3 CellToWorld(Vector2 cell)
        {
            return new Vector3(cell.x * CellSize, 0f, cell.y * CellSize);
        }

        /// <summary>Returns the world position of the cell's center.</summary>
        public static Vector3 CellCenterToWorld(Vector2Int cell)
        {
            return CellToWorld(cell) + new Vector3(CellSize * 0.5f, 0f, CellSize * 0.5f);
        }
    }
}
