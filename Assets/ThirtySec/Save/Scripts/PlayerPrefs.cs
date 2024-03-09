using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace ThirtySec
{
	[Serializable]
	public sealed class PlayerPrefs : ThirtySec.Serializable<PlayerPrefs>
	{
		[Serializable]
		public class IntPrefs : KeyValuePair<int> { }
		[Serializable]
		public class StringPrefs : KeyValuePair<string> { }
		[Serializable]
		public class BoolPrefs : KeyValuePair<bool> { }
		[Serializable]
		public class FloatPrefs : KeyValuePair<float> { }

        
		[Serializable]
		public class KeyValuePair<T>
		{
			public void Clear()
			{
				Keys.Clear();
				Values.Clear();
			}
			public void Delete(string key)
			{
				for (int i = Keys.Count; i >= 0; i--)
				{
					if (Keys[i] == key)
					{
						Keys.RemoveAt(i);
						Values.RemoveAt(i);
						return;
					}
				}

			}
			public bool HasKey(string key)
			{
				for (int i = Keys.Count; i >= 0; i--)
					if (Keys[i] == key)
						return true;

				return false;
			}
			public List<string> Keys = new List<string>();
			public List<T> Values = new List<T>();
			public void InitKey(string key, T defaultValue = default(T))
			{

				if (!Keys.Contains(key))
				{

					Keys.Add(key);
					Values.Add(defaultValue);

					if ((Device.instance.logType & LogType.Info) == LogType.Info)
						Debug.LogFormat("[PlayerPrefs]Created key [{0}]", key);
				}
			}
			public T this[string key]
			{
				get
				{
					for (int i = 0; i < Keys.Count; i++)
					{
						if (Keys[i] == key)
						{
							return Values[i];
						}
					}
					//throw new KeyNotFoundException();
					//No Key, Add New Key
					throw new KeyNotFoundException();


				}
				set
				{
					for (int i = 0; i < Keys.Count; i++)
					{
						if (Keys[i] == key)
						{
							Values[i] = value;
						}
					}
				}
			}

		}

		public IntPrefs Int = new IntPrefs();
		public StringPrefs String = new StringPrefs();
		public FloatPrefs Float = new FloatPrefs();
		public BoolPrefs Bool = new BoolPrefs();

		/// <summary>
		/// Removes all keys and values from the preferences. Use with caution.
		/// Call this function in a script to delete all current settings in the PlayerPrefs. Any values or keys have previously been set up are then reset. Be careful when using this.
		/// </summary>
		public static void DeleteAll()
		{
			instance.Int.Clear();
			instance.Bool.Clear();
			instance.Float.Clear();
			instance.String.Clear();
		}
		/// <summary>
		/// Removes key and its corresponding value from the preferences.
		/// </summary>
		/// <param name="key">Key.</param>
		public static void DeleteKey(string key)
		{
			instance.Int.Delete(key);
			instance.Float.Delete(key);
			instance.String.Delete(key);
			instance.Bool.Delete(key);
		}
		/// <summary>
		/// Returns true if key exists in the preferences.
		/// </summary>
		/// <returns><c>true</c>, if key was hased, <c>false</c> otherwise.</returns>
		public static bool HasKey(string key)
		{
			return instance.Int.HasKey(key) ||
				   instance.Float.HasKey(key) ||
				   instance.String.HasKey(key) ||
				   instance.Bool.HasKey(key);
		}
           
		public static void SetInt(string key, int value)
		{
			instance.Int.InitKey(key);
			instance.Int[key] = value;
		}
		public static int GetInt(string key, int defaultValue = default(int))
		{
			instance.Int.InitKey(key, defaultValue);
			return instance.Int[key];
		}
		public static void SetFloat(string key, float value)
		{
			instance.Float.InitKey(key);
			instance.Float[key] = value;
		}
		public static float GetFloat(string key, float defaultValue = default(float))
		{
			instance.Float.InitKey(key, defaultValue);
			return instance.Float[key];
		}
		public static void SetBool(string key, bool value)
		{
			instance.Bool.InitKey(key);
			instance.Bool[key] = value;
		}
		public static bool GetBool(string key, bool defaultValue = default(bool))
		{
			instance.Bool.InitKey(key, defaultValue);
			return instance.Bool[key];
		}
		public static void SetString(string key, string value)
		{
			instance.String.InitKey(key);
			instance.String[key] = value;
		}
		public static string GetString(string key, string defaultValue = "")
		{
			instance.String.InitKey(key, defaultValue);
			return instance.String[key];
		}
	}
}
