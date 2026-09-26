using RollicCase.UI.Popups;
using TMPro;
using UnityEngine;

namespace RollicCase.Gameplay.View.Popups
{
    /// <summary>Shown when the board is cleared, with the coin reward and continue.</summary>
    public sealed class WinPopup : PopupView<WinPopupArgs>
    {
        [SerializeField] private TMP_Text _rewardLabel;
        [Tooltip("The reward text; {0} is replaced by the coins earned.")]
        [SerializeField] private string _rewardFormat;

        public override void Bind(WinPopupArgs args)
        {
            _rewardLabel.SetText(_rewardFormat, args.Coins);
        }
    }
}
