using System;
using RollicCase.Gameplay.Flow.Signals;
using RollicCase.Gameplay.Logic;
using RollicCase.Gameplay.View.Popups;
using RollicCase.UI.Popups;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>Stops scaled time and opens the pause menu on request, and restores time when the menu closes or the scene ends.</summary>
    public sealed class PausePresenter : IInitializable, IDisposable
    {
        private const float PausedTimeScale = 0f;
        private const float RunningTimeScale = 1f;

        private readonly SignalBus _signalBus;
        private readonly IPopupService _popupService;
        private readonly LevelSession _session;

        private PausePopup _popup;

        public PausePresenter(SignalBus signalBus, IPopupService popupService, LevelSession session)
        {
            _signalBus = signalBus;
            _popupService = popupService;
            _session = session;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<PauseRequestedSignal>(Pause);
            _signalBus.Subscribe<ResumeRequestedSignal>(Resume);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<PauseRequestedSignal>(Pause);
            _signalBus.Unsubscribe<ResumeRequestedSignal>(Resume);
            Time.timeScale = RunningTimeScale;
        }

        private void Pause()
        {
            if (_popup != null || _session.State != LevelState.Playing)
            {
                return;
            }

            Time.timeScale = PausedTimeScale;
            _popup = _popupService.Show<PausePopup>();
            _popup.Closed += HandlePopupClosed;
        }

        private void Resume()
        {
            _popupService.CloseTop();
        }

        private void HandlePopupClosed()
        {
            _popup.Closed -= HandlePopupClosed;
            _popup = null;
            Time.timeScale = RunningTimeScale;
        }
    }
}
