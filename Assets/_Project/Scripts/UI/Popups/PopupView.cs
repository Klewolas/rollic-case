using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RollicCase.UI.Popups
{
    /// <summary>Base view of every popup: fades and scales in and out and reports close requests.</summary>
    public abstract class PopupView : MonoBehaviour, IPopup
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _panel;
        [Tooltip("Optional. When assigned, tapping the dimmed background closes the popup.")]
        [SerializeField] private Button _dimmerButton;
        [Tooltip("Whether the Android back button closes the popup. Turn it off for popups that need a decision.")]
        [SerializeField] private bool _canDismiss;

        private PopupAnimationSettings _animation;
        private Sequence _transition;
        private TweenCallback _onHidden;

        public event Action<IPopup> CloseRequested;

        /// <summary>Raised after the hide animation has finished.</summary>
        public event Action Closed;

        public bool CanDismiss => _canDismiss;

        [Inject]
        public void Construct(UIConfig config)
        {
            _animation = config.Popup;
        }

        protected virtual void Awake()
        {
            _onHidden = HandleHidden;
            gameObject.SetActive(false);

            if (_dimmerButton != null)
            {
                _dimmerButton.onClick.AddListener(RequestClose);
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 0f;
            _panel.localScale = Vector3.one * _animation.HiddenScale;

            PlayTransition(1f, 1f, _animation.ShowDuration, _animation.ShowEase);
        }

        public void Close()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            PlayTransition(0f, _animation.HiddenScale, _animation.HideDuration, _animation.HideEase)
                .OnComplete(_onHidden);
        }

        /// <summary>Asks the popup service to close this popup.</summary>
        protected void RequestClose()
        {
            CloseRequested?.Invoke(this);
        }

        private Sequence PlayTransition(float alpha, float scale, float duration, Ease ease)
        {
            _transition?.Kill();
            _transition = DOTween.Sequence()
                .Join(_canvasGroup.DOFade(alpha, duration))
                .Join(_panel.DOScale(scale, duration).SetEase(ease))
                .SetUpdate(true)
                .SetLink(gameObject);

            return _transition;
        }

        private void HandleHidden()
        {
            gameObject.SetActive(false);
            Closed?.Invoke();
        }
    }
}
