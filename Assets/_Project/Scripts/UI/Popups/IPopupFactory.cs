namespace RollicCase.UI.Popups
{
    /// <summary>Provides the single instance of each popup type.</summary>
    public interface IPopupFactory
    {
        /// <summary>Returns the popup instance, creating it on first use.</summary>
        TPopup Get<TPopup>() where TPopup : class, IPopup;
    }
}
