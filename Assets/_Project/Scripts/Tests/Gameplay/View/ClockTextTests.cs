using NUnit.Framework;
using RollicCase.Gameplay.View.Hud;

namespace RollicCase.Tests.Gameplay.View
{
    public sealed class ClockTextTests
    {
        [TestCase(0, "0:00")]
        [TestCase(9, "0:09")]
        [TestCase(65, "1:05")]
        [TestCase(600, "10:00")]
        public void Write_Seconds_WritesMinutesAndPaddedSeconds(int seconds, string expected)
        {
            var buffer = new char[ClockText.BufferLength];

            int length = ClockText.Write(seconds, buffer);

            Assert.AreEqual(expected, new string(buffer, 0, length));
        }
    }
}
