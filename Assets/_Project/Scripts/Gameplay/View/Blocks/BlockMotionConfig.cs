using DG.Tweening;
using UnityEngine;

namespace RollicCase.Gameplay.View.Blocks
{
    /// <summary>Tuning of how blocks react to dragging: the exit push, the grab highlight, the snap on release, and the exit animation.</summary>
    [CreateAssetMenu(fileName = "SO_BlockMotionConfig", menuName = "RollicCase/Gameplay/Block Motion Config")]
    public sealed class BlockMotionConfig : ScriptableObject
    {
        [Header("Drag")]
        [Tooltip("How far, in cells, the finger must push a block past its door before the block leaves.")]
        [SerializeField, Range(0.1f, 1f)] private float _exitThreshold;
        [Tooltip("How much a held block brightens toward white, from 0 (not at all) to 1 (white).")]
        [SerializeField, Range(0f, 1f)] private float _grabHighlight;

        [Header("Snap")]
        [Tooltip("Seconds a released block takes to settle into its cell.")]
        [SerializeField, Min(0f)] private float _snapDuration;
        [SerializeField] private Ease _snapEase;

        [Header("Exit")]
        [Tooltip("Seconds a block takes to slide into its door and disappear.")]
        [SerializeField, Min(0f)] private float _exitDuration;
        [SerializeField] private Ease _exitEase;

        public float ExitThreshold => _exitThreshold;
        public float GrabHighlight => _grabHighlight;
        public float SnapDuration => _snapDuration;
        public Ease SnapEase => _snapEase;
        public float ExitDuration => _exitDuration;
        public Ease ExitEase => _exitEase;
    }
}
