using RollicCase.Systems.PlayerData.Core;
using TMPro;
using UnityEngine;
using Zenject;

namespace RollicCase.UI.Components
{
    /// <summary>Shows the number of the level the player plays next.</summary>
    public sealed class LevelNumberView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [Tooltip("The label text; {0} is replaced by the level number.")]
        [SerializeField] private string _format;

        [Inject]
        public void Construct(ICoreHandler core)
        {
            _label.SetText(_format, core.CurrentLevelNumber);
        }
    }
}
