using NUnit.Framework;
using RollicCase.UI.Components;
using UnityEngine;

namespace RollicCase.Tests.UI.Components
{
    public sealed class SafeAreaAnchorsTests
    {
        private static readonly Vector2 ScreenSize = new Vector2(1080f, 2400f);

        [Test]
        public void Calculate_SafeAreaIsFullScreen_ReturnsFullAnchors()
        {
            SafeAreaAnchors.Calculate(new Rect(Vector2.zero, ScreenSize), ScreenSize, out Vector2 min, out Vector2 max);

            Assert.AreEqual(Vector2.zero, min);
            Assert.AreEqual(Vector2.one, max);
        }

        [Test]
        public void Calculate_NotchAtTopAndBarAtBottom_InsetsBothEdges()
        {
            var safeArea = new Rect(0f, 120f, 1080f, 2160f);

            SafeAreaAnchors.Calculate(safeArea, ScreenSize, out Vector2 min, out Vector2 max);

            Assert.AreEqual(0f, min.x, 1e-5f);
            Assert.AreEqual(0.05f, min.y, 1e-5f);
            Assert.AreEqual(1f, max.x, 1e-5f);
            Assert.AreEqual(0.95f, max.y, 1e-5f);
        }
    }
}
