using System;
using DG.Tweening;
using UnityEngine;

namespace RollicCase.UI
{
    /// <summary>Tuning of the press feedback shared by all buttons.</summary>
    [Serializable]
    public sealed class ButtonFeedbackSettings
    {
        [SerializeField, Min(0f)] private float _pressedScale;
        [SerializeField, Min(0f)] private float _pressDuration;
        [SerializeField] private Ease _pressEase;

        public float PressedScale => _pressedScale;
        public float PressDuration => _pressDuration;
        public Ease PressEase => _pressEase;
    }
}
