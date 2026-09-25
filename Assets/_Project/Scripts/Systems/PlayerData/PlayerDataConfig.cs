using UnityEngine;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Where and how player data files are stored.</summary>
    [CreateAssetMenu(fileName = "SO_PlayerDataConfig", menuName = "RollicCase/Config/Player Data Config")]
    public sealed class PlayerDataConfig : ScriptableObject
    {
        [SerializeField] private string _directoryName;
        [SerializeField] private string _temporaryFileSuffix;

        /// <summary>Folder under the persistent data path that holds the data files.</summary>
        public string DirectoryName => _directoryName;

        /// <summary>Suffix of the file that is written before it replaces the real file.</summary>
        public string TemporaryFileSuffix => _temporaryFileSuffix;
    }
}
