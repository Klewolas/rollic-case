using RollicCase.Systems.SceneManagement;
using Zenject;

namespace RollicCase.Systems
{
    /// <summary>Binds the global services that live across scenes.</summary>
    public sealed class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
        }
    }
}
