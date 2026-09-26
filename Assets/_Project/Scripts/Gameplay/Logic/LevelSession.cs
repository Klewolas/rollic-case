using System;
using RollicCase.Gameplay.Data;
using UnityEngine;

namespace RollicCase.Gameplay.Logic
{
    /// <summary>Rules of one level attempt: moves and exits while playing, a win when the board is empty, a fail when time runs out.</summary>
    public sealed class LevelSession
    {
        public LevelSession(BoardModel board, LevelTimer timer)
        {
            Board = board;
            Timer = timer;
            Timer.Expired += HandleTimerExpired;
        }

        /// <summary>Raised when the state changes from Playing to Won or Failed.</summary>
        public event Action<LevelState> StateChanged;

        /// <summary>Raised when a block has left the board through a door.</summary>
        public event Action<BlockModel> BlockExited;

        public LevelState State { get; private set; } = LevelState.Playing;
        public BoardModel Board { get; }
        public LevelTimer Timer { get; }

        /// <summary>Moves the block to the position while playing; returns false when the level is over or the position is taken.</summary>
        public bool TryPlace(BlockModel block, Vector2Int position)
        {
            if (State != LevelState.Playing || !Board.CanPlace(block, position))
            {
                return false;
            }

            Board.Place(block, position);
            return true;
        }

        /// <summary>Removes the block through a matching door on the side while playing.</summary>
        public bool TryExit(BlockModel block, BoardSide side)
        {
            if (State != LevelState.Playing || Board.FindExitDoor(block, side) == null)
            {
                return false;
            }

            Board.Remove(block);
            NotifyBlockExited(block);
            BlockExited?.Invoke(block);

            if (Board.Blocks.Count == 0)
            {
                SetState(LevelState.Won);
            }

            return true;
        }

        /// <summary>Advances the timer while playing.</summary>
        public void Tick(float deltaSeconds)
        {
            if (State == LevelState.Playing)
            {
                Timer.Tick(deltaSeconds);
            }
        }

        private void NotifyBlockExited(BlockModel block)
        {
            for (int i = 0; i < Board.Blocks.Count; i++)
            {
                Board.Blocks[i].NotifyBlockExited(block);
            }
        }

        private void HandleTimerExpired()
        {
            if (State == LevelState.Playing)
            {
                SetState(LevelState.Failed);
            }
        }

        private void SetState(LevelState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }
    }
}
