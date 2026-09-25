namespace RollicCase.UI.Popups
{
    /// <summary>Opens popups on a stack and closes them in reverse order.</summary>
    public interface IPopupService
    {
        /// <summary>Opens the popup on top of the stack; an open popup is returned without opening it again.</summary>
        TPopup Show<TPopup>() where TPopup : class, IPopup;

        /// <summary>Binds the data, then opens the popup on top of the stack.</summary>
        TPopup Show<TPopup, TArgs>(TArgs args) where TPopup : class, IPopup<TArgs>;

        /// <summary>Closes the topmost popup and returns false when no popup is open.</summary>
        bool CloseTop();
    }
}
