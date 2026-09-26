using RollicCase.Gameplay.Flow.Signals;
using RollicCase.UI.Buttons;

namespace RollicCase.Gameplay.View.Buttons
{
    /// <summary>Asks to pause the level.</summary>
    public sealed class PauseButton : SignalButton<PauseRequestedSignal>
    {
    }
}
