using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Logic;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Feature settings that create an exit listener.</summary>
    public sealed class FakeFeatureData : BlockFeatureData
    {
        public override BlockFeature CreateFeature()
        {
            return new FakeExitListener();
        }
    }
}
