﻿using UnityEngine;

namespace KKSFramework.ResourcesLoad
{
    /// <summary>
    /// 풀링 정보 클래스.
    /// </summary>
    public struct PoolingInfo
    {
        public PoolingInfo(string poolingPath)
        {
            IsInited = true;
            IsPooled = false;
            PoolingPath = poolingPath;
        }

        /// <summary>
        /// 풀링 타입.
        /// </summary>
        public string PoolingPath { get; }

        /// <summary>
        /// 풀링 여부.
        /// </summary>
        public bool IsPooled { get; set; }

        /// <summary>
        /// 최초 프리팹 생성 여부.
        /// </summary>
        public bool IsInited { get; }
    }

    /// <summary>
    /// 풀링으로 관리되는 오브젝트 베이스 클래스.
    /// </summary>
    public class PooingComponent : PrefabComponent
    {
        #region Fields & Property

        /// <summary>
        /// 풀링 정보.
        /// </summary>
        private PoolingInfo _poolingInfo;

        #endregion

        #region Methods

        /// <summary>
        /// 오브젝트가 생성됨.
        /// </summary>
        public T Created<T>(string poolingPath) where T : PooingComponent
        {
            gameObject.SetActive(true);
            _poolingInfo = new PoolingInfo(poolingPath);
            OnCreated();
            return GetCachedComponent<T>();
        }

        /// <summary>
        /// 오브젝트가 생성됨.
        /// </summary>
        public T Created<T>(Transform parents, string poolingPath) where T : PooingComponent
        {
            transform.SetParent(parents);
            gameObject.SetActive(true);
            _poolingInfo = new PoolingInfo(poolingPath);
            OnCreated();
            return GetCachedComponent<T>();
        }

        /// <summary>
        /// 오브젝트가 파괴되지 않고 풀링 매니저에 등록됨.
        /// </summary>
        public virtual void PoolingObject()
        {
            _poolingInfo.IsPooled = true;
            gameObject.SetActive(false);
            ObjectPoolingManager.Instance.RegistPooledObject(_poolingInfo, this);
            OnPooling();
        }

        /// <summary>
        /// 풀링에서 해제됨 (오브젝트 활성화).
        /// </summary>
        public void Unpooled()
        {
            _poolingInfo.IsPooled = false;
            gameObject.SetActive(true);
            OnUnpooled();
        }

        /// <summary>
        /// 개체 생성시 실행할 함수.
        /// </summary>
        protected virtual void OnCreated()
        {
            Debug.Log($"{nameof(PooingComponent)} : OnCreated");
        }

        /// <summary>
        /// 풀링될 때 실행 할 함수.
        /// </summary>
        protected virtual void OnPooling()
        {
            Debug.Log($"{nameof(PooingComponent)} : OnPooling");
        }

        /// <summary>
        /// 풀링에서 해제될때 실행 할 함수.
        /// </summary>
        protected virtual void OnUnpooled()
        {
            Debug.Log($"{nameof(PooingComponent)} : OnUnpooled");
        }

        #endregion
    }
}