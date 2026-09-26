namespace RollicCase.Gameplay.View.Popups
{
    /// <summary>What the win popup shows.</summary>
    public readonly struct WinPopupArgs
    {
        public WinPopupArgs(int coins)
        {
            Coins = coins;
        }

        /// <summary>Coins earned by the win.</summary>
        public int Coins { get; }
    }
}
