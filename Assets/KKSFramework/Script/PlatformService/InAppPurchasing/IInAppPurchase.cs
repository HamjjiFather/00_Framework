using System;

namespace KKSFramework.PlatformService
{
    public interface IInAppPurchaser
    {
        ProductDesc[] Products { get; }

        void ConnectBilling(string[] productIds, Action<bool> callback);
        void BuyProductId(string productId, string payload, Action<bool, string> callback);
    }


    public struct ProductDesc
    {
        public string Id;
        public string LocalizedTitle;
        public string LocalizedPrice;
        public string IsoCurrencyCode;
    }


    public struct ReceiptDesc
    {
        public string Store;
        public string TransactionID;
        public string Payload;
    }


    public struct PayloadDesc
    {
        public string json;
        public string signature;
    }


//	public struct GooglePlayReceipt
//	{
//		public string orderId;
//		public string packageName;
//		public string productId;
//		public double purchaseTime;
//		public int purchaseState;
//		public GooglePlayDeveloperPayload developerPayload;
//	}
//	
//	public struct GooglePlayDeveloperPayload
//	{
//		public string developerPayload;
//		public bool is_free_trial;
//		public bool has_introductory_price_trial;
//		public bool is_updated;
//	}
}