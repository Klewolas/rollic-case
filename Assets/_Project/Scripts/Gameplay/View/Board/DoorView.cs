using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.View.Board
{
    /// <summary>Draws one door with its combined mesh, tinted in its color.</summary>
    public sealed class DoorView : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        private void OnDestroy()
        {
            Destroy(_meshFilter.sharedMesh);
        }

        /// <summary>Shows the door mesh with its color tint.</summary>
        public void Initialize(Mesh mesh, MaterialPropertyBlock tint)
        {
            _meshFilter.sharedMesh = mesh;
            _meshRenderer.SetPropertyBlock(tint);
        }

        /// <summary>Creates door views from the door prefab.</summary>
        public sealed class Factory : PlaceholderFactory<DoorView>
        {
        }
    }
}
