using System;
using System.Runtime.InteropServices;
using Microsoft.AppCenter.Unity.Crashes;
using UnityEngine;

namespace MF.Art.Game
{
    public class AppCenterHelper : MonoBehaviour
    {
        
        [DllImport("google_breakpad")]
        private static extern void setupNativeCrashesListener(string path);

        private static void BindNDKHandler()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
              var minidumpDir = Crashes.GetMinidumpDirectoryAsync();
            setupNativeCrashesListener(minidumpDir.Result);
            
             Debug.Log("####AppCenterHelper Init BindNDKHandler");
#endif
        
        }

        private void Start()
        {
            Invoke(nameof(Init), 2f);
        }

        public void Init()
        {
            Debug.Log("####AppCenterHelper Init");
            Crashes.SetEnabledAsync(true);
            Crashes.ReportUnhandledExceptions(true);
            BindNDKHandler();
        }
    }
}