namespace RollicCase.UI.Popups
{
    /// <summary>Base view of a popup that displays data it receives before opening.</summary>
    public abstract class PopupView<TArgs> : PopupView, IPopup<TArgs>
    {
        public abstract void Bind(TArgs args);
    }
}
