using RollicCase.Gameplay.Logic;
using UnityEngine;
using Zenject;

namespace RollicCase.Gameplay.View.Cameras
{
    /// <summary>Places the camera so the whole board, rim included, is centered and visible on any aspect ratio.</summary>
    public sealed class CameraFramer : IInitializable
    {
        private const float StraightDown = 90f;

        private readonly Camera _camera;
        private readonly BoardModel _board;
        private readonly BoardViewConfig _config;

        public CameraFramer(Camera camera, BoardModel board, BoardViewConfig config)
        {
            _camera = camera;
            _board = board;
            _config = config;
        }

        public void Initialize()
        {
            float halfWidth = _board.Width * BoardSpace.CellSize * 0.5f;
            float halfHeight = _board.Height * BoardSpace.CellSize * 0.5f;
            var center = new Vector3(halfWidth, 0f, halfHeight);

            float distance = CameraFraming.GetDistance(halfWidth + BoardSpace.RimWidth, halfHeight + BoardSpace.RimWidth,
                _camera.aspect, _camera.fieldOfView, _config.CameraPadding);

            Quaternion rotation = Quaternion.Euler(StraightDown - _config.CameraTilt, 0f, 0f);
            _camera.transform.SetPositionAndRotation(center - rotation * Vector3.forward * distance, rotation);
        }
    }
}
