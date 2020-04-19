using System;
using KKSFramework.ResourcesLoad;
using UnityEngine;

namespace KKSFramework
{
    /// <summary>
    /// 프레임워크 베이스 데이터 세팅.
    /// </summary>
    public class FrameworkBase : Singleton<FrameworkBase>
    {
        [Serializable]
        public class FrameworkBaseData
        {
            public string aliasPassword;
            public string keysoPassword;

            public FrameworkBaseData (string p_key_pass, string p_alias_pass)
            {
                keysoPassword = p_key_pass;
                aliasPassword = p_alias_pass;
            }
        }
        // 모든 베이스 클래스, 베이스 클래스를 상속한 클래스에서 사용.
        //[Header("[FrameworkBase]"), Space(5)]


        #region Fields & Property

        /// <summary>
        /// 프레임워크 데이터 경로 텍스트에셋.
        /// </summary>
        private TextAsset textAsset;

        private TextAsset TaFramworkDataPath
        {
            get
            {
                if (!textAsset)
                    textAsset =
                        ResourcesLoadHelper.GetResources<TextAsset> (ResourceRoleType._Data, ResourcesType.Json,
                            Constants.Framework.FrameworkDataJsonFileName);

                return textAsset;
            }
        }

        /// <summary>
        /// 받아온 프레임워크 데이터.
        /// </summary>
        private FrameworkBaseData _frameworkBaseData;

        public FrameworkBaseData GetFrameworkBaseData
        {
            get
            {
                if (_frameworkBaseData == null)
                    _frameworkBaseData = LoadFrameworkData ();

                return _frameworkBaseData;
            }
        }

        #endregion


        #region UnityMethods

        #endregion


        #region Methods

        /// <summary>
        /// 프레임워크 데이터를 로드함.
        /// </summary>
        private FrameworkBaseData LoadFrameworkData ()
        {
            return JsonUtility.FromJson<FrameworkBaseData> (TaFramworkDataPath.text);
        }

        /// <summary>
        /// 프레임워크 데이터를 업데이트함.
        /// </summary>
        public void UpdateFrameWorkData (FrameworkBaseData frameworkBaseData)
        {
            _frameworkBaseData = frameworkBaseData;
        }

        #endregion


        #region EventMethods

        #endregion
    }
}