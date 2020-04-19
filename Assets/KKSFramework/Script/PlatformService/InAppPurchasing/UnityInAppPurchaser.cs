#if BF_UNITY_IAP
namespace KKSFramework.PlatformService
{
    using System;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Purchasing;
    //using UnityEngine.Purchasing.Security;
    

    public class UnityInAppPurchaser : IStoreListener, IInAppPurchaser
    {
        private IStoreController _storeController;
        private IExtensionProvider _extensionProvider;

        private Action<bool> _onInitializedCallback;
        private Action<bool, string> _onBuyProductCallback;

        private ProductDesc[] _products;

        public ProductDesc[] Products
        {
            get { return _products; }
        }


        private bool IsInitialized ()
        {
            return _storeController != null && _extensionProvider != null;
        }


        #region IInAppPurchaser Implementations

        /// <summary>
        /// 모든 앱 스토어에서 상품 식별자가 동일할 경우
        /// </summary>
        public void ConnectBilling (string[] productIds, Action<bool> callback)
        {
            Log.Verbose (nameof(UnityInAppPurchaser), "Connect Billing...");

            if (IsInitialized ())
            {
                callback.CallSafe (true);
                return;
            }

            _onInitializedCallback = callback;

            var module = StandardPurchasingModule.Instance ();
            var builder = ConfigurationBuilder.Instance (module);

            foreach (var productId in productIds)
            {
                builder.AddProduct (productId, ProductType.Consumable);
            }

            UnityPurchasing.Initialize (this, builder);
        }


        /// <summary>
        /// 각 앱 스토어별로 상품 식별자가 다를 경우
        /// </summary>
        /// <param name="productIds">Unity IAP에서 사용하는 상품 식별자</param>
        /// <param name="googleProductIds">구글스토어 상품 식별자</param>
        /// <param name="appleProductIds">애플스토어 상품 식별자</param>
        public void ConnectBilling (string[] productIds, string[] googleProductIds, string[] appleProductIds,
            Action<bool> callback)
        {
            if (IsInitialized ())
                return;

            _onInitializedCallback = callback;

            var module = StandardPurchasingModule.Instance ();
            var builder = ConfigurationBuilder.Instance (module);

            var numOfProduct = productIds.Length;
            for (var iProduct = 0; iProduct < numOfProduct; iProduct++)
            {
                var productId = productIds[iProduct];
                var googleProductId = googleProductIds[iProduct];
                var appleProductId = appleProductIds[iProduct];

                builder.AddProduct (productId, ProductType.Consumable, new IDs
                {
                    {appleProductId, AppleAppStore.Name},
                    {googleProductId, GooglePlay.Name},
                });
            }

            UnityPurchasing.Initialize (this, builder);
        }


        public void BuyProductId (string productId, string payload, Action<bool, string> callback)
        {
            try
            {
                if (IsInitialized ())
                {
                    var p = _storeController.products.WithID (productId);
                    if (p != null && p.availableToPurchase)
                    {
                        _onBuyProductCallback = callback;
                        Log.Verbose (nameof(UnityInAppPurchaser), string.Format ("Purchasing product asychronously: '{0}'", p.definition.id));
                        _storeController.InitiatePurchase (p, payload);
                    }
                    else
                    {
                        callback.CallSafe (false, null);
                        Log.Verbose (nameof(UnityInAppPurchaser), 
                            "BuyProductId: FAIL. Not purchasing product, either is not found or is not available for purchase");
                    }
                }
                else
                {
                    callback.CallSafe (false, null);
                    Log.Verbose (nameof(UnityInAppPurchaser), "BuyProductId FAIL. Not initialized.");
                }
            }
            catch (Exception e)
            {
                callback.CallSafe (false, null);
                Log.Verbose (nameof(UnityInAppPurchaser), "BuyProductId: FAIL. Exception during purchase. " + e);
            }
        }

        #endregion


        public void RestorePurchase ()
        {
            if (!IsInitialized ())
            {
                Log.Verbose (nameof(UnityInAppPurchaser), "RestorePurchases FAIL. Not initialized.");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                Log.Verbose (nameof(UnityInAppPurchaser), "RestorePurchases started ...");

                var apple = _extensionProvider.GetExtension<IAppleExtensions> ();

                apple.RestoreTransactions
                (
                    result =>
                    {
                        Log.Verbose (nameof(UnityInAppPurchaser), "RestorePurchases continuing: " + result +
                                   ". If no further messages, no purchases available to restore.");
                    }
                );
            }
            else
            {
                Log.Verbose (nameof(UnityInAppPurchaser),"RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }


        #region IStoreListener Impletementions

        public void OnInitialized (IStoreController sc, IExtensionProvider ep)
        {
            Log.Verbose (nameof(UnityInAppPurchaser), "OnInitialized : PASS");

            _storeController = sc;
            _extensionProvider = ep;

            var sb = new StringBuilder ();
            foreach (var product in _storeController.products.all)
            {
                sb.AppendLine ("상품정보 : " + product.definition.id + ", "
                               + product.metadata.localizedTitle + ", "
                               + product.metadata.localizedDescription + ", "
                               + product.metadata.localizedPriceString + ", "
                               + product.metadata.localizedPrice + ", "
                               + product.metadata.isoCurrencyCode);
            }

            Log.Verbose (nameof(UnityInAppPurchaser),sb.ToString ());

            _products = _storeController.products.all.Select (x => new ProductDesc
            {
                Id = x.definition.id,
                LocalizedTitle = x.metadata.localizedTitle,
                LocalizedPrice = x.metadata.localizedPriceString,
                IsoCurrencyCode = x.metadata.isoCurrencyCode
            }).ToArray ();

            _onInitializedCallback.CallSafe (true);
        }


        public void OnInitializeFailed (InitializationFailureReason reason)
        {
            Log.Verbose (nameof(UnityInAppPurchaser), "OnInitializeFailed InitializationFailureReason:" + reason);
            _onInitializedCallback.CallSafe (false);
        }

        public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs args)
        {
            var receiptDesc = JsonUtility.FromJson<ReceiptDesc> (args.purchasedProduct.receipt);
//            var payloadDesc = JsonUtility.FromJson<PayloadDesc> (receiptDesc.Payload);
            Log.Verbose (nameof(UnityInAppPurchaser), $"Receipt : '{args.purchasedProduct.receipt}'");
//            Log.PrintSplitLog (string.Format ("payload : '{0}'", receiptDesc.Payload));

            _onBuyProductCallback.CallSafe (true, args.purchasedProduct.receipt);
            
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed (Product product, PurchaseFailureReason failureReason)
        {
            _onBuyProductCallback.CallSafe (false, null);
            Log.Verbose (nameof(UnityInAppPurchaser), string.Format ("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
                product.definition.storeSpecificId, failureReason));
        }

        #endregion
    }
}
#endif