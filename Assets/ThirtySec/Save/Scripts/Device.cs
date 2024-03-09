
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThirtySec
{
    [System.Flags]
    public enum LogType
    {
        Info = 0x01 << 0x00,
        Error = 0x01 << 0x01,
    }

    [System.Flags]
    public enum SaveEvent
    {
        OnQuit = 01,
        OnPause = 10,

    }




    public class Device : ThirtySec.ScriptableSingleton<Device>
    {

#if UNITY_EDITOR
        [MenuItem("ThirtySec/Settings")]
        public static void FileSetting()
        {
            ThirtySec.Scriptable.CreateAsset<Device>("Assets/ThirtySec/Save/Resources/");
        }

#endif





        [Header("Log file path and status")]
        [HideWhenPlay]
        [EnumFlags]
        public LogType logType = LogType.Info;


        [Header("File will automatic save in OnApplicationPause and/or OnApplicationQuit")]
        [HideWhenPlay]
        [EnumFlags]
        public SaveEvent saveEvent = SaveEvent.OnPause | SaveEvent.OnQuit;

        [HideWhenPlay, Header("Directory to stores file in the Unity Editor, persistentDataPath on other platforms")]
        public string fileDirectory = "/ThirtySec/File/Files";

        [HideWhenPlay, Header("You can set anything you want like .js, .xml, .cat, .dog")]
        public string fileType = ".txt";

        [HideWhenPlay]
        [Header("Should the file encrypt")]
        public bool encrypt = true;


        public static string filePath<T>() where T : ISerializable
        {
            return (Path.Combine(path(instance.fileDirectory), string.Format("Ardodev_{0}", typeof(T).Name) + (instance.fileType)));
        }
        public static string filePath(string fileName)
        {
            return (Path.Combine(path(instance.fileDirectory), fileName));
        }


        public static string path(string directory)
        {

#if !UNITY_EDITOR
		return Application.persistentDataPath;
#else
            if (!Directory.Exists(Application.dataPath + directory))
                Directory.CreateDirectory(Application.dataPath + directory);
            return Application.dataPath + directory;
#endif

        }
    }
}
