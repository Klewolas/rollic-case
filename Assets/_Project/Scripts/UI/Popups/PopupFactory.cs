using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RollicCase.UI.Popups
{
    /// <summary>Creates each popup once under the popup layer and reuses it afterwards.</summary>
    public sealed class PopupFactory : IPopupFactory
    {
        private readonly DiContainer _container;
        private readonly PopupCatalog _catalog;
        private readonly Transform _layer;
        private readonly Dictionary<Type, IPopup> _instances = new Dictionary<Type, IPopup>();

        public PopupFactory(DiContainer container, PopupCatalog catalog, PopupLayer layer)
        {
            _container = container;
            _catalog = catalog;
            _layer = layer.Content;
        }

        public TPopup Get<TPopup>() where TPopup : class, IPopup
        {
            if (_instances.TryGetValue(typeof(TPopup), out IPopup instance))
            {
                return (TPopup)instance;
            }

            PopupView prefab = _catalog.GetPrefab<TPopup>();
            PopupView view = _container.InstantiatePrefabForComponent<PopupView>(prefab, _layer);
            _instances.Add(typeof(TPopup), view);
            return view as TPopup;
        }
    }
}
