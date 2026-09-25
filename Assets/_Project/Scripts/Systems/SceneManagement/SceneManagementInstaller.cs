using UnityEngine;
using Zenject;

namespace RollicCase.Systems.SceneManagement
{
    /// <summary>Binds the scene configuration and the scene loader.</summary>
    [CreateAssetMenu(fileName = "SO_SceneManagementInstaller", menuName = "RollicCase/Installers/Scene Management Installer")]
    public sealed class SceneManagementInstaller : ScriptableObjectInstaller<SceneManagementInstaller>
    {
        [SerializeField] private SceneConfig _sceneConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_sceneConfig);
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }
    }
}
