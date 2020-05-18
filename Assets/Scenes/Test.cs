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

    void Start()
    {
//        CheckFirebase();
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
