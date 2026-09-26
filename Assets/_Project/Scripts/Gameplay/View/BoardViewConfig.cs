using System;
using RollicCase.Gameplay.View.Blocks;
using UnityEngine;

namespace RollicCase.Gameplay.View
{
    /// <summary>Meshes, materials, and camera tuning used to draw the board.</summary>
    [CreateAssetMenu(fileName = "SO_BoardViewConfig", menuName = "RollicCase/Gameplay/Board View Config")]
    public sealed class BoardViewConfig : ScriptableObject
    {
        [Header("Block Pieces")]
        [SerializeField] private Mesh _centerPiece;
        [SerializeField] private Mesh _edgePiece;
        [SerializeField] private Mesh _outerCornerPiece;
        [SerializeField] private Mesh _innerCornerPiece;

        [Header("Board")]
        [SerializeField] private Mesh _groundTile;
        [SerializeField] private Mesh _wall;
        [SerializeField] private Mesh _corner;
        [SerializeField] private Mesh _door;
        [SerializeField] private Mesh _doorArrow;

        [Header("Ground")]
        [Tooltip("Color of the base under the ground tiles, seen through the gaps between them.")]
        [SerializeField] private Color _groundBaseColor;

        [Header("Doors")]
        [Tooltip("How strongly a door glows in its color.")]
        [SerializeField, Range(0f, 1f)] private float _doorGlow;

        [Header("Camera")]
        [Tooltip("Degrees the camera leans back from looking straight down.")]
        [SerializeField, Range(0f, 45f)] private float _cameraTilt;
        [Tooltip("1 fits the board edge to edge; larger values leave more space around it.")]
        [SerializeField, Min(1f)] private float _cameraPadding;

        public Mesh GroundTile => _groundTile;
        public Mesh Wall => _wall;
        public Mesh Corner => _corner;
        public Mesh Door => _door;
        public Mesh DoorArrow => _doorArrow;
        public Color GroundBaseColor => _groundBaseColor;
        public float DoorGlow => _doorGlow;
        public float CameraTilt => _cameraTilt;
        public float CameraPadding => _cameraPadding;

        /// <summary>Returns the mesh for the block piece type.</summary>
        public Mesh GetPieceMesh(BlockPieceType type)
        {
            switch (type)
            {
                case BlockPieceType.Center: return _centerPiece;
                case BlockPieceType.Edge: return _edgePiece;
                case BlockPieceType.OuterCorner: return _outerCornerPiece;
                case BlockPieceType.InnerCorner: return _innerCornerPiece;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
