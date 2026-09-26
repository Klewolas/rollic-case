using NUnit.Framework;
using RollicCase.Gameplay.View.Cameras;
using UnityEngine;

namespace RollicCase.Tests.Gameplay.View
{
    public sealed class CameraFramingTests
    {
        private const float FieldOfView = 60f;
        private const float Portrait = 9f / 16f;

        [Test]
        public void GetDistance_TallBoard_FitsItsHeight()
        {
            float distance = CameraFraming.GetDistance(1f, 4f, Portrait, FieldOfView, 1f);

            float visibleHalfHeight = distance * Mathf.Tan(FieldOfView * 0.5f * Mathf.Deg2Rad);
            Assert.AreEqual(4f, visibleHalfHeight, 1e-3f);
        }

        [Test]
        public void GetDistance_WideBoardOnPortraitScreen_FitsItsWidth()
        {
            float distance = CameraFraming.GetDistance(4f, 1f, Portrait, FieldOfView, 1f);

            float visibleHalfWidth = distance * Mathf.Tan(FieldOfView * 0.5f * Mathf.Deg2Rad) * Portrait;
            Assert.AreEqual(4f, visibleHalfWidth, 1e-3f);
        }

        [Test]
        public void GetDistance_Padding_MovesTheCameraFurtherAway()
        {
            float tight = CameraFraming.GetDistance(3f, 3f, Portrait, FieldOfView, 1f);
            float padded = CameraFraming.GetDistance(3f, 3f, Portrait, FieldOfView, 1.2f);

            Assert.AreEqual(tight * 1.2f, padded, 1e-3f);
        }
    }
}
