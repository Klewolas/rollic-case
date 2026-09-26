using DG.Tweening;
using RollicCase.Gameplay.Logic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace RollicCase.Gameplay.View.Blocks
{
    /// <summary>Draws one block, forwards the pointer input on it, and animates its snap and exit.</summary>
    public sealed class BlockView : MonoBehaviour, IPointerDownHandler, IInitializePotentialDragHandler, IDragHandler,
        IPointerUpHandler
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshCollider _meshCollider;

        private IBlockDragHandler _dragHandler;
        private BlockMotionConfig _motion;
        private Tween _motionTween;
        private TweenCallback _onExited;

        public BlockModel Model { get; private set; }

        [Inject]
        public void Construct(IBlockDragHandler dragHandler, BlockMotionConfig motion)
        {
            _dragHandler = dragHandler;
            _motion = motion;
        }

        private void Awake()
        {
            _onExited = HandleExited;
        }

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

        /// <summary>Places the block at a continuous board position in cells.</summary>
        public void MoveTo(Vector2 cell)
        {
            _motionTween?.Kill();
            transform.localPosition = BoardSpace.CellToWorld(cell);
        }

        /// <summary>Settles the block into the cell.</summary>
        public void SnapTo(Vector2Int cell)
        {
            _motionTween?.Kill();
            _motionTween = transform.DOLocalMove(BoardSpace.CellToWorld(cell), _motion.SnapDuration)
                .SetEase(_motion.SnapEase)
                .SetLink(gameObject);
        }

        /// <summary>Slides the block into its door while squeezing it flat against the door, then hides it.</summary>
        public void PlayExit(Vector3 doorPosition, Vector3 squeezedScale)
        {
            _meshCollider.enabled = false;
            _motionTween?.Kill();
            _motionTween = DOTween.Sequence()
                .Join(transform.DOLocalMove(doorPosition, _motion.ExitDuration).SetEase(Ease.Linear))
                .Join(transform.DOScale(squeezedScale, _motion.ExitDuration).SetEase(Ease.Linear))
                .SetEase(_motion.ExitEase)
                .SetLink(gameObject)
                .OnComplete(_onExited);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            _dragHandler.HandlePressed(this, eventData.pointerCurrentRaycast.worldPosition);
        }

        void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            _dragHandler.HandleDragged(this, eventData.position);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            _dragHandler.HandleReleased(this);
        }

        private void HandleExited()
        {
            gameObject.SetActive(false);
        }

        /// <summary>Creates block views from the block prefab.</summary>
        public sealed class Factory : PlaceholderFactory<BlockView>
        {
        }
    }
}
