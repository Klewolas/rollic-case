using RollicCase.UI.Popups;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Popup with data that records whether it was bound before opening.</summary>
    public sealed class FakeArgsPopup : FakePopup, IPopup<int>
    {
        public int BoundArgs { get; private set; }
        public bool WasBoundBeforeOpen { get; private set; }

        public void Bind(int args)
        {
            BoundArgs = args;
            WasBoundBeforeOpen = !IsOpen;
        }
    }
}
