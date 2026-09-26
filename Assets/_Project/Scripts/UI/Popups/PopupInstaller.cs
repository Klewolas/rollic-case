using UnityEngine;
using Zenject;

namespace RollicCase.UI.Popups
{
    /// <summary>Binds the popup system to the scene's popup canvas and catalog, and lets close buttons close the top popup.</summary>
    public sealed class PopupInstaller : MonoInstaller
    {
        [SerializeField] private PopupCatalog _catalog;

        [Header("Scene References")]
        [Tooltip("The popup canvas in the scene; assigned on the scene's context prefab instance.")]
        [SerializeField] private PopupLayer _layer;

        public override void InstallBindings()
        {
            Container.BindInstance(_layer);
            Container.BindInstance(_catalog);
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();
            Container.Bind<IPopupService>().To<PopupService>().AsSingle();
            Container.BindInterfacesTo<PopupBackButtonHandler>().AsSingle();

            Container.DeclareSignal<ClosePopupRequestedSignal>();
            Container.BindInterfacesTo<PopupCloseHandler>().AsSingle();
        }
    }
}
