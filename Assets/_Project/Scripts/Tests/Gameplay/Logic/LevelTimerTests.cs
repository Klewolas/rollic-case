using NUnit.Framework;
using RollicCase.Gameplay.Logic;

namespace RollicCase.Tests.Gameplay.Logic
{
    public sealed class LevelTimerTests
    {
        [Test]
        public void Tick_ElapsedTime_ReducesRemaining()
        {
            var timer = new LevelTimer(10f);

            timer.Tick(3f);

            Assert.AreEqual(7f, timer.RemainingSeconds, 1e-5f);
            Assert.IsFalse(timer.IsExpired);
        }

        [Test]
        public void Tick_PastZero_ClampsToZeroAndExpires()
        {
            var timer = new LevelTimer(2f);

            timer.Tick(5f);

            Assert.AreEqual(0f, timer.RemainingSeconds);
            Assert.IsTrue(timer.IsExpired);
        }

        [Test]
        public void Tick_AfterExpired_RaisesExpiredOnlyOnce()
        {
            var timer = new LevelTimer(1f);
            int expiredCount = 0;
            timer.Expired += () => expiredCount++;

            timer.Tick(1f);
            timer.Tick(1f);

            Assert.AreEqual(1, expiredCount);
        }
    }
}
