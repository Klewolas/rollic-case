using System;
using RollicCase.Gameplay.Data;
using RollicCase.Gameplay.Flow.Signals;
using Zenject;

namespace RollicCase.Sandbox
{
    /// <summary>Binds Game_Sandbox next to the shared gameplay installer: the chosen level, the dev overlay, and restart. No player data, rewards, or popups.</summary>
    public sealed class SandboxInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SandboxLevelSelection>().AsSingle();
            Container.Bind<SandboxLevelLibrary>().AsSingle();
            Container.Bind<LevelData>().FromMethod(ResolveLevel).AsSingle();

            Container.DeclareSignal<RestartRequestedSignal>();
            Container.BindInterfacesTo<SandboxSceneReloader>().AsSingle();

            Container.Bind<GuiRenderableManager>().AsSingle();
            Container.Bind<GuiRenderer>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.BindInterfacesTo<SandboxPanel>().AsSingle();
        }

        private static LevelData ResolveLevel(InjectContext context)
        {
            LevelData selected = context.Container.Resolve<SandboxLevelSelection>().Level;

            if (selected != null)
            {
                return selected;
            }

            SandboxLevelLibrary library = context.Container.Resolve<SandboxLevelLibrary>();

            if (library.Levels.Count == 0)
            {
                throw new InvalidOperationException("There is no level to play. Create a level in Tools > Level Editor first.");
            }

            return library.Levels[0];
        }
    }
}
