using System.Collections.Generic;
using NUnit.Framework;
using RollicCase.Gameplay.View.Blocks;
using UnityEngine;

namespace RollicCase.Tests.Gameplay.View
{
    public sealed class BlockPieceLayoutTests
    {
        [Test]
        public void Build_SingleCell_UsesFourOuterCornersFacingOutward()
        {
            List<BlockPiece> pieces = BlockPieceLayout.Build(Shapes.Single);

            Assert.AreEqual(4, pieces.Count);
            AssertHasPiece(pieces, BlockPieceType.OuterCorner, new Vector2(0.75f, 0.75f), 0f);
            AssertHasPiece(pieces, BlockPieceType.OuterCorner, new Vector2(0.75f, 0.25f), 90f);
            AssertHasPiece(pieces, BlockPieceType.OuterCorner, new Vector2(0.25f, 0.25f), 180f);
            AssertHasPiece(pieces, BlockPieceType.OuterCorner, new Vector2(0.25f, 0.75f), 270f);
        }

        [Test]
        public void Build_HorizontalPair_JoinsCellsWithEdgesFacingUpAndDown()
        {
            List<BlockPiece> pieces = BlockPieceLayout.Build(Shapes.Horizontal2);

            Assert.AreEqual(4, Count(pieces, BlockPieceType.OuterCorner));
            Assert.AreEqual(4, Count(pieces, BlockPieceType.Edge));
            AssertHasPiece(pieces, BlockPieceType.Edge, new Vector2(0.75f, 0.75f), 0f);
            AssertHasPiece(pieces, BlockPieceType.Edge, new Vector2(1.25f, 0.25f), 180f);
        }

        [Test]
        public void Build_VerticalPair_UsesEdgesFacingLeftAndRight()
        {
            List<BlockPiece> pieces = BlockPieceLayout.Build(Shapes.Vertical2);

            AssertHasPiece(pieces, BlockPieceType.Edge, new Vector2(0.75f, 0.75f), 90f);
            AssertHasPiece(pieces, BlockPieceType.Edge, new Vector2(0.25f, 1.25f), 270f);
        }

        [Test]
        public void Build_Square_FillsTheMiddleWithCenters()
        {
            List<BlockPiece> pieces = BlockPieceLayout.Build(Shapes.Square2);

            Assert.AreEqual(4, Count(pieces, BlockPieceType.Center));
            Assert.AreEqual(8, Count(pieces, BlockPieceType.Edge));
            Assert.AreEqual(4, Count(pieces, BlockPieceType.OuterCorner));
        }

        [Test]
        public void Build_LShape_PlacesOneInnerCornerThatReplacesThreeQuadrants()
        {
            List<BlockPiece> pieces = BlockPieceLayout.Build(Shapes.L);

            Assert.AreEqual(1, Count(pieces, BlockPieceType.InnerCorner));
            AssertHasPiece(pieces, BlockPieceType.InnerCorner, new Vector2(1f, 1f), 0f);
            Assert.AreEqual(10, pieces.Count);
        }

        private static int Count(List<BlockPiece> pieces, BlockPieceType type)
        {
            int count = 0;

            foreach (BlockPiece piece in pieces)
            {
                if (piece.Type == type)
                {
                    count++;
                }
            }

            return count;
        }

        private static void AssertHasPiece(List<BlockPiece> pieces, BlockPieceType type, Vector2 position, float yaw)
        {
            foreach (BlockPiece piece in pieces)
            {
                if (piece.Type == type && Vector2.Distance(piece.Position, position) < 1e-4f && Mathf.Approximately(piece.Yaw, yaw))
                {
                    return;
                }
            }

            Assert.Fail($"Expected {type} at {position} with yaw {yaw}.");
        }
    }
}
