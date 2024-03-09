
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ThirtySec
{
	public static class Scriptable
	{

		public static void CreateAsset<T>(string dir, string name = null, bool multipler = false, int offset = 0) where T : ScriptableObject
		{
			string mult = multipler ? (Resources.LoadAll<T>("").Length + offset).ToString() : "";
			string assetPathAndName = dir + (name == null ? typeof(T).Name : name) + mult + ".asset";
			if (!System.IO.File.Exists(assetPathAndName))
			{
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);


				T asset = ScriptableObject.CreateInstance<T>();
				AssetDatabase.CreateAsset(asset, assetPathAndName);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				Selection.activeObject = asset;
				Debug.Log("Asset created!" + assetPathAndName);
			}
			else
				Debug.Log("File Exists: " + assetPathAndName);
			Selection.activeObject = Resources.Load<T>(typeof(T).Name);

			//EditorUtility.FocusProjectWindow ();

		}
	
		public static void CreateInitiator<T>() where T : Component
		{
			var ins = GameObject.FindObjectOfType<T>();
			if (ins == null)
				ins = new GameObject(typeof(T).Name).AddComponent<T>();

			Selection.activeObject = ins;
			EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
		}
	}
}
#endif