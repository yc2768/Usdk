﻿using System.Collections.Generic;
using UnityEngine;

namespace Usdk {
    public static class UsdkApi {
        public const string PLATFORM_NAME = "PlatformProxy";
        private static IUsdkApi api = null;
        // public static Usdk _instance = null;
        // public static Usdk Instance
        // {
        //     get
        //     {
        //         if (_instance == null) 
        //             _instance = new Usdk();
        //         return _instance;
        //     }
        // }

        static UsdkApi () {
            //sdk api
            if (Application.platform == RuntimePlatform.Android)
                api = new UsdkAndroidApi ();
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                api = new UsdkiOSApi ();
            else
                api = new UsdkWindowsApi ();

            //sdk callback
            UsdkCallBack callBack = UsdkCallBack.Create ();
            callBack.OnInitSDKCallBack = OnInitSDKCallBack;
            callBack.OnLoginCallBack = OnLoginCallBack;
            callBack.OnLogoutCallBack = OnLogoutCallBack;
            callBack.OnExitGameCallBack = OnExitGameCallBack;
            callBack.OnPayCallBack = OnPayCallBack;
            SetSdkCallBackReceiver (callBack.gameObject.name);
        }

        #region sdk api
        public static void SetSdkCallBackReceiver (string receiverName) {
            if (string.IsNullOrEmpty (receiverName))
                return;
            CallPugin (PLATFORM_NAME, "setSdkCallBackReceiver", receiverName);
        }

        public static string GetConfig (string key) {
            if (key == null)
                return string.Empty;
            return CallPugin<string> (PLATFORM_NAME, "getConfig", key);
        }

        public static void Login () {
            CallPugin (PLATFORM_NAME, "login");
        }

        public static void Logout () {
            CallPugin (PLATFORM_NAME, "logout");
        }

        public static void Pay (SdkPayInfo payInfo) {
            CallPugin (PLATFORM_NAME, "pay", payInfo.ToString ());
        }

        public static void Quit () {
            CallPugin (PLATFORM_NAME, "exitGame");
        }

        public static void OpenUserCenter () {
            CallPugin (PLATFORM_NAME, "openUserCenter");
        }

        public static void SwitchAccount () {
            CallPugin (PLATFORM_NAME, "switchAccount");
        }

        public static void OpenAppstoreComment (string appid) {
            CallPugin (PLATFORM_NAME, "openAppstoreComment", appid);
        }

        public static void ReleaseSdkResource () {
            CallPugin (PLATFORM_NAME, "releaseSdkResource");
        }

        public static void CallPugin (string pluginName, string methodName, params string[] parameters) {
            api.CallPugin (pluginName, methodName, parameters);
        }

        public static R CallPugin<R> (string pluginName, string methodName, params string[] parameters) {
            return api.CallPugin<R> (pluginName, methodName, parameters);
        }
        #endregion

        #region sdk callback
        private static void OnPayCallBack (int errorCode, List<string> msg) {
            if (errorCode == (int) UsdkCallBackErrorCode.InitSuccess) {
                //初始化成功
                Debug.Log ("sdk init success");
            } else if (errorCode == (int) UsdkCallBackErrorCode.InitFail) {
                //初始化失败
                Debug.Log ("sdk init failed");
            }
        }

        private static void OnExitGameCallBack (int errorCode, List<string> msg) {
            Debug.Log ("exit game");
            if (errorCode == (int) UsdkCallBackErrorCode.ExitNoChannelExiter) {
                //SDK无退出页，需要调用游戏内部退出页
                Application.Quit ();
            } else if (errorCode == (int) UsdkCallBackErrorCode.ExitSuccess) {
                //SDK自带退出页并且已经确认退出
                Application.Quit ();
            }
        }

        private static void OnLogoutCallBack (int errorCode, List<string> msg) {
            if (errorCode == (int) UsdkCallBackErrorCode.LogoutFinish) {
                //SDK退出成功
                Debug.Log ("logout success");
            }
        }

        private static void OnLoginCallBack (int errorCode, List<string> msg) {
            if (errorCode == (int) UsdkCallBackErrorCode.LoginSuccess) {
                //SDK登录成功
                Debug.Log ("login success");
            } else if (errorCode == (int) UsdkCallBackErrorCode.LoginCancel) {
                //SDK取消登录
                Debug.Log ("login cancel");
            } else if (errorCode == (int) UsdkCallBackErrorCode.LoginFail) {
                //SDK登录失败
                Debug.Log ("login failed");
            }
        }

        private static void OnInitSDKCallBack (int errorCode, List<string> msg) {
            if (errorCode == (int) UsdkCallBackErrorCode.PaySuccess) {
                //SDK支付成功
                Debug.Log ("pay success");
            } else if (errorCode == (int) UsdkCallBackErrorCode.PayCancel) {
                //SDK取消支付
                Debug.Log ("pay cancel");
            } else if (errorCode == (int) UsdkCallBackErrorCode.PayProgress) {
                //SDK支付进行中
                Debug.Log ("pay progress");
            } else if (errorCode == (int) UsdkCallBackErrorCode.PayOthers) {
                //SDK其他支付错误码
                Debug.Log ("pay other errorcode");
            } else if (errorCode == (int) UsdkCallBackErrorCode.PayFail) {
                //SDK支付失败
                Debug.Log ("pay failed");
            }
        }
        #endregion
    }
}