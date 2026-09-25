using System;
using DG.Tweening;
using UnityEngine;

namespace RollicCase.UI
{
    /// <summary>Tuning of the popup show and hide animations.</summary>
    [Serializable]
    public sealed class PopupAnimationSettings
    {
        [SerializeField, Min(0f)] private float _showDuration;
        [SerializeField, Min(0f)] private float _hideDuration;
        [SerializeField, Min(0f)] private float _hiddenScale;
        [SerializeField] private Ease _showEase;
        [SerializeField] private Ease _hideEase;

        public float ShowDuration => _showDuration;
        public float HideDuration => _hideDuration;

        /// <summary>Panel scale at the start of the show and the end of the hide animation.</summary>
        public float HiddenScale => _hiddenScale;
        public Ease ShowEase => _showEase;
        public Ease HideEase => _hideEase;
    }
}
