using UnityEngine;
using Zenject;

namespace RollicCase.UI.Popups
{
    /// <summary>Binds the popup system of a scene to its popup layer and catalog.</summary>
    public sealed class PopupInstaller : MonoInstaller
    {
        [SerializeField] private PopupCatalog _catalog;
        [SerializeField] private Transform _popupLayer;

        public override void InstallBindings()
        {
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle().WithArguments(_catalog, _popupLayer);
            Container.Bind<IPopupService>().To<PopupService>().AsSingle();
            Container.BindInterfacesTo<PopupBackButtonHandler>().AsSingle();
        }
    }
}
