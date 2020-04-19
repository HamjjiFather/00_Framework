//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Threading.Tasks;
//using Unity.Notifications.Android;
//using Unity.Notifications.iOS;
//using Unity.UNetWeaver;
//using UnityEngine;
////#if UNITY_ANDROID
//using Unity.Notifications.Android;
//
////#elif UNITY_IOS
//using Unity.Notifications.iOS;
//
////#endif
//
//namespace CityFisherman
//{
//    public enum LocalPushCallType
//    {
//    }
//
//
//    public class NotificationBundle
//    {
//        
//    }
//    
//    /// <summary>
//    /// 푸시 메세지 모델.
//    /// </summary>
//    public class PushMessageModel
//    {
//        public LocalPushCallType LocalPushCallType;
//        public DateTime FireTime;
//        public string Title;
//        public string Description;
//
//        public bool IsExpired ()
//        {
//            return DateTime.Now >= FireTime;
//        }
//
//        public bool IsNight ()
//        {
//            return LocalPushServiceHelper.CheckNightTime (FireTime);
//        }
//
//        public void Clear ()
//        {
//            FireTime = DateTime.Now;
//            Title = string.Empty;
//            Description = string.Empty;
//        }
//
//        public void RemoveNightPush ()
//        {
//            if (LocalPushServiceHelper.CheckNightTime (FireTime))
//            {
//                Clear ();
//            }
//        }
//    }
//
//    public class LocalPushServiceHelper : MonobehaviourSingletonClass<LocalPushServiceHelper>
//    {
//        #region Binding
//
//#pragma warning disable 0649
//
//#pragma warning restore 0649
//
//        #endregion
//
//
//        /// <summary>
//        /// 전체 푸시 동의 여부.
//        /// </summary>
//        private static bool _enableAllNotification;
//
//        /// <summary>
//        /// 야간 푸시 동의 여부.
//        /// </summary>
//        private static bool _enableNightNotification;
//        
//        /// <summary>
//        /// 현재 관리되고 있는 푸시 목록.
//        /// </summary>
//        private static Dictionary<LocalPushCallType, PushMessageModel> _localPushDict;
//
//        /// <summary>
//        /// 야간 푸시 시점(21시 ~ 08시).
//        /// </summary>
//        private static readonly Vector2 NightTime = new Vector2 (8, 21);
//        
//        
////#if UNITY_ANDROID && !UNITY_EDITOR
//
//        /// <summary>
//        /// 안드로이드 전용 로컬 푸시 채널 이름.
//        /// </summary>
//        private const string AndroidLocalPushChannel = "CityFishermanAndroidChannel";
//
//        /// <summary>
//        /// 안드로이드 전용 아이콘 이름.
//        /// </summary>
//        private const string LocalPushIconName = "icon_name";
//        
////#endif
//
//
//        #region Unity Methods
//
//        private void OnApplicationQuit ()
//        {
//            SendPushMessage ();
//        }
//
//        private void OnApplicationPause (bool pause)
//        {
//            // 앱이 백그라운드로 들어갈경우.
//            if (pause)
//            {
//                // 모든 푸시를 보냄.
//                SendPushMessage ();
//                return;
//            }
//
//            // 모든 푸시
//            RemovePushMessage ();
//        }
//
//        #endregion
//
//
//        /// <summary>
//        /// 푸시 메세지 전송.
//        /// </summary>
//        private void SendPushMessage ()
//        {
//            if (_localPushDict == null || _localPushDict.Count == 0) return;
//
//            // 보내기 전 보냈던 모든 푸시메세지를 삭제한다.
//            RemovePushMessage ();
//
//            _localPushDict.Values
//                .Where (x => !x.IsExpired ())
//                .Foreach (x =>
//                {
//                    // 전체 푸시 수신에 동의되어 있지 않았을 경우.
//                    if (!_enableAllNotification) return;
//
//                    // 야간 푸시 수신에 동의하지 않거나, 야간에 표시되는 푸시일 경우.
//                    if (!_enableNightNotification && x.IsNight ())
//                    {
//                        return;
//                    }
//
////#if UNITY_ANDROID && !UNITY_EDITOR
//                    var identifier = (int) x.LocalPushCallType;
//                    var notification = new AndroidNotification
//                    {
//                        Title = _localPushDict[x.LocalPushCallType].Title,
//                        Text = _localPushDict[x.LocalPushCallType].Description,
//                        FireTime = x.FireTime,
//                        SmallIcon = "push_icon_small",
//                        ShouldAutoCancel = true,
//                    };
//                    
//                    if (AndroidNotificationCenter.CheckScheduledNotificationStatus (identifier) ==
//                        NotificationStatus.Scheduled)
//                    {
//                        // Replace the currently scheduled notification with a new notification.
//                        AndroidNotificationCenter.UpdateScheduledNotification (identifier, notification,
//                            AndroidLocalPushChannel);
//                    }
//                    else if (AndroidNotificationCenter.CheckScheduledNotificationStatus (identifier) ==
//                             NotificationStatus.Delivered)
//                    {
//                        AndroidNotificationCenter.CancelDisplayedNotification (identifier);
//                    }
//                    
//                    AndroidNotificationCenter.SendNotification (notification, AndroidLocalPushChannel);
//
////#elif UNITY_IOS
//                    iOSNotificationCenter.RemoveScheduledNotification (x.LocalPushCallType.ToString ());
//                    iOSNotificationCenter.RemoveDeliveredNotification (x.LocalPushCallType.ToString ());
//
//                    var timeTrigger = new iOSNotificationTimeIntervalTrigger
//                    {
//                        TimeInterval = TimeSpan.FromSeconds ((x.FireTime - DateTime.Now).TotalSeconds),
//                        Repeats = false
//                    };
//
//                    var iOsNotification = new iOSNotification
//                    {
//                        Identifier = x.LocalPushCallType.ToString (),
//                        Title = _localPushDict[x.LocalPushCallType].Title,
//                        Body = string.Empty,
//                        Subtitle = _localPushDict[x.LocalPushCallType].Description,
//                        
//                        ShowInForeground = false,
//                        ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Sound,
//                        CategoryIdentifier = "Category",
//                        ThreadIdentifier = "Thread",
//                        Trigger = timeTrigger
//                    };
//
//                    iOSNotificationCenter.ScheduleNotification (iOsNotification);
//
//#endif
//                });
//        }
//
//
//        /// <summary>
//        /// 발송되어 대기 중인 모든 푸시 메세지 삭제.
//        /// </summary>
//        private static void RemovePushMessage ()
//        {
//#if UNITY_ANDROID && !UNITY_EDITOR
//            AndroidNotificationCenter.CancelAllScheduledNotifications ();
//#elif UNITY_IOS
//            iOSNotificationCenter.RemoveAllScheduledNotifications ();
//#endif
//        }
//
//
//        public void Awake()
//        {
//            var localTypeEnums = Enum.GetValues (typeof (LocalPushCallType)) as LocalPushCallType[];
//            _localPushDict = localTypeEnums?.ToDictionary (x => x, x => new PushMessageModel ());
//            localTypeEnums.ForEach ((enums, i) =>
//            {
//                _localPushDict[enums].LocalPushCallType = enums;
//                _localPushDict[enums].FireTime = _localPushBundle.LocalPushDateTimes[i];
//            });
//            RemovePushMessage ();
//        }
//
//
//        private static async Task InitializeLocalPush ()
//        {
////#if UNITY_ANDROID && !UNITY_EDITOR
//            AndroidNotificationCenter.Initialize ();
//
//            var channel = new AndroidNotificationChannel
//            {
//                Id = AndroidLocalPushChannel,
//                Name = "channel",
//                Importance = Importance.High,
//                Description = "desc",
//            };
//            AndroidNotificationCenter.RegisterNotificationChannel (channel);
//
//            void ReceivedNotificationHandler (AndroidNotificationIntentData data)
//            {
//                var msg = "Notification received : " + data.Id + "\n";
//                msg += "\n Notification received: ";
//                msg += "\n .Channel: " + data.Channel;
//                msg += "\n .Title: " + data.Notification.Title;
//                msg += "\n .Body: " + data.Notification.Text;
//                Debug.Log (msg);
//            }
//
//            AndroidNotificationCenter.OnNotificationReceived += ReceivedNotificationHandler;
////#elif UNITY_IOS
//            using (var req = new AuthorizationRequest (AuthorizationOption.Alert | AuthorizationOption.Badge, true))
//            {
//                await Task.Run (() => req.IsFinished);
//                
//                string res = "\n RequestAuthorization: \n";
//                res += "\n finished: " + req.IsFinished;
//                res += "\n granted :  " + req.Granted;
//                res += "\n error:  " + req.Error;
//                res += "\n deviceToken:  " + req.DeviceToken;
//                Debug.Log (res);
//            }
//#endif
//
//            await Task.CompletedTask;
//        }
//
//
//        /// <summary>
//        /// 푸시 메세지 동록.
//        /// </summary>
//        public void RegistPushMessage (LocalPushCallType pushCallType, string title, string description, float fireSeconds = 0f)
//        {
//            var fireTime = DateTime.Now.AddSeconds (fireSeconds);
//            _localPushDict[pushCallType].LocalPushCallType = pushCallType;
//            _localPushDict[pushCallType].FireTime = fireTime;
//            _localPushDict[pushCallType].Title = title;
//            _localPushDict[pushCallType].Description = description;
//            _bundleSystem.SaveLocalPushData ();
//        }
//
//
//        /// <summary>
//        /// 푸시 메세지 삭제.
//        /// </summary>
//        public void CancelPushMessage (LocalPushCallType pushCallType)
//        {
//#if UNITY_ANDROID && !UNITY_EDITOR
//            var identifier = (int) pushCallType;
//            if (AndroidNotificationCenter.CheckScheduledNotificationStatus (identifier) ==
//                NotificationStatus.Scheduled)
//            {
//                AndroidNotificationCenter.CancelScheduledNotification (identifier);
//            }
//#elif UNITY_IOS
//                if (iOSNotificationCenter.GetScheduledNotifications ()
//                    .Any (x => x.Identifier.Equals (pushCallType.ToString ())))
//                {
//                    iOSNotificationCenter.RemoveScheduledNotification (pushCallType.ToString ());
//                }
//#endif
//            _localPushDict[pushCallType].Clear ();
//            _bundleSystem.SaveLocalPushData ();
//        }
//
//
//        /// <summary>
//        /// 푸시될 시간이 야간 오후 9시에서 오전 8시 사이인지 여부.
//        /// </summary>
//        public static bool CheckNightTime (DateTime dateTime)
//        {
//            return dateTime.Hour >= NightTime.y || dateTime.Hour < NightTime.x;
//        }
//
//    }
//}

