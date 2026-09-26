using System;
using RollicCase.Gameplay.Logic;

namespace RollicCase.Gameplay.Data
{
    /// <summary>Saved settings of one block feature, such as ice; each feature type derives its own data class.</summary>
    [Serializable]
    public abstract class BlockFeatureData
    {
        /// <summary>Creates the runtime feature with these settings for one block.</summary>
        public abstract BlockFeature CreateFeature();
    }
}
