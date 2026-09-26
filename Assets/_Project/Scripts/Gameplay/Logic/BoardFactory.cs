using RollicCase.Gameplay.Data;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Builds a fresh board model from level data.</summary>
    public sealed class BoardFactory
    {
        /// <summary>Creates a board in the start state of the level.</summary>
        public BoardModel Create(LevelData level)
        {
            var blocks = new BlockModel[level.Blocks.Count];

            for (int i = 0; i < blocks.Length; i++)
            {
                BlockData block = level.Blocks[i];
                blocks[i] = new BlockModel(i, block.Color, block.Origin, block.Cells, CreateFeatures(block));
            }

            var doors = new DoorModel[level.Doors.Count];

            for (int i = 0; i < doors.Length; i++)
            {
                DoorData door = level.Doors[i];
                doors[i] = new DoorModel(door.Side, door.Start, door.Length, door.Color);
            }

            return new BoardModel(level.Width, level.Height, blocks, doors);
        }

        private static BlockFeature[] CreateFeatures(BlockData block)
        {
            var features = new BlockFeature[block.Features.Count];

            for (int i = 0; i < features.Length; i++)
            {
                features[i] = block.Features[i].CreateFeature();
            }

            return features;
        }
    }
}
