using UnityEngine;

namespace RollicCase.Gameplay.View.Cameras
{
    /// <summary>Camera distance math that keeps a board of any size fully visible.</summary>
    public static class CameraFraming
    {
        /// <summary>Returns the distance at which a view of the given half extents fits the screen with the padding factor.</summary>
        public static float GetDistance(float halfWidth, float halfHeight, float aspect, float verticalFieldOfView, float padding)
        {
            float tangent = Mathf.Tan(verticalFieldOfView * 0.5f * Mathf.Deg2Rad);
            float heightDistance = halfHeight / tangent;
            float widthDistance = halfWidth / (tangent * aspect);
            return Mathf.Max(heightDistance, widthDistance) * padding;
        }
    }
}
