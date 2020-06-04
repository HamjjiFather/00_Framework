using System;
using UnityEngine.SocialPlatforms;

namespace KKSFramework.PlatformService
{
    public interface IGameServiceFriend
    {
        void LoadFriends (Action<bool, IUserProfile[]> callback);
    }
}