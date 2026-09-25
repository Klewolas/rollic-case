using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace RollicCase.UI.Components
{
    /// <summary>Scales the target down while an interactable selectable is pressed; one reused tween, no allocations per press.</summary>
    public sealed class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Selectable _selectable;
        [SerializeField] private Transform _target;

        private ButtonFeedbackSettings _settings;
        private Tween _pressTween;

        [Inject]
        public void Construct(UIConfig config)
        {
            _settings = config.Button;
        }

        private void OnDisable()
        {
            _pressTween?.Rewind();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_selectable.IsInteractable())
            {
                return;
            }

            _pressTween ??= CreatePressTween();
            _pressTween.PlayForward();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressTween?.PlayBackwards();
        }

        private Tween CreatePressTween()
        {
            return _target.DOScale(_settings.PressedScale, _settings.PressDuration)
                .SetEase(_settings.PressEase)
                .SetAutoKill(false)
                .SetUpdate(true)
                .SetLink(gameObject)
                .Pause();
        }
    }
}
