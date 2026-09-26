using System;
using RollicCase.Gameplay.Logic;
using RollicCase.Gameplay.View.Popups;
using RollicCase.UI.Popups;
using Zenject;

namespace RollicCase.Gameplay.Flow
{
    /// <summary>Reacts to the end of the level: a win completes it once and opens the win popup, a fail opens the fail popup.</summary>
    public sealed class LevelResultPresenter : IInitializable, IDisposable
    {
        private readonly LevelSession _session;
        private readonly LevelCompletion _completion;
        private readonly LevelRewardConfig _reward;
        private readonly IPopupService _popupService;

        public LevelResultPresenter(LevelSession session, LevelCompletion completion, LevelRewardConfig reward,
            IPopupService popupService)
        {
            _session = session;
            _completion = completion;
            _reward = reward;
            _popupService = popupService;
        }

        public void Initialize()
        {
            _session.StateChanged += HandleStateChanged;
        }

        public void Dispose()
        {
            _session.StateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(LevelState state)
        {
            if (state == LevelState.Won && _completion.TryComplete())
            {
                _popupService.Show<WinPopup, WinPopupArgs>(new WinPopupArgs(_reward.CoinAmount));
            }
            else if (state == LevelState.Failed)
            {
                _popupService.Show<FailPopup>();
            }
        }
    }
}
