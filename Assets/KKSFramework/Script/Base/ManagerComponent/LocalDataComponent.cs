using KKSFramework.Management;
using UnityEngine.Events;

namespace KKSFramework.LocalData
{
    public class LocalDataComponent : ComponentBase
    {
        #region Fields & Property

#pragma warning disable CS0649

        private UnityAction _saveAction;

#pragma warning restore CS0649

        #endregion

        #region UnityMethods

        /// <summary>
        /// 앱을 종료할 경우 데이터를 저장.
        /// </summary>
        private void OnApplicationQuit()
        {
            _saveAction?.Invoke();
        }

        #endregion

        #region Methods

        public void SetSaveAction(UnityAction action)
        {
            _saveAction = action;
        }

        #endregion

        #region Constructor

        #endregion

        #region EventMethods

        #endregion
    }
}