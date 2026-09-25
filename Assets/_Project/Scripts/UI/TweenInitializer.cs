using DG.Tweening;
using Zenject;

namespace RollicCase.UI
{
    /// <summary>Initializes DOTween once with a preallocated tween capacity.</summary>
    public sealed class TweenInitializer : IInitializable
    {
        private const int TweenerCapacity = 200;
        private const int SequenceCapacity = 50;

        public void Initialize()
        {
            DOTween.Init(recycleAllByDefault: false, useSafeMode: true, logBehaviour: LogBehaviour.ErrorsOnly)
                .SetCapacity(TweenerCapacity, SequenceCapacity);
        }
    }
}
