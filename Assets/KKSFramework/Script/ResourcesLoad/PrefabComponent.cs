using System;
using UnityEngine;
using Zenject;

namespace KKSFramework.ResourcesLoad
{
    /// <summary>
    /// 프리팹 관리 클래스.
    /// </summary>
    [DisallowMultipleComponent]
    public class PrefabComponent : CachedComponent
    {
        #region Return Generic Type

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        public T InstantiateObject<T> (params Type[] types) where T : Component
        {
            var prefabObj = InstantiateObject (types);
            return prefabObj.GetComponent<T> ();
        }


        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        public T InstantiateObject<T> (Transform parents, params Type[] types) where T : Component
        {
            var prefabObj = InstantiateObject (parents, types);
            return prefabObj.GetComponent<T> ();
        }


        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        public T InstantiateObject<T> (Transform parents, Vector3 localPosition,
            Vector3 localEulerAngle, params Type[] types) where T : Component
        {
            var prefabObj = InstantiateObject<T> (parents, Vector3.one, localPosition, localEulerAngle, types);
            return prefabObj.GetComponent<T> ();
        }

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        public T InstantiateObject<T> (Transform parents, Vector3 localScale,
            Vector3 localPosition, Vector3 localEulerAngle, params Type[] types) where T : Component
        {
            var prefabObj = InstantiateObject<T> (parents, localScale, localPosition, localEulerAngle, types);
            return prefabObj.GetComponent<T> ();
        }

        #endregion


        #region Return PrefabComponent

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        public PrefabComponent InstantiateObject (params Type[] types)
        {
            var prefabComp = ProjectContext.Instance.Container.InstantiatePrefab (this)
                .GetComponent<PrefabComponent> ();
            foreach (var t in types)
            {
                prefabComp.AddCachedComponent (t);
            }

            return prefabComp;
        }


        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        public PrefabComponent InstantiateObject (Transform parents, params Type[] types)
        {
            var prefabComp = ProjectContext.Instance.Container.InstantiatePrefab (this, parents)
                .GetComponent<PrefabComponent> ();
            prefabComp.transform.SetInstantiateTransform ();
            foreach (var t in types)
            {
                prefabComp.AddCachedComponent (t);
            }

            return prefabComp;
        }

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        public PrefabComponent InstantiateObject (Transform parents, Vector3 localPosition,
            Vector3 localEulerAngle, params Type[] types)
        {
            return InstantiateObject (parents, Vector3.one, localPosition, localEulerAngle, types);
        }

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        public PrefabComponent InstantiateObject (Transform parents, Vector3 localScale,
            Vector3 localPosition, Vector3 localEulerAngle, params Type[] types)
        {
            var prefabObj = InstantiateObject (parents, types);
            prefabObj.transform.SetInstantiateTransform ();

            return prefabObj;
        }

        #endregion
    }
}