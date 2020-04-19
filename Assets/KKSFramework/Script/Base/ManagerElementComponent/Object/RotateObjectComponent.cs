﻿using UnityEngine;

namespace KKSFramework.Event
{
    /// <summary>
    /// 오브젝트 클릭 회전 클래스.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class RotateObjectComponent : MonoBehaviour
    {
        #region Fields & Property

        /// <summary>
        /// 회전 속도.
        /// </summary>
        [Header("[RotateObjectBase]")] [Space(5)]
        public Vector2 v2_rotate_spd;

        /// <summary>
        /// 회전 기준.
        /// </summary>
        public Space status_rotate;

        /// <summary>
        /// 클릭 회전 조작 가능 여부.
        /// </summary>
        private bool is_allowed = true;

        /// <summary>
        /// 오브젝트 클릭 여부.
        /// </summary>
        private bool is_hit;

        #endregion

        #region UnityMethods

        private void OnMouseDown()
        {
            if (is_allowed)
                is_hit = true;
        }

        private void OnMouseUp()
        {
            if (is_allowed)
                is_hit = false;
        }

        private void Update()
        {
            if (is_hit) RotateToObject();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 회전 실행.
        /// </summary>
        private void RotateToObject()
        {
            var xAngle = Input.GetAxis("Mouse X") * v2_rotate_spd.x;
            var yAngle = Input.GetAxis("Mouse Y") * v2_rotate_spd.y;
            transform.Rotate(yAngle, -xAngle, 0, status_rotate);
        }

        /// <summary>
        /// 회전을 허용함.
        /// </summary>
        public void AllowRotate(bool p_is_allow)
        {
            is_allowed = p_is_allow;
        }

        #endregion
    }
}