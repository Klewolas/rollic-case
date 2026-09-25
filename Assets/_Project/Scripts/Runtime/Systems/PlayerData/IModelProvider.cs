namespace RollicCase.Systems.PlayerData
{
    /// <summary>Gives handlers access to one player data model and saves it after changes.</summary>
    public interface IModelProvider<out TModel> where TModel : class
    {
        /// <summary>The loaded model. It is loaded synchronously on first access if it was not loaded before.</summary>
        TModel Model { get; }

        /// <summary>Marks the model as changed and schedules a save.</summary>
        void MarkDirty();
    }
}
