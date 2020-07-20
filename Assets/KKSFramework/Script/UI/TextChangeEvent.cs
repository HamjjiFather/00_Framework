using System.Collections;
using System.Threading.Tasks;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.UI
{
    [RequireComponent(typeof(Text))]
    public class TextChangeEvent : CachedComponent
    {
        #region Constructor

        #endregion

        #region Fields & Property

#pragma warning disable CS0649
#pragma warning restore CS0649

        protected Text Text => GetCachedComponent<Text>();

        /// <summary>
        /// 문자열 표기 포맷.
        /// </summary>
        public string customFormat = "{0}";

        /// <summary>
        /// 변화 이벤트 시간.
        /// </summary>
        private const float ChangeTime = 1f;

        #endregion

        #region UnityMethods

        #endregion

        #region Methods

        /// <summary>
        /// 비동기 텍스트 애니메이션.
        /// </summary>
        public virtual async Task AsyncTextAnimation(float originValue, float changedValue)
        {
            await WaitTextAnimation(originValue, changedValue).ToUniTask();
        }

        /// <summary>
        /// 텍스트 애니메이션.
        /// </summary>
        public virtual void TextAnimation(int originValue, int changedValue)
        {
            StopAllCoroutines();
            StartCoroutine(WaitTextAnimation(originValue, changedValue));
        }

        /// <summary>
        /// 텍스트 애니메이션 코루틴.
        /// </summary>
        private IEnumerator WaitTextAnimation(float originValue, float changedValue)
        {
            var nowChangeTime = 0f;
            while (nowChangeTime < ChangeTime)
            {
                Text.text = string.Format(customFormat,
                    Mathf.Lerp(originValue, changedValue, Mathf.Clamp(nowChangeTime / ChangeTime, 0f, 1f)));
                nowChangeTime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        /// <summary>
        /// 텍스트 포맷 변경.
        /// </summary>
        public void SetCustomFormat(string newFormat)
        {
            customFormat = newFormat;
        }

        #endregion

        #region EventMethods

        #endregion
    }
}