using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

public class Test : MonoBehaviour
{
//    void CheckFirebase()
//    {
//        Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
//
//        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
//            var dependencyStatus = task.Result;
//            if (dependencyStatus == Firebase.DependencyStatus.Available) {
//                // Create and hold a reference to your FirebaseApp,
//                // where app is a Firebase.FirebaseApp property of your application class.
//                var app = Firebase.FirebaseApp.DefaultInstance;
//
//                // Set a flag here to indicate whether Firebase is ready to use by your app.
//            } else {
//                UnityEngine.Debug.LogError(System.String.Format(
//                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
//                // Firebase Unity SDK is not safe to use here.
//            }
//        });
//    }

    private void InitBugly()
    {
#if DEBUG
        // 开启SDK的日志打印，发布版本请务必关闭
        BuglyAgent.ConfigDebugMode (true); 
#endif
      
        // 注册日志回调，替换使用 'Application.RegisterLogCallback(Application.LogCallback)'注册日志回调的方式
        // BuglyAgent.RegisterLogCallback (CallbackDelegate.Instance.OnApplicationLogCallbackHandler);

#if UNITY_IPHONE || UNITY_IOS
        BuglyAgent.InitWithAppId ("Your App ID");
#elif UNITY_ANDROID
        BuglyAgent.InitWithAppId ("bb92e8b47d");
#endif

        // 如果你确认已在对应的iOS工程或Android工程中初始化SDK，那么在脚本中只需启动C#异常捕获上报功能即可
        BuglyAgent.EnableExceptionHandler ();
    }

    void Start()
    {
//        CheckFirebase();
        InitBugly();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerNativeSoCrash()
    {
        Debug.LogWarning("UnityCrashTest : OnTriggerNativeSoCrash");

        Crash();
    }

    public void OnTriggerNativeCrash()
    {
        Debug.LogWarning("UnityCrashTest : OnTriggerNativeCrash");
        Task.Run(() =>
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            intentClass.Call("startActivity", intentClass);
        });
    }
    
    public void OnTriggerJavaCrash()
    {
        Debug.LogWarning("UnityCrashTest : OnTriggerJavaCrash");
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.res.Configuration");
        currentActivity.Call("onConfigurationChanged", null);
    }
    
    public void OnTriggerCSharpCrash()
    {
        Debug.LogWarning("UnityCrashTest : OnTriggerCSharpCrash");
        
        throw new Exception("Test crash");
    }


    [DllImport("testCrash")]
    private static extern void Crash();

}
