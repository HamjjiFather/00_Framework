using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace KKSFramework
{
    public static class DeviceUniqueIdentifier
    {
        public delegate void UniqueIdentifierDelegate(string uid);


        public const string UnsupportMacAddress = "02:00:00:00:00:00";

#if UNITY_IPHONE && !UNITY_EDITOR
        [DllImport ("__Internal")]
        static extern string _Get_Device_id ();
#endif

        public static void GetUniqueIdentifier(UniqueIdentifierDelegate callback)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
		    GetiOSIdentifier (callback);
#elif UNITY_ANDROID && !UNITY_EDITOR
            GetAndroidIdentifier (callback);
#else
            var password = SystemInfo.deviceUniqueIdentifier;
            callback(password);
#endif
        }

#if UNITY_IPHONE && !UNITY_EDITOR
        public static string GetiOSIdentifier ()
        {
            return _Get_Device_id ();
        }

        [Conditional ("UNITY_IPHONE")]
        private static void GetiOSIdentifier (UniqueIdentifierDelegate callback)
        {
            var password = _Get_Device_id ();
            callback (password);
        }
#endif


        [Conditional("UNITY_ANDROID")]
        private static void GetAndroidIdentifier(UniqueIdentifierDelegate callback)
        {
            string password;

            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        using (var wifiManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "wifi"))
                        {
                            using (var wInfo = wifiManager.Call<AndroidJavaObject>("getConnectionInfo"))
                            {
                                password = wInfo.Call<string>("getMacAddress");
                            }
                        }
                    }
                }
            }
            catch (AndroidJavaException)
            {
                password = "";
            }

            if (!string.IsNullOrEmpty(password) && !password.Equals(UnsupportMacAddress))
            {
                password = ToMd5(password);
                callback(password);
            }
            else
            {
                if (!Application.RequestAdvertisingIdentifierAsync(
                    (advertisingId, trackingEnabled, error) =>
                    {
                        if (advertisingId.Equals(string.Empty))
                        {
                            password = SystemInfo.deviceUniqueIdentifier;
                            callback(password);
                        }
                        else
                        {
                            callback(advertisingId);
                        }
                    }
                ))
                {
                    password = SystemInfo.deviceUniqueIdentifier;
                    callback(password);
                }
            }
        }


        private static string ToMd5(string s)
        {
            var md5 = MD5.Create();
            var buffer = Encoding.UTF8.GetBytes(s);
            var numberArray = md5.ComputeHash(buffer);

            var destStrBuilder = new StringBuilder();
            foreach (var curByte in numberArray) destStrBuilder.Append(curByte.ToString("x2"));

            return destStrBuilder.ToString();
        }
    }
}