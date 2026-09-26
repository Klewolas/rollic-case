using RollicCase.Gameplay.Logic;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay
{
    /// <summary>Advances the level session with scaled time, so pausing the game also stops the timer.</summary>
    public sealed class LevelSessionTicker : ITickable
    {
        private readonly LevelSession _session;

        public LevelSessionTicker(LevelSession session)
        {
            _session = session;
        }

        public void Tick()
        {
            _session.Tick(Time.deltaTime);
        }
    }
}
