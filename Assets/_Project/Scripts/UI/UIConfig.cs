using UnityEngine;

namespace RollicCase.UI
{
    /// <summary>Animation tuning shared by all UI.</summary>
    [CreateAssetMenu(fileName = "SO_UIConfig", menuName = "RollicCase/Config/UI Config")]
    public sealed class UIConfig : ScriptableObject
    {
        [SerializeField] private PopupAnimationSettings _popup;
        [SerializeField] private ButtonFeedbackSettings _button;

        public PopupAnimationSettings Popup => _popup;
        public ButtonFeedbackSettings Button => _button;
    }
}
