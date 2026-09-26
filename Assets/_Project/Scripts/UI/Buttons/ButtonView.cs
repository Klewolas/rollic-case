using UnityEngine;
using UnityEngine.UI;

namespace RollicCase.UI.Buttons
{
    /// <summary>Base of every button that has a function: it wires its own Button once and reacts to clicks.</summary>
    [RequireComponent(typeof(Button))]
    public abstract class ButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        protected virtual void Awake()
        {
            _button.onClick.AddListener(HandleClick);
        }

        private void Reset()
        {
            CacheButton();
        }

        private void OnValidate()
        {
            CacheButton();
        }

        /// <summary>Called when the button is clicked.</summary>
        protected abstract void HandleClick();

        private void CacheButton()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }
    }
}
