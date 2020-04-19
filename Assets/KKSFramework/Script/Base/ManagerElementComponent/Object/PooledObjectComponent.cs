﻿using UnityEngine;

namespace KKSFramework.Object
{
    /// <summary>
    /// 풀링 정보 클래스.
    /// </summary>
    public struct PoolingInfo
    {
        public PoolingInfo(PoolingObjectType poolingObjectType, string prefixName)
        {
            IsInited = true;
            IsPooled = false;
            PoolingObjectType = poolingObjectType;
            PrefixName = prefixName;
        }

        /// <summary>
        /// 풀링 타입.
        /// </summary>
        public PoolingObjectType PoolingObjectType { get; }

        /// <summary>
        /// 풀링 이름.
        /// </summary>
        public string PrefixName { get; }

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
    public class PooledObjectComponent : PrefabComponent
    {
        #region Fields & Property

        /// <summary>
        /// 풀링 정보.
        /// </summary>
        private PoolingInfo PoolingInfo;

        #endregion

        #region Methods

        /// <summary>
        /// 오브젝트가 생성됨.
        /// </summary>
        public T Created<T>(PoolingObjectType pEPoolingObjectType, string p_name)
            where T : PooledObjectComponent
        {
            gameObject.SetActive(true);
            PoolingInfo = new PoolingInfo(pEPoolingObjectType, p_name);
            OnCreated();
            return GetCachedComponent<T>();
        }

        /// <summary>
        /// 오브젝트가 생성됨.
        /// </summary>
        public T Created<T>(Transform parents, PoolingObjectType pEPoolingObjectType, string p_name)
            where T : PooledObjectComponent
        {
            transform.SetParent(parents);
            gameObject.SetActive(true);
            PoolingInfo = new PoolingInfo(pEPoolingObjectType, p_name);
            OnCreated();
            return GetCachedComponent<T>();
        }

        /// <summary>
        /// 오브젝트가 파괴되지 않고 풀링 매니저에 등록됨.
        /// </summary>
        public virtual void PoolingObject()
        {
            PoolingInfo.IsPooled = true;
            gameObject.SetActive(false);
            ObjectPoolingManager.Instance.RegistPooledObject(PoolingInfo, this);
            OnPooling();
        }

        /// <summary>
        /// 풀링에서 해제됨 (오브젝트 활성화).
        /// </summary>
        public void Unpooled()
        {
            PoolingInfo.IsPooled = false;
            gameObject.SetActive(true);
            OnUnpooled();
        }

        /// <summary>
        /// 개체 생성시 실행할 함수.
        /// </summary>
        protected virtual void OnCreated()
        {
            Debug.Log($"{nameof(PooledObjectComponent)} : OnCreated");
        }

        /// <summary>
        /// 풀링될 때 실행 할 함수.
        /// </summary>
        protected virtual void OnPooling()
        {
            Debug.Log($"{nameof(PooledObjectComponent)} : OnPooling");
        }

        /// <summary>
        /// 풀링에서 해제될때 실행 할 함수.
        /// </summary>
        protected virtual void OnUnpooled()
        {
            Debug.Log($"{nameof(PooledObjectComponent)} : OnUnpooled");
        }

        #endregion
    }
}