using UnityEngine;

namespace RollicCase.UI.Popups
{
    /// <summary>Marks the popup canvas and the transform popups are created under.</summary>
    public sealed class PopupLayer : MonoBehaviour
    {
        [SerializeField] private Transform _content;

        public Transform Content => _content;
    }
}
