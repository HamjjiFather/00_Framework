﻿using System;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace KKSFramework.Object
{
    /// <summary>
    /// 프리팹 관리 클래스.
    /// </summary>
    [DisallowMultipleComponent]
    public class PrefabComponent : CachedComponent
    {
        public RectTransform rectTransform => GetCachedComponent<RectTransform> ();
        
        #region Methods
        
        
        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        /// <param name="p_t_parents"> 생성 경로. </param>
        /// <param name="p_types"> 추가할 컴포넌트 타입. </param>
        /// <returns></returns>
        public PrefabComponent InstantiateObject(params Type[] p_types)
        {
            var prefabComp = Instantiate(this);
            foreach (var t in p_types) prefabComp.AddCachedComponent(t);

            return prefabComp;
        }
        

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        /// <param name="p_t_parents"> 생성 경로. </param>
        /// <param name="p_types"> 추가할 컴포넌트 타입. </param>
        /// <returns></returns>
        public PrefabComponent InstantiateObject(Transform p_t_parents, params Type[] p_types)
        {
            var prefabComp = Instantiate(this, p_t_parents);
            foreach (var t in p_types) prefabComp.AddCachedComponent(t);

            return prefabComp;
        }

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        /// <param name="p_t_parents"></param>
        /// <param name="p_v3_local_euler_angle"></param>
        /// <param name="p_types"></param>
        /// <param name="p_v3_local_position"></param>
        /// <returns></returns>
        public PrefabComponent InstantiateObject(Transform p_t_parents, Vector3 p_v3_local_position,
            Vector3 p_v3_local_euler_angle, params Type[] p_types)
        {
            return InstantiateObject(p_t_parents, Vector3.one, p_v3_local_position, p_v3_local_euler_angle, p_types);
        }

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        /// <param name="p_t_parents"></param>
        /// <param name="p_v3_local_euler_angle"></param>
        /// <param name="p_types"></param>
        /// <param name="p_v3_local_scale"></param>
        /// <param name="p_v3_local_position"></param>
        /// <returns></returns>
        public PrefabComponent InstantiateObject(Transform p_t_parents, Vector3 p_v3_local_scale,
            Vector3 p_v3_local_position, Vector3 p_v3_local_euler_angle, params Type[] p_types)
        {
            var temp_obj = InstantiateObject(p_t_parents, p_types);
            var transform1 = temp_obj.transform;
            transform1.localScale = p_v3_local_scale;
            transform1.localPosition = p_v3_local_position;
            transform1.localEulerAngles = p_v3_local_euler_angle;

            return temp_obj;
        }

        /// <summary>
        /// 오브젝트를 생성하고 제네릭 타입에 해당하는 컴포넌트를 리턴.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_t_parents"></param>
        /// <param name="p_types"></param>
        /// <returns></returns>
        public T InstantiateType<T>(Transform p_t_parents, params Type[] p_types) where T : Component
        {
            return InstantiateObject(p_t_parents, p_types).GetCachedComponent<T>();
        }

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        /// <param name="p_t_parents"></param>
        /// <param name="p_v3_local_euler_angle"></param>
        /// <param name="p_types"></param>
        /// <param name="p_v3_local_position"></param>
        /// <returns></returns>
        public T InstantiateType<T>(Transform p_t_parents, Vector3 p_v3_local_position, Vector3 p_v3_local_euler_angle,
            params Type[] p_types) where T : Component
        {
            return InstantiateType<T>(p_t_parents, Vector3.one, p_v3_local_position, p_v3_local_euler_angle, p_types);
        }

        /// <summary>
        /// 오브젝트 생성.
        /// </summary>
        /// <param name="p_t_parents"></param>
        /// <param name="p_v3_local_scale"></param>
        /// <param name="p_v3_local_position"></param>
        /// <param name="p_v3_local_euler_angle"></param>
        /// <param name="p_types"></param>
        /// <returns></returns>
        public T InstantiateType<T>(Transform p_t_parents, Vector3 p_v3_local_scale, Vector3 p_v3_local_position,
            Vector3 p_v3_local_euler_angle, params Type[] p_types) where T : Component
        {
            return InstantiateObject(p_t_parents, p_v3_local_scale, p_v3_local_position, p_v3_local_euler_angle,
                p_types).GetCachedComponent<T>();
        }

        #endregion
    }
}