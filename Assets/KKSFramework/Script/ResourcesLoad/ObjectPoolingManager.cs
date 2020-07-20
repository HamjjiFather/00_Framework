using System;
using System.Collections.Generic;
using KKSFramework.Management;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KKSFramework.ResourcesLoad
{
    /// <summary>
    /// 오브젝트 풀링 관리 클래스.
    /// 선입선출 타입의 페이지뷰 풀링.
    /// 2018.06.03. 작성.
    /// </summary>
    public class ObjectPoolingManager : ManagerBase<ObjectPoolingManager>
    {
        #region Constructor

        #endregion


        #region Fields & Property

#pragma warning disable CS0649

        /// <summary>
        /// 오브젝트 풀링 컴포넌트.
        /// </summary>
        private ObjectPoolingComponent _objectPoolingComponent => ComponentBase as ObjectPoolingComponent;

#pragma warning restore CS0649

        /// <summary>
        /// 풀링된 오브젝트 보관 트랜스폼.
        /// </summary>
        private readonly Dictionary<string, Transform> _pooledObjTransformDict =
            new Dictionary<string, Transform> ();

        /// <summary>
        /// 풀링된 일반 프리팹 오브젝트 리스트.
        /// enum 타입별로 관리하고 싶으면 변수 타입에 해당하는 딕셔너리를 늘려서 관리하도록.
        /// </summary>
        private readonly Dictionary<string, Queue<PooingComponent>> _pooledPrefabs =
            new Dictionary<string, Queue<PooingComponent>> ();

        #endregion


        #region Methods

        /// <summary>
        /// 해당 타입의 풀링된 오브젝트를 리턴.
        /// </summary>
        private PooingComponent ReturnPooledObjectBase (string poolingPath)
        {
            if (IsExistPooledObject (poolingPath))
                return _pooledPrefabs[poolingPath].Dequeue ();

            return null;
        }

        /// <summary>
        /// 해당 타입의 해당 문자열을 가진 풀링된 오브젝트가 있는지 여부.
        /// </summary>
        /// <returns></returns>
        public bool IsExistPooledObject (string resourceTypeString)
        {
            return _pooledPrefabs.ContainsKey (resourceTypeString) && _pooledPrefabs[resourceTypeString].Count != 0;
        }

        /// <summary>
        /// 풀링된 오브젝트를 꺼냄.
        /// 리소스 관리를 껐다 켰다 할 수 있도록 설계해야 함.
        /// </summary>
        public T ReturnLoadResources<T> (string poolingPath) where T : Object
        {
            if (!IsExistPooledObject (poolingPath))
            {
                Debug.LogException (new NullReferenceException ("null ref exception pooled object"));
                return null;
            }

            var pooledObject = ReturnPooledObjectBase (poolingPath);

            // 리소스 오브젝트가 없음.
            if (pooledObject == null)
            {
                Debug.LogException (new NullReferenceException ("null ref exception pooled object"));
                return null;
            }

            pooledObject.Unpooled ();

            return pooledObject.GetComponent<T> ();
        }


        /// <summary>
        /// 오브젝트를 파괴하지 않고 풀링함.
        /// 풀링된 오브젝트는 하위에 위치 시킴.
        /// </summary>
        public void RegistPooledObject (PoolingInfo poolingInfo, PooingComponent pooingComponent)
        {
            if (_pooledPrefabs.ContainsKey (poolingInfo.PoolingPath) == false)
                _pooledPrefabs.Add (poolingInfo.PoolingPath, new Queue<PooingComponent> ());

            _pooledPrefabs[poolingInfo.PoolingPath].Enqueue (pooingComponent);

            if (_pooledObjTransformDict.ContainsKey (poolingInfo.PoolingPath) == false)
            {
                var gameObj = new GameObject ();
                var objTransform = gameObj.transform;
                objTransform.name = poolingInfo.PoolingPath;
                objTransform.SetParent (_objectPoolingComponent.transform);
                objTransform.SetInstantiateTransform ();

                _pooledObjTransformDict.Add (poolingInfo.PoolingPath, objTransform);
            }

            pooingComponent.transform.SetParent (_pooledObjTransformDict[poolingInfo.PoolingPath]);
            pooingComponent.gameObject.SetActive (false);
        }

        #endregion
    }
}