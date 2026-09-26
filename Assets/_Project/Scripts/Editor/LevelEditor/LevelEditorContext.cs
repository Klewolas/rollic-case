using System;
using RollicCase.Gameplay.Data;
using RollicCase.Editor.LevelEditor.Editing;
using UnityEditor;

namespace RollicCase.Editor.LevelEditor
{
    /// <summary>Shared editor state: the edited level, the palette selection, and the selected block.</summary>
    public sealed class LevelEditorContext
    {
        public LevelEditorContext(LevelEditing editing)
        {
            Editing = editing;
        }

        /// <summary>Raised after an edit changed the level.</summary>
        public event Action LevelChanged;

        public LevelEditing Editing { get; }
        public LevelData Level { get; set; }
        public BlockColor SelectedColor { get; set; }
        public BlockShape SelectedShape { get; set; }
        public int SelectedBlock { get; set; } = LevelEditing.None;

        /// <summary>Runs the edit as one undoable step and reports whether it changed the level.</summary>
        public bool Apply(string actionName, Func<bool> edit)
        {
            Undo.RecordObject(Level, actionName);

            if (!edit())
            {
                return false;
            }

            EditorUtility.SetDirty(Level);
            LevelChanged?.Invoke();
            return true;
        }
    }
}
