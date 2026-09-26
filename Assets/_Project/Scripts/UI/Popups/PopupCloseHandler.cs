using System;
using Zenject;

namespace RollicCase.UI.Popups
{
    /// <summary>Closes the topmost popup when a close button asks for it.</summary>
    public sealed class PopupCloseHandler : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IPopupService _popupService;

        public PopupCloseHandler(SignalBus signalBus, IPopupService popupService)
        {
            _signalBus = signalBus;
            _popupService = popupService;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ClosePopupRequestedSignal>(CloseTop);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ClosePopupRequestedSignal>(CloseTop);
        }

        private void CloseTop()
        {
            _popupService.CloseTop();
        }
    }
}
