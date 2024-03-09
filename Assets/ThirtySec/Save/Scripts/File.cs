
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Threading;


namespace ThirtySec
{
    /// <summary>
    /// Serializable generic class using singleton 
    /// </summary>
    public class Serializable<T> : Serializable where T : Serializable, new()
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                return _instance == null ? (_instance = File.Open<T>()) : _instance;
            }
        }

        /// <summary>
        /// Writes all modified preferences to disk.
        ///By default Unity writes preferences to disk during OnApplicationQuit(). In cases when the game crashes or otherwise prematuraly exits, you might want to write the PlayerPrefs at sensible 'checkpoints' in your game.This function will write to disk potentially causing a small hiccup, therefore it is not recommended to call during actual gameplay.
        ///Note: There is no need to call this function manually inside OnApplicationQuit().
        ///Note: On Windows Store Apps and Windows Phone 8.1 preferences are saved during application suspend, because there's no application quit event.
        /// </summary>
        public static void Save()
        {
            instance.Save();
        }
        public static bool IsExist()
        {
            string path = Device.filePath(string.Format("NasfaDev_{0}", typeof(T).Name) + Device.instance.fileType);
            return System.IO.File.Exists(path);

        }


    }


    /// <summary>
    /// Base Serializable class
    /// </summary>
	[System.Serializable]
    public abstract class Serializable : ISerializable
    {
        public string path { get; set; }
    }

    /// <summary>
    /// Serializable interface, contains path and cryption state
    /// </summary>
	public interface ISerializable
    {
        string path { get; set; }
    }

    public static class File
    {
        private static void delete(this string path)
        {
            System.IO.File.Delete(path);
        }

        public static T Open<T>(string fileName = "") where T : ISerializable, new()
        {
            string path = Device.filePath(string.Format("NasfaDev_{0}", typeof(T).Name) + fileName + Device.instance.fileType);
            T @out = new T();
            ThirtySec.Scripton<FileDelegate>.Init();

            ThreadStart fileResult = new ThreadStart(() =>
            {
                goto TrySerializable;
            DeserializableFailed:
                if ((Device.instance.logType & LogType.Error) == LogType.Error)
                    Debug.LogError("Can't deserialize, file format not found.\nAttemping create another file... ");

                System.IO.File.Delete(path);

            TrySerializable:
                BinaryFormatter bf = new BinaryFormatter();

                if (System.IO.File.Exists(path))
                {

                    switch (Device.instance.encrypt)
                    {

                        case true:
                            using (FileStream fs = System.IO.File.Open(path, FileMode.Open))
                            {
                                try
                                {

                                    string decryptor = (string)bf.Deserialize(fs);
                                    @out = JsonUtility.FromJson<T>(Cryptography.Decrypt(decryptor));

                                }

                                catch
                                {

                                    //try read raw file
                                    try
                                    {
                                        var json = System.IO.File.ReadAllText(path);
                                        @out = JsonUtility.FromJson<T>(json);
                                    }
                                    catch
                                    {
                                        goto DeserializableFailed;
                                    }


                                    //congrats, it's successful, we shall set the enscript method according to the file.

                                    //Device.instance.encrypt = false;



                                    //check again with 
                                    //goto DeserializableFailed; 
                                }

                            }
                            break;

                        case false:
                            try
                            {
                                var json = System.IO.File.ReadAllText(path);
                                @out = JsonUtility.FromJson<T>(json);
                            }
                            catch
                            {
                                //try with descriptor
                                using (FileStream fs = System.IO.File.Open(path, FileMode.Open))
                                {
                                    try
                                    {

                                        string decryptor = (string)bf.Deserialize(fs);
                                        @out = JsonUtility.FromJson<T>(Cryptography.Decrypt(decryptor));

                                    }

                                    catch
                                    {
                                        goto DeserializableFailed;
                                    }

                                    //congrats, it's successful, we shall set the enscript method according to the file.

                                    //Device.instance.encrypt = true;
                                    //sucess

                                }
                                //goto DeserializableFailed;
                            }

                            break;

                    }


                }
                else
                {
                    switch (Device.instance.encrypt)
                    {
                        case true:
                            using (FileStream fs = System.IO.File.Create(path))
                            {

                                string encryptor = Cryptography.Encrypt(JsonUtility.ToJson(@out));
                                bf.Serialize(fs, encryptor);
                            }
                            break;
                        case false:
                            System.IO.File.WriteAllText(path, JsonUtility.ToJson(@out));
                            break;
                    }

                }
                @out.path = path;
                if (!FileDelegate.instance.openedFiles.Contains(path))
                {
                    FileDelegate.instance.openedFiles.Add(path);

                    if ((Device.instance.saveEvent & SaveEvent.OnQuit) == SaveEvent.OnQuit)
                    {
                        FileDelegate.instance.onQuit += delegate
                        {
                            Thread microThread = new Thread(new ThreadStart(() =>
                            {
                                if ((Device.instance.logType & LogType.Info) == LogType.Info)
                                    Debug.LogFormat("File saved, event {0}\n{1}", "OnApplicationQuit", path);

                                @out.Save();

                            }));
                            microThread.Start();
                            microThread.Join();

                        };
                    }
                    if ((Device.instance.saveEvent & SaveEvent.OnPause) == SaveEvent.OnPause)
                    {

                        FileDelegate.instance.onPause += status =>
                        {
                            Thread microThread = new Thread(new ThreadStart(() =>
                            {
                                if (status)
                                {
                                    if ((Device.instance.logType & LogType.Info) == LogType.Info)
                                        Debug.LogFormat("File saved, event {0}\n{1}", "OnApplicationPause(bool status)", path);

                                    @out.Save();
                                }
                            }));
                            microThread.Start();
                            microThread.Join();


                        };
                    }
                }
            });

            Thread fileThread = new Thread(fileResult);
            fileThread.Start();
            fileThread.Join();
            if ((Device.instance.logType & LogType.Info) == LogType.Info)
                Debug.Log("File Openned.\n" + path);
            return @out;


        }
        public static void Save<T>(this T file) where T : ISerializable
        {

            if (System.IO.File.Exists(file.path))
                file.path.delete();
            Thread fileSave = new Thread(() =>
            {
                switch (Device.instance.encrypt)
                {
                    case true:
                        using (FileStream fs = System.IO.File.Open(file.path, FileMode.Create))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            string cryptor = Cryptography.Encrypt(JsonUtility.ToJson(file));
                            bf.Serialize(fs, cryptor);

                        }
                        break;
                    case false:
                        string data = JsonUtility.ToJson(file);
                        System.IO.File.WriteAllText(file.path, data);
                        break;
                }


            });
            fileSave.Start();
            fileSave.Join();
        }


    }




}