using UnityEngine;
using Zenject;

namespace RollicCase.UI
{
    /// <summary>Binds the shared UI configuration and initializes tweening.</summary>
    [CreateAssetMenu(fileName = "SO_UIInstaller", menuName = "RollicCase/Installers/UI Installer")]
    public sealed class UIInstaller : ScriptableObjectInstaller<UIInstaller>
    {
        [SerializeField] private UIConfig _config;

        public override void InstallBindings()
        {
            Container.BindInstance(_config);
            Container.BindInterfacesTo<TweenInitializer>().AsSingle();
        }
    }
}
