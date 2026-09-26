using NUnit.Framework;
using RollicCase.Tests.Gameplay;
using RollicCase.Editor.LevelEditor.Editing;
using UnityEngine;

namespace RollicCase.Tests.Editor.LevelEditor
{
    public sealed class ShapeRotationTests
    {
        [Test]
        public void RotateClockwise_Horizontal_BecomesVertical()
        {
            Vector2Int[] rotated = ShapeRotation.RotateClockwise(Shapes.Horizontal2);

            CollectionAssert.AreEquivalent(Shapes.Vertical2, rotated);
        }

        [Test]
        public void RotateClockwise_L_TurnsAndStaysAtOrigin()
        {
            Vector2Int[] rotated = ShapeRotation.RotateClockwise(Shapes.L);

            CollectionAssert.AreEquivalent(new[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 1) }, rotated);
        }

        [Test]
        public void RotateClockwise_FourTimes_ReturnsOriginalShape()
        {
            Vector2Int[] cells = Shapes.L;

            for (int i = 0; i < 4; i++)
            {
                cells = ShapeRotation.RotateClockwise(cells);
            }

            CollectionAssert.AreEquivalent(Shapes.L, cells);
        }
    }
}
