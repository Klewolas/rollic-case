namespace RollicCase.Gameplay.Logic
{
    /// <summary>One validation problem and the index of the block or door it concerns, or -1 for the whole level.</summary>
    public readonly struct LevelIssue
    {
        public const int NoIndex = -1;

        public LevelIssue(LevelIssueType type, int index = NoIndex)
        {
            Type = type;
            Index = index;
        }

        public LevelIssueType Type { get; }
        public int Index { get; }
    }
}
