using RollicCase.Systems.PlayerData;

namespace RollicCase.Tests.Fakes
{
    /// <summary>Provider that holds a model in memory and counts dirty marks.</summary>
    public sealed class FakeModelProvider<TModel> : IModelProvider<TModel> where TModel : class
    {
        public FakeModelProvider(TModel model)
        {
            Model = model;
        }

        public TModel Model { get; }
        public int DirtyCount { get; private set; }

        public void MarkDirty()
        {
            DirtyCount++;
        }
    }
}
