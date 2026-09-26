using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Gameplay.Data
{
    /// <summary>One level: the only level format, written by the level editor and read by the game.</summary>
    [CreateAssetMenu(fileName = "SO_Level_", menuName = "RollicCase/Gameplay/Level")]
    public sealed class LevelData : ScriptableObject
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;
        [SerializeField] private int _timerSeconds;
        [SerializeField] private List<BlockData> _blocks = new List<BlockData>();
        [SerializeField] private List<DoorData> _doors = new List<DoorData>();

        public int Width => _width;
        public int Height => _height;
        public int TimerSeconds => _timerSeconds;
        public IReadOnlyList<BlockData> Blocks => _blocks;
        public IReadOnlyList<DoorData> Doors => _doors;

        public void SetSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void SetTimer(int seconds)
        {
            _timerSeconds = seconds;
        }

        public void AddBlock(BlockData block)
        {
            _blocks.Add(block);
        }

        public void AddDoor(DoorData door)
        {
            _doors.Add(door);
        }
    }
}
