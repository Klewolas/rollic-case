using RollicCase.Meta.Home;
using RollicCase.Meta.Settings;
using Zenject;

namespace RollicCase.Meta
{
    /// <summary>Binds the Home screen: play, the settings popup, and the setting toggles.</summary>
    public sealed class MapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<PlayRequestedSignal>();
            Container.DeclareSignal<SettingsRequestedSignal>();
            Container.DeclareSignal<SettingToggleRequestedSignal>();

            Container.BindInterfacesTo<HomeNavigation>().AsSingle();
            Container.BindInterfacesTo<SettingsPresenter>().AsSingle();
        }
    }
}
