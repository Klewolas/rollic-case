using RollicCase.Gameplay.Data;
using RollicCase.Sandbox;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RollicCase.Editor.LevelEditor
{
    /// <summary>Opens Game_Sandbox and plays the given level in it.</summary>
    public static class SandboxLauncher
    {
        private const string ScenePath = "Assets/_Project/Scenes/Game_Sandbox.unity";

        /// <summary>Asks to save open scene changes, then enters Play mode in Game_Sandbox with the level.</summary>
        public static void Play(LevelData level)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            new SandboxLevelSelection().Level = level;
            EditorSceneManager.OpenScene(ScenePath);
            EditorApplication.EnterPlaymode();
        }
    }
}
