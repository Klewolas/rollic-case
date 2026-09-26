using RollicCase.Gameplay.Data;
using UnityEditor;
using UnityEditor.Callbacks;

namespace RollicCase.Editor.LevelEditor
{
    /// <summary>Opens a level asset in the level editor when it is double-clicked.</summary>
    public static class LevelAssetOpener
    {
        [OnOpenAsset]
        public static bool Open(int instanceId, int line)
        {
            if (!(EditorUtility.InstanceIDToObject(instanceId) is LevelData level))
            {
                return false;
            }

            LevelEditorWindow.Open(level);
            return true;
        }
    }
}
