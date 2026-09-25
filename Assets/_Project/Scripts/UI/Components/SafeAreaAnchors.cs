using UnityEngine;

namespace RollicCase.UI.Components
{
    /// <summary>Converts the device safe area into normalized RectTransform anchors.</summary>
    public static class SafeAreaAnchors
    {
        /// <summary>Returns the anchors that fit a full-screen rect inside the safe area.</summary>
        public static void Calculate(Rect safeArea, Vector2 screenSize, out Vector2 anchorMin, out Vector2 anchorMax)
        {
            anchorMin = new Vector2(safeArea.xMin / screenSize.x, safeArea.yMin / screenSize.y);
            anchorMax = new Vector2(safeArea.xMax / screenSize.x, safeArea.yMax / screenSize.y);
        }
    }
}
