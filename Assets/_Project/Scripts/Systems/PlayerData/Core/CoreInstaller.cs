using UnityEngine;
using Zenject;

namespace RollicCase.Systems.PlayerData.Core
{
    /// <summary>Binds the stored core data and its handler.</summary>
    [CreateAssetMenu(fileName = "SO_CoreInstaller", menuName = "RollicCase/Installers/Core Installer")]
    public sealed class CoreInstaller : ScriptableObjectInstaller<CoreInstaller>
    {
        [SerializeField] private string _fileName;

        public override void InstallBindings()
        {
            Container.BindPersistentModel(_fileName, () => new CoreModel());
            Container.Bind<ICoreHandler>().To<CoreHandler>().AsSingle();
        }
    }
}
