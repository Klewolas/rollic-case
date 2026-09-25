using UnityEngine;
using Zenject;

namespace RollicCase.Systems.PlayerData.Core
{
    /// <summary>Binds the stored core data and its handler.</summary>
    [CreateAssetMenu(fileName = "SO_CoreInstaller", menuName = "RollicCase/Installers/Core Installer")]
    public sealed class CoreInstaller : ScriptableObjectInstaller<CoreInstaller>
    {
        private const string FileName = "core.json";

        public override void InstallBindings()
        {
            Container.BindPersistentModel(FileName, () => new CoreModel());
            Container.Bind<ICoreHandler>().To<CoreHandler>().AsSingle();
        }
    }
}
