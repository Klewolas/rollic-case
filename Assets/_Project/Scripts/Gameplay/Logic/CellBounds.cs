using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Bounding rectangle of a set of cells.</summary>
    public static class CellBounds
    {
        /// <summary>Returns the smallest rectangle that contains every cell.</summary>
        public static RectInt Calculate(IReadOnlyList<Vector2Int> cells)
        {
            Vector2Int min = cells[0];
            Vector2Int max = cells[0];

            for (int i = 1; i < cells.Count; i++)
            {
                min = Vector2Int.Min(min, cells[i]);
                max = Vector2Int.Max(max, cells[i]);
            }

            return new RectInt(min, max - min + Vector2Int.one);
        }
    }
}
