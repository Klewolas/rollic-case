using Zenject;

namespace RollicCase.UI.Buttons
{
    /// <summary>A button that fires its signal on click, so no script needs a reference to the button.</summary>
    public abstract class SignalButton<TSignal> : ButtonView where TSignal : struct
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void HandleClick()
        {
            _signalBus.Fire(CreateSignal());
        }

        /// <summary>Creates the signal to fire; a button that sends data with the click overrides it.</summary>
        protected virtual TSignal CreateSignal()
        {
            return default;
        }
    }
}
