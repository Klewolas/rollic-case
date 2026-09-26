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
        public void Tick_CrossesWholeSecond_RaisesWholeSecondsChanged()
        {
            var timer = new LevelTimer(10f);
            int? raised = null;
            timer.WholeSecondsChanged += seconds => raised = seconds;

            timer.Tick(1.1f);

            Assert.AreEqual(9, raised);
            Assert.AreEqual(9, timer.RemainingWholeSeconds);
        }

        [Test]
        public void Tick_WithinSameWholeSecond_DoesNotRaiseWholeSecondsChanged()
        {
            var timer = new LevelTimer(10f);
            timer.Tick(1.1f);
            int raisedCount = 0;
            timer.WholeSecondsChanged += _ => raisedCount++;

            timer.Tick(0.5f);

            Assert.AreEqual(0, raisedCount);
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
