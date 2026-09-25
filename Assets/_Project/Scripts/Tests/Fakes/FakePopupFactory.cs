using System;
using System.Collections.Generic;
using RollicCase.UI.Popups;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Factory that returns popups registered by the test.</summary>
    public sealed class FakePopupFactory : IPopupFactory
    {
        private readonly Dictionary<Type, IPopup> _popups = new Dictionary<Type, IPopup>();

        public void Register<TPopup>(TPopup popup) where TPopup : class, IPopup
        {
            _popups[typeof(TPopup)] = popup;
        }

        public TPopup Get<TPopup>() where TPopup : class, IPopup
        {
            return (TPopup)_popups[typeof(TPopup)];
        }
    }
}
