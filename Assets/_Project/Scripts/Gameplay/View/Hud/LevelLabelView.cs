using RollicCase.Gameplay.Flow;
using TMPro;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.View.Hud
{
    /// <summary>Shows the number of the level being played.</summary>
    public sealed class LevelLabelView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [Tooltip("The label text; {0} is replaced by the level number.")]
        [SerializeField] private string _format;

        [Inject]
        public void Construct(ILevelProvider levelProvider)
        {
            _label.SetText(_format, levelProvider.CurrentLevelNumber);
        }
    }
}
