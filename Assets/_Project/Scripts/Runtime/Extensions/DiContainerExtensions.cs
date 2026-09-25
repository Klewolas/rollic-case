using System;
using RollicCase.Systems.PlayerData;
using Zenject;

namespace RollicCase.Extensions
{
    /// <summary>Binding shortcuts for the DI container.</summary>
    public static class DiContainerExtensions
    {
        /// <summary>Binds a stored player data model that is saved under the given file name.</summary>
        public static void BindPersistentModel<TModel>(this DiContainer container, string fileName, Func<TModel> createDefault)
            where TModel : class
        {
            container.BindInterfacesTo<ModelProvider<TModel>>().AsSingle().WithArguments(fileName, createDefault);
        }
    }
}
