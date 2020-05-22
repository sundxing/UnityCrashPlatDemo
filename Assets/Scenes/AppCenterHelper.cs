using System;
using System.IO;
using System.Text;
using Microsoft.AppCenter.Unity.Crashes;
using UnityEngine;

namespace MF.Art.Game
{
    public class AppCenterHelper : MonoBehaviour
    {

        private static string LogPath;
        public static string GetCrashLogPath()
        {
            return LogPath;
        }
        
        private static void BindNDKHandler()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            var minidumpDir = Crashes.GetMinidumpDirectoryAsync();
            var path = minidumpDir.Result;
            
            
            var crashLogPath = GetCrashLogPath();
            if (!File.Exists(crashLogPath)) {
                File.Create(crashLogPath);
            }

            var jniLoaderClass = new AndroidJavaClass("com.example.libdumplog.JniLoader");

            jniLoaderClass.CallStatic("InitNativeCrashListener",
                new AndroidJavaObject("java.lang.String", path),
                new AndroidJavaObject("java.lang.String", crashLogPath));

            Debug.Log($"####AppCenterHelper Init BindNDKHandler {crashLogPath}");
#endif
        
        }

        private byte[] FileContent(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    byte[] buffur = new byte[fs.Length];
                    fs.Read(buffur, 0, (int)fs.Length);
                    return buffur;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void Awake()
        {
            LogPath = Application.persistentDataPath + "/crash_log.txt";
            
            AppCenterBehavior.Started += OnAppCenterStart;
            
            Crashes.GetErrorAttachments = report =>
            {
 #if !UNITY_ANDROID
 
return null;
#endif
                var path = LogPath;
                if (!File.Exists(path))
                {
                    return null;
                }
                
                var result = new[]
                {
                    ErrorAttachmentLog.AttachmentWithText(File.ReadAllText(path), "CrashLog.txt"),
                };
                
                File.Delete(path);

                return result;
            };
        }

        private void OnAppCenterStart()
        {
            Init();
        }

        public void Init()
        {
            Debug.Log("####AppCenterHelper Init");
            Crashes.SetEnabledAsync(true);
            Crashes.ReportUnhandledExceptions(true, true);
            BindNDKHandler();
        }
    }
}