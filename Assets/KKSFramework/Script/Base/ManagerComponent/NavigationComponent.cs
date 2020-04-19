using KKSFramework.Management;
using UnityEngine;

namespace KKSFramework.Navigation
{
    public class NavigationComponent : ComponentBase
    {
        // 모든 베이스 클래스, 베이스 클래스를 상속한 클래스에서 사용.
        //[Header("[NavigationComponent]"), Space(5)]

        #region Constructor

        #endregion

        
        #region Fields & Property

#pragma warning disable CS0649

        /// <summary>
        /// PageView 캔버스.
        /// </summary>
        public Transform pageParents;

        public Transform PageParents => pageParents;

        /// <summary>
        /// CommonView 캔버스.
        /// </summary>
        public Transform commonViewParents;

        public Transform CommonViewParents => commonViewParents;

        /// <summary>
        /// 화면 전환 이펙터.
        /// </summary>
        public ViewEffector transitionEffector;

        /// <summary>
        /// 메인 UI 카메라.
        /// </summary>
        public Camera mainCamera;

#pragma warning restore CS0649

        #endregion


        #region UnityMethods

        #endregion


        #region Methods

        #endregion


        #region EventMethods

        #endregion
    }
}