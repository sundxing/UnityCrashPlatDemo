using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Crashlytics;
using UnityEngine;

public class Test : MonoBehaviour
{
    private FirebaseApp app;
    void CheckFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                FirebaseApp.LogLevel = LogLevel.Debug;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                
                LogFireBaseEvents();
        
                Crashlytics.SetCustomKey("Size", "5");
                Crashlytics.SetCustomKey("User", "Jack");
                Crashlytics.SetCustomKey("CurrentPage", "Main");
                Crashlytics.SetCustomKey("Memo", "500");
                Crashlytics.Log("Start");

                Crashlytics.Log("PageShow");
                
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Firebase : Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    private void LogFireBaseEvents()
    {
        // Log an event with no parameters.
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);

// Log an event with a float parameter
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("progress", "percent", 0.4f);

// Log an event with an int parameter.
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent(
                Firebase.Analytics.FirebaseAnalytics.EventPostScore,
                Firebase.Analytics.FirebaseAnalytics.ParameterScore,
                42
            );

// Log an event with a string parameter.
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent(
                Firebase.Analytics.FirebaseAnalytics.EventJoinGroup,
                Firebase.Analytics.FirebaseAnalytics.ParameterGroupId,
                "spoon_welders"
            );

// Log an event with multiple parameters, passed as a struct:
        Firebase.Analytics.Parameter[] LevelUpParameters =
        {
            new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterLevel, 5),
            new Firebase.Analytics.Parameter(
                Firebase.Analytics.FirebaseAnalytics.ParameterCharacter, "mrspoon"),
            new Firebase.Analytics.Parameter(
                "hit_accuracy", 3.14f)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
            LevelUpParameters);
    }

    void Start()
    {
        CheckFirebase();
 
    }

    // Update is called once per frame
    void Update()
    {
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
        Crashlytics.Log("OnTriggerJavaCrash");
        
        Debug.LogWarning("UnityCrashTest : OnTriggerJavaCrash");
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.res.Configuration");
        currentActivity.Call("onConfigurationChanged", null);
    }
    
    public void OnTriggerCSharpCrash()
    {
        Crashlytics.Log("OnTriggerCSharpCrash");

        Debug.LogWarning("UnityCrashTest : OnTriggerCSharpCrash");
        
        throw new Exception("Test crash");
    }
}
