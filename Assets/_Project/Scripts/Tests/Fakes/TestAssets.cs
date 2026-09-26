using System.Collections.Generic;
using RollicCase.Gameplay.Data;
using RollicCase.Systems.PlayerData.Wallet;
using UnityEditor;
using UnityEngine;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Creates configured ScriptableObjects for tests and destroys them afterwards.</summary>
    public sealed class TestAssets
    {
        private readonly List<ScriptableObject> _created = new List<ScriptableObject>();

        /// <summary>Creates a consumable; a negative capacity means the item has no capacity.</summary>
        public ConsumableDefinition CreateConsumable(string id, int initialAmount, int capacity = -1)
        {
            var item = Create<ConsumableDefinition>();
            var serialized = new SerializedObject(item);
            serialized.FindProperty("_id").stringValue = id;
            serialized.FindProperty("_initialAmount").intValue = initialAmount;
            serialized.FindProperty("_hasCapacity").boolValue = capacity >= 0;
            serialized.FindProperty("_capacity").intValue = Mathf.Max(0, capacity);
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return item;
        }

        public ConsumableCatalog CreateCatalog(params ConsumableDefinition[] items)
        {
            var catalog = Create<ConsumableCatalog>();
            var serialized = new SerializedObject(catalog);
            SerializedProperty list = serialized.FindProperty("_items");
            list.arraySize = items.Length;

            for (int i = 0; i < items.Length; i++)
            {
                list.GetArrayElementAtIndex(i).objectReferenceValue = items[i];
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            return catalog;
        }

        public BlockColor CreateBlockColor()
        {
            return Create<BlockColor>();
        }

        public LevelData CreateLevel(int width, int height, int timerSeconds)
        {
            var level = Create<LevelData>();
            level.SetSize(width, height);
            level.SetTimer(timerSeconds);
            return level;
        }

        public LevelCatalog CreateLevelCatalog(params LevelData[] levels)
        {
            var catalog = Create<LevelCatalog>();

            foreach (LevelData level in levels)
            {
                catalog.Add(level);
            }

            return catalog;
        }

        public void DestroyAll()
        {
            foreach (ScriptableObject asset in _created)
            {
                Object.DestroyImmediate(asset);
            }

            _created.Clear();
        }

        private T Create<T>() where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            _created.Add(asset);
            return asset;
        }
    }
}
