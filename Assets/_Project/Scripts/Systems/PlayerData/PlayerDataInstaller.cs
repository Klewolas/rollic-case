using UnityEngine;
using Zenject;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Binds the storage, serialization, and save scheduling of player data.</summary>
    [CreateAssetMenu(fileName = "SO_PlayerDataInstaller", menuName = "RollicCase/Installers/Player Data Installer")]
    public sealed class PlayerDataInstaller : ScriptableObjectInstaller<PlayerDataInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IDataStorage>().To<FileDataStorage>().AsSingle();
            Container.Bind<IDataSerializer>().To<JsonDataSerializer>().AsSingle();
            Container.Bind<ISaveScheduler>().To<NextFrameSaveScheduler>().AsSingle();
            Container.BindInterfacesTo<PlayerDataService>().AsSingle();
        }
    }
}
