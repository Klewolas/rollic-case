using UnityEngine;

namespace RollicCase.Splash
{
    /// <summary>Tuning values for the splash screen.</summary>
    [CreateAssetMenu(fileName = "SO_SplashConfig", menuName = "RollicCase/Config/Splash Config")]
    public sealed class SplashConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _minimumDisplaySeconds;

        public float MinimumDisplaySeconds => _minimumDisplaySeconds;
    }
}
