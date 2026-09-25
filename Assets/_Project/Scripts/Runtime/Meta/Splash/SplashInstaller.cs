using UnityEngine;
using Zenject;

namespace RollicCase.Meta.Splash
{
    /// <summary>Binds the splash scene flow and its configuration.</summary>
    public sealed class SplashInstaller : MonoInstaller
    {
        [SerializeField] private SplashConfig _splashConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_splashConfig);
            Container.BindInterfacesTo<SplashFlow>().AsSingle();
        }
    }
}
