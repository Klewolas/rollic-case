using DG.Tweening;
using RollicCase.Systems.PlayerData.Settings;
using RollicCase.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RollicCase.Meta.Settings
{
    /// <summary>Shows the toggle's setting as a switch: the knob slides to the right when on and to the left when off.</summary>
    [RequireComponent(typeof(SettingToggleButton))]
    public sealed class SettingToggleView : MonoBehaviour
    {
        [SerializeField] private SettingToggleButton _button;
        [SerializeField] private Image _track;
        [SerializeField] private Image _knob;
        [Tooltip("The icon of the setting next to the switch; dimmed while the setting is off.")]
        [SerializeField] private Image _icon;

        private ISettingsHandler _settings;
        private ToggleVisualSettings _visuals;
        private Tween _slide;

        [Inject]
        public void Construct(ISettingsHandler settings, UIConfig config)
        {
            _settings = settings;
            _visuals = config.Toggle;
            _settings.SettingChanged += HandleSettingChanged;

            bool isEnabled = _settings.IsEnabled(_button.Setting);
            Show(isEnabled);
            _knob.rectTransform.anchoredPosition = new Vector2(GetKnobX(isEnabled), _knob.rectTransform.anchoredPosition.y);
        }

        private void OnDestroy()
        {
            if (_settings != null)
            {
                _settings.SettingChanged -= HandleSettingChanged;
            }
        }

        private void HandleSettingChanged(SettingType setting, bool isEnabled)
        {
            if (setting != _button.Setting)
            {
                return;
            }

            Show(isEnabled);
            _slide?.Kill();
            _slide = _knob.rectTransform.DOAnchorPosX(GetKnobX(isEnabled), _visuals.SlideDuration)
                .SetEase(_visuals.SlideEase)
                .SetUpdate(true)
                .SetLink(gameObject);
        }

        private void Show(bool isEnabled)
        {
            ToggleStateVisual state = isEnabled ? _visuals.On : _visuals.Off;
            _track.color = state.TrackColor;
            _knob.sprite = state.Knob;
            _knob.color = state.KnobColor;
            _icon.color = state.IconColor;
        }

        private float GetKnobX(bool isEnabled)
        {
            float travel = (_track.rectTransform.rect.width - _knob.rectTransform.rect.width) * 0.5f;
            return isEnabled ? travel : -travel;
        }
    }
}
