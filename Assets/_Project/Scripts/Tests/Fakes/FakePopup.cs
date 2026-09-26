using System;
using RollicCase.UI.Popups;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Popup that records how it was opened and closed.</summary>
    public class FakePopup : IPopup
    {
        public event Action<IPopup> CloseRequested;

        public bool CanDismiss { get; set; } = true;
        public bool IsOpen { get; private set; }
        public int OpenCount { get; private set; }

        public void Open()
        {
            IsOpen = true;
            OpenCount++;
        }

        public void Close()
        {
            IsOpen = false;
        }

        /// <summary>Simulates the popup's own close button.</summary>
        public void RequestClose()
        {
            CloseRequested?.Invoke(this);
        }
    }
}
