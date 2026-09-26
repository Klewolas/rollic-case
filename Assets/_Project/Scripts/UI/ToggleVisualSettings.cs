using System;
using DG.Tweening;
using UnityEngine;

namespace RollicCase.UI
{
    /// <summary>How an on/off switch looks in each state and how its knob slides between them.</summary>
    [Serializable]
    public sealed class ToggleVisualSettings
    {
        [SerializeField] private ToggleStateVisual _on;
        [SerializeField] private ToggleStateVisual _off;
        [Tooltip("Seconds the knob takes to slide to the other side.")]
        [SerializeField, Min(0f)] private float _slideDuration;
        [SerializeField] private Ease _slideEase;

        public ToggleStateVisual On => _on;
        public ToggleStateVisual Off => _off;
        public float SlideDuration => _slideDuration;
        public Ease SlideEase => _slideEase;
    }
}
