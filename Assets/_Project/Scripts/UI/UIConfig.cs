using UnityEngine;

namespace RollicCase.UI
{
    /// <summary>Animation and visual tuning shared by all UI.</summary>
    [CreateAssetMenu(fileName = "SO_UIConfig", menuName = "RollicCase/Config/UI Config")]
    public sealed class UIConfig : ScriptableObject
    {
        [SerializeField] private PopupAnimationSettings _popup;
        [SerializeField] private ButtonFeedbackSettings _button;
        [SerializeField] private ToggleVisualSettings _toggle;

        public PopupAnimationSettings Popup => _popup;
        public ButtonFeedbackSettings Button => _button;
        public ToggleVisualSettings Toggle => _toggle;
    }
}
