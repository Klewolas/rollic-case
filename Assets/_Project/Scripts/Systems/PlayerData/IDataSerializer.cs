namespace RollicCase.Systems.PlayerData
{
    /// <summary>Converts player data models to text and back.</summary>
    public interface IDataSerializer
    {
        /// <summary>Converts the model to text.</summary>
        string Serialize<TModel>(TModel model);

        /// <summary>Converts text to a model and throws an ArgumentException when the text is invalid.</summary>
        TModel Deserialize<TModel>(string content);
    }
}
