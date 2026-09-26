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

        public LevelState State { get; private set; } = LevelState.Playing;
        public BoardModel Board { get; }
        public LevelTimer Timer { get; }

        /// <summary>Moves the block while playing and returns the steps actually taken.</summary>
        public int Move(BlockModel block, Vector2Int direction, int steps)
        {
            if (State != LevelState.Playing)
            {
                return 0;
            }

            int allowedSteps = Mathf.Min(steps, Board.GetFreeDistance(block, direction));

            if (allowedSteps > 0)
            {
                Board.Move(block, direction, allowedSteps);
            }

            return allowedSteps;
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
