using UnityEngine;
using Zenject;

namespace RollicCase.Systems.PlayerData.Settings
{
    /// <summary>Binds the stored settings and their handler.</summary>
    [CreateAssetMenu(fileName = "SO_SettingsInstaller", menuName = "RollicCase/Installers/Settings Installer")]
    public sealed class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        private const string FileName = "settings.json";

        public override void InstallBindings()
        {
            Container.BindPersistentModel(FileName, () => new SettingsModel());
            Container.Bind<ISettingsHandler>().To<SettingsHandler>().AsSingle();
        }
    }
}
