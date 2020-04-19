using System;
using System.Threading;
using System.Collections.Generic;
using UniRx;
using UniRx.Async;
using UnityEngine;


namespace KKSFramework.Fsm
{
    /// <summary>
    /// Fsm 클래스.
    /// 시작(Start), 변경(Change) 정지(Stop)로 상태 제어.
    /// </summary>
    public class FsmRunner
    {
        #region Fields & Property

        /// <summary>
        /// 상태별 작동 함수 딕셔너리.
        /// </summary>
        private readonly Dictionary<string, Func<CancellationTokenSource, UniTask>> _fsmStateDict =
            new Dictionary<string, Func<CancellationTokenSource, UniTask>> ();

        /// <summary>
        /// 현재 상태 이름.
        /// </summary>
        private StringReactiveProperty _currentStateName;

        /// <summary>
        /// 상태 이름 변환 구독.
        /// </summary>
        private IDisposable _stateNameDisposable;

        /// <summary>
        /// Fsm 제어 토큰.
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 상태가 진행 중인지 여부.
        /// </summary>
        private bool _isRunned;

        public bool IsRunned => _isRunned;

        #endregion


        #region UnityMethods

        #endregion


        #region Methods

        /// <summary>
        /// Fsm 상태를 등록.
        /// </summary>
        public void RegistFsmState (string stateName, Func<CancellationTokenSource, UniTask> stateMethod)
        {
            if (_fsmStateDict.ContainsKey (stateName))
            {
                Debug.Log ($"already contained state: {stateName}");
                return;
            }

            _fsmStateDict.Add (stateName, stateMethod);
        }


        /// <summary>
        /// Fsm 시작.
        /// </summary>
        public void StartFsm (string initStateName)
        {
            if (_fsmStateDict.Count.Equals (0))
            {
                Debug.Log ("state is Empty");
                return;
            }

            _isRunned = true;
            _cancellationTokenSource = new CancellationTokenSource ();
            _currentStateName = new StringReactiveProperty(initStateName);
            
            _stateNameDisposable = _currentStateName.Subscribe (value =>
            {
                _fsmStateDict[value].Invoke (_cancellationTokenSource).Forget();
                Debug.Log ($"{value}");
            });
        }


        /// <summary>
        /// Fsm 상태 변경.
        /// </summary>
        public void ChangeState (string stateName)
        {
            if (!_fsmStateDict.ContainsKey (stateName))
            {
                Debug.Log ($"not exist state: {stateName}");
                return;
            }

            _currentStateName.Value = stateName;
            Debug.Log ($"change state: {stateName}");
        }


        /// <summary>
        /// Fsm 정지.
        /// </summary>
        public void StopFsm ()
        {
            if (!_isRunned) return;
            
            _isRunned = false;
            _currentStateName?.Dispose ();
            _stateNameDisposable?.Dispose();
            _cancellationTokenSource?.Cancel ();
            _cancellationTokenSource?.Dispose ();
            _fsmStateDict?.Clear ();
            Debug.Log ("stop fsm");
        }

        #endregion
    }
}