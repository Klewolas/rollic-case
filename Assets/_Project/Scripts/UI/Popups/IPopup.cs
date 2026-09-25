using System;

namespace RollicCase.UI.Popups
{
    /// <summary>A popup that the popup service can open and close.</summary>
    public interface IPopup
    {
        /// <summary>Raised when the popup asks to be closed, for example by its close button.</summary>
        event Action<IPopup> CloseRequested;

        /// <summary>Shows the popup on top of the others.</summary>
        void Open();

        /// <summary>Hides the popup.</summary>
        void Close();
    }
}
