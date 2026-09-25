namespace RollicCase.UI.Popups
{
    /// <summary>A popup that displays data it receives before it opens.</summary>
    public interface IPopup<in TArgs> : IPopup
    {
        /// <summary>Fills the popup with the data to display.</summary>
        void Bind(TArgs args);
    }
}
