#if UNITY_FACEBOOK
namespace KKSFramework.PlatformService
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Facebook.Unity;
    using UnityEngine.SocialPlatforms;
    using System.Linq;


    /// <summary>
    /// 페이스북 래핑 클래스
    /// </summary>
    public class FacebookGameroomService : IGameService, IInAppPurchaser
    {
        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        public Texture2D PlayerImage { get; private set; }

        public bool IsLoggedIn
        {
            get { return FB.IsLoggedIn; }
        }

        private ProductDesc[] _products;

        public ProductDesc[] Products
        {
            get { return _products; }
        }


        private void Awake ()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp ();
            }
            else
            {
                FB.Init (FB.ActivateApp);
            }
        }


        #region Tracking

        /// <summary>
        /// 사용자의 구매 액션을 로깅합니다.
        /// </summary>
        /// <param name="packageName">구매한 항목의 SKU code</param>
        /// <param name="priceAmount">지출한 금액</param>
        /// <param name="priceCurrency">지출한 통화를 나타내는 3자리 ISO code</param>
        public void LogPurchase (FBPurchaseParam param)
        {
            var iapParameters = new Dictionary<string, object> ();
            iapParameters["mygame_packagename"] = param.productId;
            FB.LogPurchase (param.priceAmount, param.priceCurrency, iapParameters);
        }


        /// <summary>
        /// 지출한 크레딧 금액을 추적합니다.
        /// </summary>
        /// <param name="storeItem">사용자가 구매한 항목의 이름</param>
        /// <param name="numGold">구매에서 지출한 앱 내 통화 수</param>
        public void LogAppEvent (string storeItem, float numGold)
        {
            var softPurchaseParameters = new Dictionary<string, object> ();
            softPurchaseParameters["mygame_purchased_item"] = storeItem;
            FB.LogAppEvent (
                AppEventName.SpentCredits,
                numGold,
                softPurchaseParameters
            );
        }

        #endregion


        #region Login / Logout

        public void Login (Action<bool> callback)
        {
            var perms = new List<string> {"public_profile", "email", "user_friends"};
            FB.LogInWithReadPermissions (perms, result =>
            {
                if (FB.IsLoggedIn)
                {
                    Log.Verbose (LogCategory.Service, result);

                    FB.API ("me?fields=id,name,picture", HttpMethod.GET, graphResult =>
                    {
                        PlayerId = (string) graphResult.ResultDictionary["id"];
                        PlayerName = (string) graphResult.ResultDictionary["name"];
                        PlayerImage = graphResult.Texture;

                        callback.CallSafe (FB.IsLoggedIn);
                    });
                }
                else
                {
                    Debug.Log ("User cancelled login");
                    callback.CallSafe (FB.IsLoggedIn);
                }
            });
        }


        public void Logout ()
        {
            FB.LogOut ();
        }

        #endregion


        #region Achievement

        public void ShowAchievementsUi ()
        {
            throw new NotImplementedException ();
        }


        public void UnlockAchievement (string achievementId, Action<bool> callback = null)
        {
            throw new NotImplementedException ();
        }

        #endregion


        #region Leaderboad

        public void SetDefaultLeaderboard (string leaderboardId)
        {
            throw new NotImplementedException ();
        }


        public void PostScore (long score, Action<bool> callback = null, string leaderboardId = null)
        {
            throw new NotImplementedException ();
        }


        public void ShowLeaderboardUi ()
        {
            throw new NotImplementedException ();
        }


        public void ShowLeaderboardUi (string leaderboardId)
        {
            throw new NotImplementedException ();
        }

        #endregion


        #region Cloud Save / Load

        public void SavedGameData (byte[] data, Action<bool> callback)
        {
            throw new NotImplementedException ();
        }


        public void LoadGameData (Action<bool, byte[]> callback)
        {
            throw new NotImplementedException ();
        }


        public void DeleteGameData (Action<bool> callback = null)
        {
            throw new NotImplementedException ();
        }

        #endregion


        #region Friend

        public void LoadFriends (Action<bool, IUserProfile[]> callback)
        {
            throw new NotImplementedException ();
        }

        #endregion


        #region IInAppPurchaser Implementations

        public void ConnectBilling (string[] productIds, Action<bool> callback)
        {
            FB.API ("app/products", HttpMethod.GET, result =>
            {
                if (string.IsNullOrEmpty (result.Error))
                {
                    Log.Verbose (LogCategory.Service, result.RawResult);
                    var fbProducts = JsonUtility.FromJson<FBProducts> (result.RawResult);

                    _products = fbProducts.data.Select (x => new ProductDesc
                    {
                        Id = x.product_id,
                        LocalizedTitle = x.title,
                        LocalizedPrice = x.price,
                        IsoCurrencyCode = x.price_currency_code
                    }).ToArray ();

                    callback.CallSafe (true);
                }
                else
                {
                    Log.VerboseFormat (LogCategory.Service, "Facebook request products failed : {0}", result.Error);
                    callback.CallSafe (false);
                }
            });
        }


        public void BuyProductId (string productId, Action<bool, PayloadDesc?> callback)
        {
            FB.Canvas.PayWithProductId (productId, "purchaseiap", 1, null, null, null, null, null,
                result =>
                {
                    Log.Verbose (LogCategory.Service, result.RawResult);
                    var isError = result.Cancelled || result.ErrorCode > 0;
                    callback.CallSafe (!isError, null);
                });
        }

        #endregion
    }


    public struct FBPurchaseParam
    {
        public string productId;
        public float priceAmount;
        public string priceCurrency;
    }


    [Serializable]
    public class FBProduct
    {
        public string title;
        public string product_id;
        public string product_type;
        public string description;
        public string price;
        public int price_amount_cents;
        public string price_currency_code;
    }


    [Serializable]
    public class FBProducts
    {
        public List<FBProduct> data;
    }
}
#endif