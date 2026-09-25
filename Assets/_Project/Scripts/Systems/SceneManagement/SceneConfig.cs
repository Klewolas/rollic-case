using UnityEngine;

namespace RollicCase.Systems.SceneManagement
{
    /// <summary>Scene names used by the scene flow.</summary>
    [CreateAssetMenu(fileName = "SO_SceneConfig", menuName = "RollicCase/Config/Scene Config")]
    public sealed class SceneConfig : ScriptableObject
    {
        [SerializeField] private string _splashScene;
        [SerializeField] private string _mapScene;
        [SerializeField] private string _gameScene;

        public string SplashScene => _splashScene;
        public string MapScene => _mapScene;
        public string GameScene => _gameScene;
    }
}
