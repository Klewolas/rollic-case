using RollicCase.Gameplay.Logic;
using UnityEngine;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Feature that forbids its block from moving in one direction.</summary>
    public sealed class FakeMoveRule : BlockFeature, IBlockMoveRule
    {
        private readonly Vector2Int _blockedDirection;

        public FakeMoveRule(Vector2Int blockedDirection)
        {
            _blockedDirection = blockedDirection;
        }

        public bool CanMove(Vector2Int direction)
        {
            return direction != _blockedDirection;
        }
    }
}
