using UnityEngine;
using Zenject;

namespace RollicCase.Systems.SceneManagement
{
    /// <summary>Binds the scene loader.</summary>
    [CreateAssetMenu(fileName = "SO_SceneManagementInstaller", menuName = "RollicCase/Installers/Scene Management Installer")]
    public sealed class SceneManagementInstaller : ScriptableObjectInstaller<SceneManagementInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }
    }
}
