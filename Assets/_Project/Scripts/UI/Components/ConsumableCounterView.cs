using RollicCase.Systems.PlayerData.Wallet;
using TMPro;
using UnityEngine;
using Zenject;

namespace RollicCase.UI.Components
{
    /// <summary>Shows the wallet balance of one consumable and follows its changes.</summary>
    public sealed class ConsumableCounterView : MonoBehaviour
    {
        private const string CountFormat = "{0}";

        [SerializeField] private ConsumableDefinition _item;
        [SerializeField] private TMP_Text _label;

        private IWalletHandler _wallet;

        [Inject]
        public void Construct(IWalletHandler wallet)
        {
            _wallet = wallet;
            _wallet.BalanceChanged += HandleBalanceChanged;
            Show(_wallet.GetBalance(_item));
        }

        private void OnDestroy()
        {
            if (_wallet != null)
            {
                _wallet.BalanceChanged -= HandleBalanceChanged;
            }
        }

        private void HandleBalanceChanged(ConsumableBalanceChange change)
        {
            if (change.Item == _item)
            {
                Show(change.NewBalance);
            }
        }

        private void Show(int balance)
        {
            _label.SetText(CountFormat, balance);
        }
    }
}
