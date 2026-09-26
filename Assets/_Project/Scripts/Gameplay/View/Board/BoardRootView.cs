using UnityEngine;

namespace RollicCase.Gameplay.View.Board
{
    /// <summary>The board placed in the scene: its ground, ground base, and rim renderers and the parents for doors and blocks.</summary>
    public sealed class BoardRootView : MonoBehaviour
    {
        [SerializeField] private MeshFilter _ground;
        [Tooltip("One ground tile stretched under the whole board, seen through the gaps between the tiles.")]
        [SerializeField] private MeshRenderer _groundBase;
        [SerializeField] private MeshFilter _rim;
        [SerializeField] private Transform _doors;
        [SerializeField] private Transform _blocks;

        public MeshFilter Ground => _ground;
        public MeshRenderer GroundBase => _groundBase;
        public MeshFilter Rim => _rim;
        public Transform Doors => _doors;
        public Transform Blocks => _blocks;
    }
}
