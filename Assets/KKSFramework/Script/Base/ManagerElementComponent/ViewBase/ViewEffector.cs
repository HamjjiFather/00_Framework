using System;
using System.Threading;
using BaseFrame.DoTween.Async.Triggers;
using DG.Tweening;
using UniRx.Async;
using UnityEngine;

namespace KKSFramework.Navigation
{
    public enum ViewEffectType
    {
        None,
        Fade,
        Move,
        Scale
    }

    [Serializable]
    public class ViewEffectModel
    {
        /// <summary>
        /// effect animation duration.
        /// </summary>
        public float duration = 0.2f;

        /// <summary>
        /// animation effect ease.
        /// </summary>
        public Ease effectEase = Ease.OutQuad;
        
        /// <summary>
        /// animation effect type.
        /// </summary>
        public ViewEffectType viewEffectType;
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class ViewEffector : CachedComponent
    {
        /// <summary>
        /// show effect model.
        /// </summary>
        public ViewEffectModel ShowEffectModel;
        
        
        /// <summary>
        /// hide effect model.
        /// </summary>
        public ViewEffectModel HideEffectModel;
        
        
        /// <summary>
        /// canvasGroup.
        /// </summary>
        private CanvasGroup canvasGroup => GetCachedComponent<CanvasGroup>();

        
        /// <summary>
        /// is lock animation play.
        /// </summary>
        private bool _isLockPlayEffector;

        
        /// <summary>
        /// play fade-in view effect animation async. 
        /// </summary>
        public async UniTask ShowAsync(CancellationToken ct = default)
        {
            if (_isLockPlayEffector)
                return;
            
            var tweenCore = canvasGroup.DOFade(0, 0);
            
            if (ShowEffectModel.viewEffectType == ViewEffectType.Fade)
            {
                canvasGroup.alpha = 0;
                tweenCore = canvasGroup
                    .DOFade(1, ShowEffectModel.duration)
                    .SetEase(ShowEffectModel.effectEase);
                await tweenCore.WaitForCompleteAsync(ct);
            }
            if (ShowEffectModel.viewEffectType == ViewEffectType.Scale)
            {
                await (tweenCore.target as Transform)
                    .DOScale (1f, ShowEffectModel.duration)
                    .SetEase (ShowEffectModel.effectEase)
                    .WaitForCompleteAsync (ct);
            }
            if (ShowEffectModel.viewEffectType == ViewEffectType.Move)
            {
                await (tweenCore.target as Transform)
                    .DOLocalMove (new Vector3 (0, 50f, 0), ShowEffectModel.duration)
                    .SetEase (ShowEffectModel.effectEase)
                    .From (true)
                    .WaitForCompleteAsync (ct);
            }

            SetRaycast ();
        }

        
        /// <summary>
        /// play fade-out view effect animation async. 
        /// </summary>
        public async UniTask HideAsync(CancellationToken ct = default)
        {
            if (_isLockPlayEffector)
                return;
            
            var tweenCore = canvasGroup.DOFade(1, HideEffectModel.duration);
            
            if (HideEffectModel.viewEffectType == ViewEffectType.Fade)
            {
                canvasGroup.alpha = 1;
                tweenCore = canvasGroup
                    .DOFade(0, HideEffectModel.duration)
                    .SetEase(HideEffectModel.effectEase);
                await tweenCore.WaitForCompleteAsync(ct);
            }

            if (HideEffectModel.viewEffectType == ViewEffectType.Scale)
            {
                await (tweenCore.target as Transform)
                    .DOScale (0, HideEffectModel.duration)
                    .SetEase (HideEffectModel.effectEase)
                    .WaitForCompleteAsync (ct);
            }

            if (HideEffectModel.viewEffectType == ViewEffectType.Move)
            {
                await (tweenCore.target as Transform)
                    .DOLocalMove (new Vector3 (0, 50f, 0), HideEffectModel.duration)
                    .SetEase (HideEffectModel.effectEase)
                    .WaitForCompleteAsync (ct);
            }

            SetRaycast ();
        }

        
        /// <summary>
        /// play fade-in view effect animation immediately.
        /// </summary>
        public void ShowFadeImmediately ()
        {
            canvasGroup.alpha = 1;
            SetRaycast ();
        }

        
        /// <summary>
        /// play fade-out view effect animation immediately.
        /// </summary>
        public void HideFadeImmediately ()
        {
            canvasGroup.alpha = 0;
            SetRaycast ();
        }


        /// <summary>
        /// change effector playable state.
        /// </summary>
        public void SetLockState (bool isLockState)
        {
            _isLockPlayEffector = isLockState;
        }

        
        /// <summary>
        /// change 'blockRaycasts' value of canvasGroup by canvas alpha value.
        /// </summary>
        private void SetRaycast ()
        {
            canvasGroup.blocksRaycasts = canvasGroup.alpha.Equals (1);
        }
    }
}