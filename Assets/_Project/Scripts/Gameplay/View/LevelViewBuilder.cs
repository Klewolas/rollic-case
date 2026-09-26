using System;
using RollicCase.Gameplay.Logic;
using RollicCase.Gameplay.View.Blocks;
using RollicCase.Gameplay.View.Board;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.View
{
    /// <summary>Fills the scene's board with the level: ground and rim meshes, then a door and block view for each door and block.</summary>
    public sealed class LevelViewBuilder : IInitializable, IDisposable
    {
        private readonly BoardModel _board;
        private readonly BoardRootView _root;
        private readonly BoardViewConfig _config;
        private readonly BoardMeshBuilder _boardMeshes;
        private readonly BlockMeshBuilder _blockMeshes;
        private readonly DoorView.Factory _doorFactory;
        private readonly BlockView.Factory _blockFactory;
        private readonly MaterialPropertyBlock _tint = new MaterialPropertyBlock();

        private Mesh _groundMesh;
        private Mesh _rimMesh;

        public LevelViewBuilder(BoardModel board, BoardRootView root, BoardViewConfig config, BoardMeshBuilder boardMeshes,
            BlockMeshBuilder blockMeshes, DoorView.Factory doorFactory, BlockView.Factory blockFactory)
        {
            _board = board;
            _root = root;
            _config = config;
            _boardMeshes = boardMeshes;
            _blockMeshes = blockMeshes;
            _doorFactory = doorFactory;
            _blockFactory = blockFactory;
        }

        public void Initialize()
        {
            _groundMesh = _boardMeshes.BuildGround(_board.Width, _board.Height);
            _rimMesh = _boardMeshes.BuildRim(_board);
            _root.Ground.sharedMesh = _groundMesh;
            _root.Rim.sharedMesh = _rimMesh;

            foreach (DoorModel door in _board.Doors)
            {
                DoorView view = _doorFactory.Create();
                view.transform.SetParent(_root.Doors, false);
                view.Initialize(_boardMeshes.BuildDoor(_board, door), _boardMeshes.BuildDoorArrows(_board, door), DoorTint(door));
            }

            foreach (BlockModel block in _board.Blocks)
            {
                BlockView view = _blockFactory.Create();
                view.transform.SetParent(_root.Blocks, false);
                view.Initialize(block, _blockMeshes.Build(block.Cells), BlockTint(block));
            }
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(_groundMesh);
            UnityEngine.Object.Destroy(_rimMesh);
        }

        private MaterialPropertyBlock BlockTint(BlockModel block)
        {
            _tint.Clear();
            _tint.SetColor(ShaderProperties.BaseColor, block.Color.DisplayColor);
            return _tint;
        }

        private MaterialPropertyBlock DoorTint(DoorModel door)
        {
            _tint.Clear();
            _tint.SetColor(ShaderProperties.BaseColor, door.Color.DisplayColor);
            _tint.SetColor(ShaderProperties.EmissionColor, door.Color.DisplayColor * _config.DoorGlow);
            return _tint;
        }
    }
}
