using UnityEngine;

namespace RollicCase.Tests.Gameplay
{
    /// <summary>Block cell layouts used by the gameplay tests.</summary>
    public static class Shapes
    {
        public static readonly Vector2Int[] Single = { new Vector2Int(0, 0) };
        public static readonly Vector2Int[] Horizontal2 = { new Vector2Int(0, 0), new Vector2Int(1, 0) };
        public static readonly Vector2Int[] Vertical2 = { new Vector2Int(0, 0), new Vector2Int(0, 1) };
        public static readonly Vector2Int[] Square2 = { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) };
        public static readonly Vector2Int[] L = { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(0, 1) };
    }
}
