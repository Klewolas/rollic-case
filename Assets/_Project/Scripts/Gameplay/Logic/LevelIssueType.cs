namespace RollicCase.Gameplay.Logic
{
    /// <summary>Problems that make a level invalid.</summary>
    public enum LevelIssueType
    {
        BoardSizeOutOfRange,
        TimerOutOfRange,
        NoBlocks,
        BlockOutOfBounds,
        BlocksOverlap,
        DoorOutOfBounds,
        DoorsOverlap,
        MissingDoorForColor,
        BlockDoesNotFitAnyDoor
    }
}
