using UnityEngine;

namespace RollicCase.UI.Components
{
    /// <summary>Fits this RectTransform inside the device safe area, away from notches and system bars.</summary>
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeArea : MonoBehaviour
    {
        private void Awake()
        {
            var rectTransform = (RectTransform)transform;
            SafeAreaAnchors.Calculate(Screen.safeArea, new Vector2(Screen.width, Screen.height), out Vector2 anchorMin, out Vector2 anchorMax);
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}
