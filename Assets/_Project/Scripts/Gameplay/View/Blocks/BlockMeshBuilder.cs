using System.Collections.Generic;
using UnityEngine;

namespace RollicCase.Gameplay.View.Blocks
{
    /// <summary>Combines the modular pieces of a block shape into one mesh, so every block is a single renderer.</summary>
    public sealed class BlockMeshBuilder
    {
        private const string MeshName = "Block";

        private readonly BoardViewConfig _config;

        public BlockMeshBuilder(BoardViewConfig config)
        {
            _config = config;
        }

        /// <summary>Returns a new mesh in block-local space, where the block origin cell starts at zero.</summary>
        public Mesh Build(IReadOnlyList<Vector2Int> cells)
        {
            List<BlockPiece> pieces = BlockPieceLayout.Build(cells);
            var parts = new CombineInstance[pieces.Count];

            for (int i = 0; i < pieces.Count; i++)
            {
                BlockPiece piece = pieces[i];
                Matrix4x4 placement = Matrix4x4.TRS(BoardSpace.CellToWorld(piece.Position), Quaternion.Euler(0f, piece.Yaw, 0f), Vector3.one);
                Matrix4x4 pivot = piece.Type == BlockPieceType.InnerCorner ? Matrix4x4.identity : Matrix4x4.Translate(BoardSpace.QuadrantPivotOffset);

                parts[i] = new CombineInstance
                {
                    mesh = _config.GetPieceMesh(piece.Type),
                    transform = placement * pivot * Matrix4x4.Rotate(BoardSpace.BlockPieceUpright)
                };
            }

            var mesh = new Mesh { name = MeshName };
            mesh.CombineMeshes(parts, true, true);
            return mesh;
        }
    }
}
