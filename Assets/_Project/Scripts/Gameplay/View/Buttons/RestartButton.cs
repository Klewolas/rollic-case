using RollicCase.Gameplay.Flow.Signals;
using RollicCase.UI.Buttons;

namespace RollicCase.Gameplay.View.Buttons
{
    /// <summary>Asks to play the current level again from its start.</summary>
    public sealed class RestartButton : SignalButton<RestartRequestedSignal>
    {
    }
}
