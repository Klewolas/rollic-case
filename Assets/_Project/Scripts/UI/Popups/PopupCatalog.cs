using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.UI.Popups
{
    /// <summary>Popup prefabs available in a scene, looked up by their view type.</summary>
    [CreateAssetMenu(fileName = "SO_PopupCatalog", menuName = "RollicCase/UI/Popup Catalog")]
    public sealed class PopupCatalog : ScriptableObject
    {
        [SerializeField] private List<PopupView> _prefabs = new List<PopupView>();

        /// <summary>Returns the prefab whose view is of the requested type.</summary>
        public PopupView GetPrefab<TPopup>() where TPopup : class, IPopup
        {
            for (int i = 0; i < _prefabs.Count; i++)
            {
                if (_prefabs[i] is TPopup)
                {
                    return _prefabs[i];
                }
            }

            throw new KeyNotFoundException($"{name} has no popup prefab of type {typeof(TPopup).Name}.");
        }
    }
}
