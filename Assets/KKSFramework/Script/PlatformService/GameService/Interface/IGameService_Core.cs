using System;
using UnityEngine;

namespace KKSFramework.PlatformService
{
    public interface IGameServiceCore
    {
        bool IsInited { get; }

        /// <summary>
        /// 프로필 ID.
        /// </summary>
        string PlayerId { get; }

        /// <summary>
        /// 프로필 이름.
        /// </summary>
        string PlayerName { get; }

        /// <summary>
        /// 프로필 이미지.
        /// </summary>
        Texture2D PlayerImage { get; }

        /// <summary>
        /// 인증이 되어 있는지(플랫폼 or 게스트)
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// 게스트인지.
        /// </summary>
        bool IsGuestUser { get; }
        
        
        #region Login / Logout

        void Login(Action<bool, string> callback);
        
        void Logout();

        #endregion
    }
}