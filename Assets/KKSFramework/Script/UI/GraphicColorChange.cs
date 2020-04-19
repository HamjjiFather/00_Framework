using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.UI
{
    public enum StatusColorOption
    {
        Color,
        ColorOnly,
        AlphaOnly,
        None
    }

    /// <summary>
    /// Unity UI 색상.
    /// </summary>
    [RequireComponent(typeof(Graphic))]
    public class GraphicColorChange : CachedComponent
    {
        #region Fields & Property

        /// <summary>
        /// 색상 변경 참고 상태(색상 전체, 색상 만, 알파값 만).
        /// </summary>
        public StatusColorOption statusFollowOption;

        /// <summary>
        /// 타겟들.
        /// </summary>
        public List<Graphic> changeGraphics;

        /// <summary>
        /// 타겟 그래픽.
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
            _followingSubscribe = Observable.EveryUpdate().Subscribe(_ => changeGraphics.ForEach(x =>
            {
                switch (statusFollowOption)
                {
                    case StatusColorOption.Color:
                        x.SetColor(_targetGraphic.color);
                        break;
                    case StatusColorOption.ColorOnly:
                        x.SetOnlyColor(_targetGraphic.color);
                        break;
                    case StatusColorOption.AlphaOnly:
                        x.SetAlphaColor(_targetGraphic.color.a);
                        break;
                }
            }));
        }

        private void OnDisable()
        {
            _followingSubscribe.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 타겟이 될 그래픽 소스를 동적으로 할당함.
        /// </summary>
        public void AddChangeTarget(Graphic p_graphic)
        {
            changeGraphics.Add(p_graphic);
        }

        /// <summary>
        /// 타겟이었던 그래픽 소스를 동적으로 삭제함.
        /// </summary>
        /// <param name="p_graphic"></param>
        public void RemoveChangeTarget(Graphic p_graphic)
        {
            changeGraphics.Remove(p_graphic);
        }

        #endregion
    }
}