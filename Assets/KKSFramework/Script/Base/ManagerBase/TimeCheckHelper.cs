using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine.Events;

namespace CityFisherman
{
    using DisposableDictionary = Dictionary<string, TimeCheckHelper.TimeCheckData>;


    public static class TimeCheckHelper
    {
        /// <summary>
        /// 구독중인 시간 체크 딕셔너리.
        /// </summary>
        private static readonly DisposableDictionary _disposablesDict = new DisposableDictionary();


        /// <summary>
        /// 남은 시간 체크.
        /// 키를 가지고 있지 않을 경우 새 시간 체크 이벤트가 등록이 되고.
        /// 이미 키를 가진 시간 체크 이벤트의 경우 업데이트가 된다.
        /// </summary>
        public static void RegisterRemainingTimeEvent(string key, double completeTime,
            UnityAction<double> nextAction = null, UnityAction completeAction = null, double intervalTime = 0.1f,
            bool autoComplete = true)
        {
            var remainTime = completeTime;

            if (string.IsNullOrEmpty(key)) return;

            // 이미 같은 키값으로 시간 체크가 되고 있음.
            if (_disposablesDict.ContainsKey(key))
            {
                _disposablesDict[key].TimeCheckDisposable.DisposeSafe();
            }
            // 새로운 시간 체크.
            else
            {
                _disposablesDict.Add(key, new TimeCheckData());
                _disposablesDict[key].AddEvents(nextAction, completeAction);
            }

            _disposablesDict[key].TimeCheckDisposable = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(intervalTime))
                .TimeInterval()
                .Subscribe(Next);

            // 시간 체크 처리.
            void Next(TimeInterval<long> interval)
            {
                if (!_disposablesDict.ContainsKey(key)) return;


                remainTime -= interval.Interval.TotalSeconds;
                if (remainTime <= 0)
                {
                    Complete();
                    return;
                }

                _disposablesDict[key].TimeCheckEvent.NextEvent?.Invoke(remainTime);
            }

            // 구독 완료 시 처리.
            void Complete()
            {
                if (!_disposablesDict.ContainsKey(key)) return;

                _disposablesDict[key].Clear();

                if (autoComplete)
                    _disposablesDict.Remove(key);
            }
        }


        /// <summary>
        /// 경과된 시간 체크
        /// </summary>
        public static void RegisterElapsedTimeEvent(string key, double completeTime,
            UnityAction<double> nextAction = null, UnityAction completeAction = null, double intervalTime = 0.1f,
            bool autoComplete = true)
        {
            var remainTime = 0d;

            if (!_disposablesDict.ContainsKey(key))
            {
                _disposablesDict.Add(key, new TimeCheckData());
                _disposablesDict[key].AddEvents(nextAction, completeAction);
            }

            _disposablesDict[key].TimeCheckDisposable = Observable
                .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(intervalTime))
                .TimeInterval()
                .Subscribe(Next, Complete);

            // 시간 체크 처리.
            void Next(TimeInterval<long> interval)
            {
                remainTime += interval.Interval.TotalSeconds;
                if (remainTime >= completeTime)
                {
                    Complete();
                    return;
                }

                _disposablesDict[key].TimeCheckEvent.NextEvent.Invoke(remainTime);
            }

            // 구독 완료 시 처리.
            void Complete()
            {
                _disposablesDict[key].TimeCheckDisposable.Dispose();
                _disposablesDict[key].TimeCheckEvent.CompleteEvent.Invoke();

                if (autoComplete)
                    _disposablesDict.Remove(key);
            }
        }


        public static bool HasTimeCheckEvent(string key)
        {
            return _disposablesDict.ContainsKey(key);
        }


        /// <summary>
        /// 시간 체크 이벤트 추가.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="nextAction"></param>
        /// <param name="completeAction"></param>
        public static void AddTimeCheckEvent(string key, UnityAction<double> nextAction = null,
            UnityAction completeAction = null)
        {
            if (!_disposablesDict.ContainsKey(key)) return;

            _disposablesDict[key].AddEvents(nextAction, completeAction);
        }


        /// <summary>
        /// 시간 체크 이벤트 삭제.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="nextAction"></param>
        /// <param name="completeAction"></param>
        public static void RemoveTimeCheckEvent(string key, UnityAction<double> nextAction = null,
            UnityAction completeAction = null)
        {
            if (!_disposablesDict.ContainsKey(key)) return;

            _disposablesDict[key].RemoveEvents(nextAction, completeAction);
        }


        /// <summary>
        /// 시간 체크 구독 해지.
        /// </summary>
        public static void Unregister(string key)
        {
            if (!_disposablesDict.ContainsKey(key)) return;

            _disposablesDict[key].TimeCheckDisposable.DisposeSafe();
            _disposablesDict.Remove(key);
        }

        public class NextEvent : UnityEvent<double>
        {
        }


        public class TimeCheckEvent
        {
            public readonly UnityEvent CompleteEvent = new UnityEvent();
            public readonly NextEvent NextEvent = new NextEvent();
        }


        public class TimeCheckData
        {
            public readonly TimeCheckEvent TimeCheckEvent = new TimeCheckEvent();
            public IDisposable TimeCheckDisposable;


            public void AddEvents(UnityAction<double> nextAction = null, UnityAction completeAction = null)
            {
                if (nextAction != null)
                    TimeCheckEvent.NextEvent.AddListener(nextAction);
                if (completeAction != null)
                    TimeCheckEvent.CompleteEvent.AddListener(completeAction);
            }


            public void RemoveEvents(UnityAction<double> nextAction = null, UnityAction completeAction = null)
            {
                if (nextAction != null)
                    TimeCheckEvent.NextEvent.RemoveListener(nextAction);
                if (completeAction != null)
                    TimeCheckEvent.CompleteEvent.RemoveListener(completeAction);
            }


            public void Clear()
            {
                TimeCheckDisposable.DisposeSafe();
                TimeCheckEvent.CompleteEvent?.Invoke();
            }
        }
    }
}