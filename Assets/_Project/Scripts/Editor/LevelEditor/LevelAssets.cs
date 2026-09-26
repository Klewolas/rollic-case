using System.Collections.Generic;
using System.Text.RegularExpressions;
using RollicCase.Gameplay.Data;
using UnityEditor;
using UnityEngine;

namespace RollicCase.Editor.LevelEditor
{
    /// <summary>Finds and creates the assets the level editor works with.</summary>
    public static class LevelAssets
    {
        private const string LevelsFolder = "Assets/_Project/ScriptableObjects/Levels";
        private const string LevelNameFormat = "SO_Level_{0:000}";

        private static readonly Regex s_levelNumber = new Regex(@"^SO_Level_(\d+)$");

        /// <summary>Returns every asset of the type in the project, sorted by name.</summary>
        public static List<T> FindAll<T>() where T : Object
        {
            var assets = new List<T>();

            foreach (string guid in AssetDatabase.FindAssets("t:" + typeof(T).Name))
            {
                assets.Add(AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)));
            }

            assets.Sort((first, second) => string.CompareOrdinal(first.name, second.name));
            return assets;
        }

        /// <summary>Asks where to save and creates an empty level there; returns null when the designer cancels.</summary>
        public static LevelData CreateLevel(int width, int height, int timerSeconds)
        {
            string path = AskForPath();

            if (path == null)
            {
                return null;
            }

            var level = ScriptableObject.CreateInstance<LevelData>();
            level.SetSize(width, height);
            level.SetTimer(timerSeconds);
            return Save(level, path);
        }

        /// <summary>Asks where to save and stores a copy of the level there; returns null when the designer cancels.</summary>
        public static LevelData SaveCopy(LevelData source)
        {
            string path = AskForPath();
            return path == null ? null : Save(Object.Instantiate(source), path);
        }

        private static string AskForPath()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Level", GetNextLevelName(), "asset", "Choose where to save the level.", LevelsFolder);
            return string.IsNullOrEmpty(path) ? null : path;
        }

        private static LevelData Save(LevelData level, string path)
        {
            AssetDatabase.CreateAsset(level, path);
            AssetDatabase.SaveAssets();
            return level;
        }

        private static string GetNextLevelName()
        {
            int highest = 0;

            foreach (LevelData level in FindAll<LevelData>())
            {
                Match match = s_levelNumber.Match(level.name);

                if (match.Success)
                {
                    highest = Mathf.Max(highest, int.Parse(match.Groups[1].Value));
                }
            }

            return string.Format(LevelNameFormat, highest + 1);
        }
    }
}
