using RollicCase.Systems.SceneManagement;
using UnityEngine;
using Zenject;

namespace RollicCase.Systems
{
    /// <summary>Binds the global configuration assets.</summary>
    [CreateAssetMenu(fileName = "SO_ProjectConfigInstaller", menuName = "RollicCase/Installers/Project Config Installer")]
    public sealed class ProjectConfigInstaller : ScriptableObjectInstaller<ProjectConfigInstaller>
    {
        [SerializeField] private SceneConfig _sceneConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_sceneConfig);
        }
    }
}
