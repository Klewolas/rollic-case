using System;
using Zenject;

namespace RollicCase.Systems.PlayerData
{
    /// <summary>Binding shortcuts for player data.</summary>
    public static class PlayerDataContainerExtensions
    {
        /// <summary>Binds a stored player data model that is saved under the given file name.</summary>
        public static void BindPersistentModel<TModel>(this DiContainer container, string fileName, Func<TModel> createDefault)
            where TModel : class
        {
            container.BindInterfacesTo<ModelProvider<TModel>>().AsSingle().WithArguments(fileName, createDefault);
        }
    }
}
