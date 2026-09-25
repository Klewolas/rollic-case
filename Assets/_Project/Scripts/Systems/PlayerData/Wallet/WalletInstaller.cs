using RollicCase.Systems.PlayerData.Wallet.Validators;
using UnityEngine;
using Zenject;

namespace RollicCase.Systems.PlayerData.Wallet
{
    /// <summary>Binds the consumable catalog, the stored wallet, and the transaction validators in order.</summary>
    [CreateAssetMenu(fileName = "SO_WalletInstaller", menuName = "RollicCase/Installers/Wallet Installer")]
    public sealed class WalletInstaller : ScriptableObjectInstaller<WalletInstaller>
    {
        private const string FileName = "wallet.json";

        [SerializeField] private ConsumableCatalog _catalog;

        public override void InstallBindings()
        {
            Container.BindInstance(_catalog);
            Container.BindPersistentModel(FileName, () => new WalletModel());

            Container.Bind<ITransactionValidator>().To<NonZeroAmountValidator>().AsSingle();
            Container.Bind<ITransactionValidator>().To<KnownItemValidator>().AsSingle();
            Container.Bind<ITransactionValidator>().To<NonNegativeBalanceValidator>().AsSingle();
            Container.Bind<ITransactionValidator>().To<CapacityValidator>().AsSingle();

            Container.Bind<IWalletHandler>().To<WalletHandler>().AsSingle();
        }
    }
}
