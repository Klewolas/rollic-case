using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.View.Board
{
    /// <summary>Draws one door with its combined mesh, tinted in its color, and the arrows that mark its exit direction.</summary>
    public sealed class DoorView : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _arrows;

        private void OnDestroy()
        {
            Destroy(_meshFilter.sharedMesh);
            Destroy(_arrows.sharedMesh);
        }

        /// <summary>Shows the door mesh with its color tint, and its arrows.</summary>
        public void Initialize(Mesh mesh, Mesh arrows, MaterialPropertyBlock tint)
        {
            _meshFilter.sharedMesh = mesh;
            _arrows.sharedMesh = arrows;
            _meshRenderer.SetPropertyBlock(tint);
        }

        /// <summary>Creates door views from the door prefab.</summary>
        public sealed class Factory : PlaceholderFactory<DoorView>
        {
        }
    }
}
