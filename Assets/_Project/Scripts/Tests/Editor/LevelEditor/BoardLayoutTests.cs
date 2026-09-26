using NUnit.Framework;
using RollicCase.Gameplay.Data;
using RollicCase.Editor.LevelEditor.Board;
using UnityEngine;

namespace RollicCase.Tests.Editor.LevelEditor
{
    public sealed class BoardLayoutTests
    {
        private static readonly Rect Area = new Rect(0f, 0f, 700f, 700f);

        private readonly BoardLayout _layout = new BoardLayout(Area, 5, 5);

        [Test]
        public void Constructor_SquareArea_LeavesOneCellOfRimOnEachSide()
        {
            Assert.AreEqual(100f, _layout.CellSize, 1e-3f);
            Assert.AreEqual(new Rect(100f, 100f, 500f, 500f), _layout.BoardRect);
        }

        [Test]
        public void GetCellRect_BottomLeftCell_IsDrawnAtTheBottom()
        {
            Assert.AreEqual(new Rect(100f, 500f, 100f, 100f), _layout.GetCellRect(new Vector2Int(0, 0)));
        }

        [Test]
        public void HitTest_InsideBoard_ReturnsCell()
        {
            BoardHit hit = _layout.HitTest(new Vector2(150f, 580f));

            Assert.AreEqual(BoardHitKind.Cell, hit.Kind);
            Assert.AreEqual(new Vector2Int(0, 0), hit.Cell);
        }

        [Test]
        public void HitTest_AboveBoard_ReturnsTopEdgePosition()
        {
            BoardHit hit = _layout.HitTest(new Vector2(350f, 80f));

            Assert.AreEqual(BoardHitKind.Edge, hit.Kind);
            Assert.AreEqual(BoardSide.Top, hit.Side);
            Assert.AreEqual(2, hit.Position);
        }

        [Test]
        public void HitTest_LeftOfBoard_CountsPositionFromTheBottom()
        {
            BoardHit hit = _layout.HitTest(new Vector2(80f, 580f));

            Assert.AreEqual(BoardSide.Left, hit.Side);
            Assert.AreEqual(0, hit.Position);
        }

        [Test]
        public void HitTest_BoardCorner_ReturnsNone()
        {
            Assert.AreEqual(BoardHitKind.None, _layout.HitTest(new Vector2(80f, 80f)).Kind);
        }

        [Test]
        public void GetEdgeRect_TopDoor_SitsAboveItsCells()
        {
            Rect rect = _layout.GetEdgeRect(BoardSide.Top, 1, 2);

            Assert.AreEqual(200f, rect.xMin, 1e-3f);
            Assert.AreEqual(200f, rect.width, 1e-3f);
            Assert.AreEqual(100f, rect.yMax, 1e-3f);
        }
    }
}
