using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UniRx.Async;
using UniRx.Async.Triggers;
using UnityEngine;

#if CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NET_STANDARD_2_0 || NET_4_6))
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace BaseFrame.DoTween.Async.Triggers
{
    public static class DoTweenAsyncTriggerExtensions
    {
        private static T GetOrAddComponent<T>(GameObject gameObject)
            where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null) component = gameObject.AddComponent<T>();

            return component;
        }


        #region Special for single operation.

//        public static UniTask WaitForCompleteAsync (this Tweener tweener, CancellationToken cancellationToken = default)
//        {
//            var component = tweener.target as Component;
//            return component.GetAsyncDoTweenCompleteTrigger ().CompleteAsync (tweener, cancellationToken);
//        }


        public static UniTask<Tweener> WaitForCompleteAsync(this Tweener tweener,
            CancellationToken cancellationToken = default)
        {
            var completionSource = new UniTaskCompletionSource<Tweener>();
            tweener.OnComplete(() =>
            {
                if (!cancellationToken.IsCancellationRequested) completionSource.TrySetResult(tweener);
            });
            return completionSource.Task;
        }

        #endregion


        #region Get Triggers

        public static AsyncCompleteTrigger GetAsyncDoTweenCompleteTrigger(this GameObject gameObject)
        {
            return GetOrAddComponent<AsyncCompleteTrigger>(gameObject);
        }


        public static AsyncCompleteTrigger GetAsyncDoTweenCompleteTrigger(this Component component)
        {
            return component.gameObject.GetAsyncDoTweenCompleteTrigger();
        }

        #endregion
    }


    [DisallowMultipleComponent]
    public class AsyncCompleteTrigger : AsyncTriggerBase
    {
        private AsyncTriggerPromise<AsyncUnit> complete;
        private AsyncTriggerPromiseDictionary<AsyncUnit> completes;


        protected override IEnumerable<ICancelablePromise> GetPromises()
        {
            return Concat(complete, completes);
        }


        public UniTask CompleteAsync(Tweener tweener, CancellationToken cancellationToken)
        {
            tweener.OnComplete(() => TrySetResult(complete, completes, AsyncUnit.Default));
            return GetOrAddPromise(ref complete, ref completes, cancellationToken);
        }
    }
}

#endif