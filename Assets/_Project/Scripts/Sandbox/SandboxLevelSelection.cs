using RollicCase.Gameplay.Data;
using UnityEditor;

namespace RollicCase.Sandbox
{
    /// <summary>Remembers which level Game_Sandbox plays, for the rest of the editor session.</summary>
    public sealed class SandboxLevelSelection
    {
        private const string SessionKey = "RollicCase.Sandbox.LevelPath";

        /// <summary>The chosen level, or null when none was chosen in this session.</summary>
        public LevelData Level
        {
            get => AssetDatabase.LoadAssetAtPath<LevelData>(SessionState.GetString(SessionKey, string.Empty));
            set => SessionState.SetString(SessionKey, AssetDatabase.GetAssetPath(value));
        }
    }
}
