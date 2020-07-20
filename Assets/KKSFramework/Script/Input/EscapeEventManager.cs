using System;
using KKSFramework.Management;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace KKSFramework.InputEvent
{
    /// <summary>
    /// 안드로이드의 경우 ESC 버튼 이벤트 관리 클래스.
    /// </summary>
    public class EscapeEventManager : ManagerBase<EscapeEventManager>
    {
        /// <summary>
        /// Escape 이벤트.
        /// </summary>
        private UnityAction _escapeAction;

        /// <summary>
        /// 이벤트 구독.
        /// </summary>
        private IDisposable _escapeSubscribe;

        /// <summary>
        /// Escape 이벤트 동작 여부.
        /// </summary>
        private bool _isActiveEvent;


        /// <summary>
        /// 이벤트 동작 여부 설정.
        /// </summary>
        public virtual void SetActiveEvent (bool active)
        {
            _isActiveEvent = active;
        }

        /// <summary>
        /// 이벤트 추가.
        /// </summary>
        public virtual void AddEscapeEvent (UnityAction action)
        {
#if UNITY_ANDROID || UNITY_STANDALONE
            _escapeAction = action;
            _escapeSubscribe = Observable.EveryUpdate ()
                .Where (x => Input.GetKeyDown ("escape"))
                .Subscribe (_ => { _escapeAction?.Invoke (); });
#endif
        }

        /// <summary>
        /// 이벤트 삭제.
        /// </summary>
        public virtual void RemoveEscapeEvent ()
        {
#if UNITY_ANDROID || UNITY_STANDALONE
            _escapeAction = null;
#endif
        }
    }
}