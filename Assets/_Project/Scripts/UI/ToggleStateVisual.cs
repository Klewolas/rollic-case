using System;
using UnityEngine;

namespace RollicCase.UI
{
    /// <summary>How a switch looks in one state: its track, its knob, and the icon next to it.</summary>
    [Serializable]
    public sealed class ToggleStateVisual
    {
        [SerializeField] private Color _trackColor;
        [SerializeField] private Sprite _knob;
        [SerializeField] private Color _knobColor;
        [Tooltip("Tint of the setting's icon next to the switch.")]
        [SerializeField] private Color _iconColor;

        public Color TrackColor => _trackColor;
        public Sprite Knob => _knob;
        public Color KnobColor => _knobColor;
        public Color IconColor => _iconColor;
    }
}
