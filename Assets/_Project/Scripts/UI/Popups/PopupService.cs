using System.Collections.Generic;

namespace RollicCase.UI.Popups
{
    /// <summary>Keeps open popups on a stack so they close in reverse order.</summary>
    public sealed class PopupService : IPopupService
    {
        private readonly IPopupFactory _factory;
        private readonly List<IPopup> _openPopups = new List<IPopup>();

        public PopupService(IPopupFactory factory)
        {
            _factory = factory;
        }

        public TPopup Show<TPopup>() where TPopup : class, IPopup
        {
            TPopup popup = _factory.Get<TPopup>();

            if (_openPopups.Contains(popup))
            {
                return popup;
            }

            _openPopups.Add(popup);
            popup.CloseRequested += Close;
            popup.Open();
            return popup;
        }

        public TPopup Show<TPopup, TArgs>(TArgs args) where TPopup : class, IPopup<TArgs>
        {
            _factory.Get<TPopup>().Bind(args);
            return Show<TPopup>();
        }

        public bool CloseTop()
        {
            if (_openPopups.Count == 0)
            {
                return false;
            }

            Close(_openPopups[_openPopups.Count - 1]);
            return true;
        }

        private void Close(IPopup popup)
        {
            if (!_openPopups.Remove(popup))
            {
                return;
            }

            popup.CloseRequested -= Close;
            popup.Close();
        }
    }
}
