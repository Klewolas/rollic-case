using UnityEngine;
using Zenject;

namespace RollicCase.UI.Popups
{
    /// <summary>Dismisses the top popup when the Android back button is pressed; the only per-frame check of the popup system.</summary>
    public sealed class PopupBackButtonHandler : ITickable
    {
        private readonly IPopupService _popupService;

        public PopupBackButtonHandler(IPopupService popupService)
        {
            _popupService = popupService;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _popupService.DismissTop();
            }
        }
    }
}
