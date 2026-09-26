using RollicCase.Gameplay.Logic;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.View.Blocks
{
    /// <summary>Draws one block and is the pointer target for dragging it.</summary>
    public sealed class BlockView : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshCollider _meshCollider;

        public BlockModel Model { get; private set; }

        private void OnDestroy()
        {
            Destroy(_meshFilter.sharedMesh);
        }

        /// <summary>Shows the block with its own combined mesh, tinted in its color, at its board position.</summary>
        public void Initialize(BlockModel model, Mesh mesh, MaterialPropertyBlock tint)
        {
            Model = model;
            name = model.Color.name;
            _meshFilter.sharedMesh = mesh;
            _meshCollider.sharedMesh = mesh;
            _meshRenderer.SetPropertyBlock(tint);
            transform.localPosition = BoardSpace.CellToWorld(model.Position);
        }

        /// <summary>Creates block views from the block prefab.</summary>
        public sealed class Factory : PlaceholderFactory<BlockView>
        {
        }
    }
}
