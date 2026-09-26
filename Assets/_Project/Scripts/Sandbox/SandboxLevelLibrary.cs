using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using UnityEditor;
using UnityEngine;

namespace RollicCase.Sandbox
{
    /// <summary>Every level asset in the project sorted by name, so levels can be tested before they are in the catalog.</summary>
    public sealed class SandboxLevelLibrary
    {
        private readonly List<LevelData> _levels = new List<LevelData>();

        public SandboxLevelLibrary()
        {
            foreach (string guid in AssetDatabase.FindAssets("t:" + nameof(LevelData)))
            {
                _levels.Add(AssetDatabase.LoadAssetAtPath<LevelData>(AssetDatabase.GUIDToAssetPath(guid)));
            }

            _levels.Sort((first, second) => string.CompareOrdinal(first.name, second.name));
        }

        public IReadOnlyList<LevelData> Levels => _levels;

        /// <summary>Returns the position of the level in the list, or 0 when it is not in the list.</summary>
        public int IndexOf(LevelData level)
        {
            return Mathf.Max(0, _levels.IndexOf(level));
        }
    }
}
