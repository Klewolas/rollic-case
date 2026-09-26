using System.Collections.Generic;
using RollicCase.Gameplay.Logic;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor.Editing
{
    /// <summary>Rotates block shapes on the grid.</summary>
    public static class ShapeRotation
    {
        /// <summary>Returns the cells turned 90 degrees clockwise and moved so the lowest x and y are zero.</summary>
        public static Vector2Int[] RotateClockwise(IReadOnlyList<Vector2Int> cells)
        {
            var rotated = new Vector2Int[cells.Count];

            for (int i = 0; i < cells.Count; i++)
            {
                rotated[i] = new Vector2Int(cells[i].y, -cells[i].x);
            }

            Vector2Int offset = CellBounds.Calculate(rotated).position;

            for (int i = 0; i < rotated.Length; i++)
            {
                rotated[i] -= offset;
            }

            return rotated;
        }
    }
}
