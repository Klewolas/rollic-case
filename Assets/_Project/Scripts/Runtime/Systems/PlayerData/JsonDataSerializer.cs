using UnityEngine;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Serializes player data models as JSON.</summary>
    public sealed class JsonDataSerializer : IDataSerializer
    {
        public string Serialize<TModel>(TModel model)
        {
            return JsonUtility.ToJson(model);
        }

        public TModel Deserialize<TModel>(string content)
        {
            return JsonUtility.FromJson<TModel>(content);
        }
    }
}
