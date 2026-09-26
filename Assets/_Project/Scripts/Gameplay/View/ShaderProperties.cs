using UnityEngine;

namespace RollicCase.Gameplay.View
{
    /// <summary>Cached IDs of the URP Lit properties tinted per renderer.</summary>
    public static class ShaderProperties
    {
        public static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        public static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    }
}
