using RollicCase.Gameplay.Flow.Signals;
using RollicCase.UI.Buttons;

namespace RollicCase.Gameplay.View.Buttons
{
    /// <summary>Asks to leave the level and return to the Home screen.</summary>
    public sealed class HomeButton : SignalButton<HomeRequestedSignal>
    {
    }
}
