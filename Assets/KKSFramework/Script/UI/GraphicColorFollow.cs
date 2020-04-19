using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.UI
{
    /// <summary>
    /// 이 클래스를 사용하는 UGUI가, 타겟의 UGUI 색상을 따라감.
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public class GraphicColorFollow : CachedComponent
    {
        #region Fields & Property

        /// <summary>
        /// 색상 변경 참고 상태.
        /// </summary>
        public StatusColorOption statusFollowOption;

        /// <summary>
        /// 변경할 타겟 그래픽.
        /// </summary>
        public Graphic followingGraphic;

        /// <summary>
        /// 그래픽.
        /// </summary>
        private Graphic _targetGraphic => GetCachedComponent<Graphic>();

        /// <summary>
        /// 구독.
        /// </summary>
        private IDisposable _followingSubscribe;

        #endregion

        #region UnityMethods

        private void OnEnable()
        {
            _followingSubscribe = Observable.EveryUpdate().Subscribe(_ =>
            {
                _targetGraphic.color = followingGraphic.color;
            });
        }

        private void OnDisable()
        {
            _followingSubscribe.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 동적으로 타겟을 변경함.
        /// </summary>
        public void SetFollowTarget(Graphic p_graphic)
        {
            followingGraphic = p_graphic;
        }

        /// <summary>
        /// 동적으로 타겟을 삭제함.
        /// </summary>
        public void RemoveFollowTarget()
        {
            followingGraphic = null;
        }

        #endregion
    }
}