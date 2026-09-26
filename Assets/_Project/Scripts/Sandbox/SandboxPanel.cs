using System;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Flow.Signals;
using RollicCase.Gameplay.Logic;
using RollicCase.Gameplay.View.Hud;
using UnityEngine;
using Zenject;

namespace RollicCase.Sandbox
{
    /// <summary>The dev driver overlay in the top-right corner of the Game view: level, time, state, and remaining blocks, with level switching and restart.</summary>
    public sealed class SandboxPanel : IInitializable, IDisposable, IGuiRenderable
    {
        private const float ReferenceWidth = 540f;
        private const float PanelWidth = 230f;
        private const float Margin = 8f;
        private const string Title = "Game Sandbox";
        private const string TimePrefix = "Time: ";
        private const string StatePrefix = "State: ";
        private const string BlocksPrefix = "Blocks left: ";
        private const string PreviousLabel = "< Previous";
        private const string NextLabel = "Next >";
        private const string RestartLabel = "Restart";

        private readonly LevelSession _session;
        private readonly LevelData _level;
        private readonly SandboxLevelLibrary _library;
        private readonly SandboxLevelSelection _selection;
        private readonly SignalBus _signalBus;
        private readonly char[] _timeBuffer = new char[ClockText.BufferLength];

        private string _timeText;
        private string _stateText;
        private string _blocksText;

        public SandboxPanel(LevelSession session, LevelData level, SandboxLevelLibrary library, SandboxLevelSelection selection,
            SignalBus signalBus)
        {
            _session = session;
            _level = level;
            _library = library;
            _selection = selection;
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            HandleSecondsChanged(_session.Timer.RemainingWholeSeconds);
            HandleStateChanged(_session.State);
            HandleBlockExited(null);

            _session.Timer.WholeSecondsChanged += HandleSecondsChanged;
            _session.StateChanged += HandleStateChanged;
            _session.BlockExited += HandleBlockExited;
        }

        public void Dispose()
        {
            _session.Timer.WholeSecondsChanged -= HandleSecondsChanged;
            _session.StateChanged -= HandleStateChanged;
            _session.BlockExited -= HandleBlockExited;
        }

        public void GuiRender()
        {
            float scale = Screen.width / ReferenceWidth;
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));

            GUILayout.BeginArea(new Rect(ReferenceWidth - PanelWidth - Margin, Margin, PanelWidth, Screen.height / scale));
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(Title);
            GUILayout.Label(_level.name);
            DrawLevelButtons();
            GUILayout.Label(_timeText);
            GUILayout.Label(_stateText);
            GUILayout.Label(_blocksText);

            if (GUILayout.Button(RestartLabel))
            {
                _signalBus.Fire(new RestartRequestedSignal());
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUI.matrix = Matrix4x4.identity;
        }

        private void DrawLevelButtons()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(PreviousLabel))
            {
                SelectLevel(-1);
            }

            if (GUILayout.Button(NextLabel))
            {
                SelectLevel(1);
            }

            GUILayout.EndHorizontal();
        }

        private void SelectLevel(int step)
        {
            int count = _library.Levels.Count;
            _selection.Level = _library.Levels[(_library.IndexOf(_level) + step + count) % count];
            _signalBus.Fire(new RestartRequestedSignal());
        }

        private void HandleSecondsChanged(int seconds)
        {
            _timeText = TimePrefix + new string(_timeBuffer, 0, ClockText.Write(seconds, _timeBuffer));
        }

        private void HandleStateChanged(LevelState state)
        {
            _stateText = StatePrefix + state;
        }

        private void HandleBlockExited(BlockModel block)
        {
            _blocksText = BlocksPrefix + _session.Board.Blocks.Count;
        }
    }
}
