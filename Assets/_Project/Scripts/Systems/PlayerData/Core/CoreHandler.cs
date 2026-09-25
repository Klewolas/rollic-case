namespace RollicCase.Systems.PlayerData.Core
{
    /// <summary>Only writer of the core model.</summary>
    public sealed class CoreHandler : ICoreHandler
    {
        private readonly IModelProvider<CoreModel> _provider;

        public CoreHandler(IModelProvider<CoreModel> provider)
        {
            _provider = provider;
        }

        public int CurrentLevelNumber => _provider.Model.CompletedLevelCount + 1;

        public void CompleteCurrentLevel()
        {
            _provider.Model.CompletedLevelCount++;
            _provider.MarkDirty();
        }
    }
}
