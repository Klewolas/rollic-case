using RollicCase.Gameplay.Flow.Signals;
using RollicCase.UI.Buttons;

namespace RollicCase.Gameplay.View.Buttons
{
    /// <summary>Asks to close the pause menu and continue the level.</summary>
    public sealed class ResumeButton : SignalButton<ResumeRequestedSignal>
    {
    }
}
