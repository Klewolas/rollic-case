using RollicCase.Gameplay.Logic;
using TMPro;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.View.Hud
{
    /// <summary>Shows the level countdown as minutes and seconds, updated once per second.</summary>
    public sealed class TimerView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;

        private readonly char[] _buffer = new char[ClockText.BufferLength];
        private LevelTimer _timer;

        [Inject]
        public void Construct(LevelSession session)
        {
            _timer = session.Timer;
            _timer.WholeSecondsChanged += Show;
            Show(_timer.RemainingWholeSeconds);
        }

        private void OnDestroy()
        {
            if (_timer != null)
            {
                _timer.WholeSecondsChanged -= Show;
            }
        }

        private void Show(int seconds)
        {
            _label.SetCharArray(_buffer, 0, ClockText.Write(seconds, _buffer));
        }
    }
}
