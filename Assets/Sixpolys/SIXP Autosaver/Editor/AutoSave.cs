using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Timers;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using LOG = UnityEngine.Debug;
#if UNITY_5_3
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif

[InitializeOnLoad]
public class AutoSave : EditorWindow
{
	public static AutoSave instance = null;
	public static Texture2D logo = null;
	protected static System.Timers.Timer timer = null;
	protected static int hierarchyChangeCount = 0;
	protected static string logoPath = "Assets/Sixpolys/SIXP Autosaver/Editor/SixpolysLogo.png";
	protected static bool _saveNow = false;
	protected static bool savedBeforePlay = false;
	protected static bool saveAfterPlay = false;
	protected static Stopwatch stw1 = null;

	[MenuItem("Window/Autosave Settings")]
	public static void ShowWindow ()
	{
		var window = EditorWindow.GetWindow<AutoSave> ();
		window.maxSize = new Vector2 (window.maxSize.x, 50);
		window.minSize = new Vector2 (0, 50);
	}

	public static void LoadPreferences ()
	{
		if (AutoSavePreferences.autosaveEnabled) {
			if (timer == null) {
				timer = new System.Timers.Timer ();
				timer.Interval = AutoSavePreferences.saveInterval;
				timer.Elapsed += new  ElapsedEventHandler (timerFired);
				timer.Start ();
			} else {
				if (timer.Interval != AutoSavePreferences.saveInterval) {
					timer.Interval = AutoSavePreferences.saveInterval;
				}
			}
		} else {
			if (timer != null) {
				timer.Stop ();
				timer.Dispose ();
				timer = null;
			}
		}
		EditorApplication.hierarchyWindowChanged -= HierarchyChanged;
		EditorApplication.playmodeStateChanged -= playModeChanged;
		EditorApplication.hierarchyWindowChanged += HierarchyChanged;
		EditorApplication.playmodeStateChanged += playModeChanged;

		if (instance != null) {
			instance.Repaint ();
		}

	}


	public static void playModeChanged ()
	{
		if (AutoSavePreferences.saveBeforeRun && EditorApplication.isPlayingOrWillChangePlaymode && !savedBeforePlay) {
			savedBeforePlay = true;
			executeSave ();
		} else if (!EditorApplication.isPaused && !EditorApplication.isPlaying) {
			if (saveAfterPlay) {
				executeSave ();
			}
		}
	}

	public static void HierarchyChanged ()
	{
		if (AutoSavePreferences.saveOnHierarchyChanges && !EditorApplication.isPlaying) {
			hierarchyChangeCount++;
			if (hierarchyChangeCount >= AutoSavePreferences.hierarchyChangeCountTrigger) {
				hierarchyChangeCount = 0;
				executeSave ();
			}
		}
	}

	public static void timerFired (object sender, ElapsedEventArgs args)
	{
		if (!_saveNow) {
			_saveNow = true;
		}
	}
	
	public static void executeSave ()
	{
		stw1.Stop ();
		stw1.Reset ();

		if (EditorApplication.isCompiling || BuildPipeline.isBuildingPlayer) {
			return;
		}

		// don't save during running game
		if (EditorApplication.isPlaying || EditorApplication.isPaused) {
			saveAfterPlay = true;
			stw1.Start ();
			return;
		}
		saveAfterPlay = false;

		// save untitled scene?
#if UNITY_5_3
		string sceneName = SceneManager.GetActiveScene ().name;
#else
			string sceneName = EditorApplication.currentScene;
#endif
		if ((sceneName == "" || sceneName.StartsWith ("Untitled")) && !AutoSavePreferences.saveUnnamedNewScene) {
			stw1.Start ();
			return;
		}


		if (AutoSavePreferences.logSaveEvent) {
			LOG.Log ("Autosave");
		}
#if UNITY_5_3
		EditorSceneManager.SaveOpenScenes ();
#else
		EditorApplication.SaveScene ();
#endif
		if (AutoSavePreferences.saveAssets) {
			AssetDatabase.SaveAssets ();
		}
		if (instance != null) {
			instance.Repaint ();
		}
		stw1.Start ();
	}

	[InitializeOnLoadMethod]
	public static void InitAutosave ()
	{
		stw1 = new Stopwatch ();
		stw1.Start ();
		logo = (Texture2D)AssetDatabase.LoadAssetAtPath (logoPath, typeof(Texture2D));
		EditorApplication.update += EditorUpdate;
		AutoSavePreferences.LoadPreferences ();
		LoadPreferences ();
	}

	public static void EditorUpdate ()
	{
		if (_saveNow) {
			_saveNow = false;
			executeSave ();
		}
		if (instance != null) {
			instance.Repaint ();
		}
	}

	public void OnEnable ()
	{
		instance = this;
	}

	void OnGUI ()
	{
		EditorGUILayout.BeginHorizontal ();
		if (logo != null) {
			GUILayout.Label (logo, GUILayout.Width (50));
		}
		EditorGUILayout.BeginVertical ();
		bool autosaveEnabled = GUILayout.Toggle (AutoSavePreferences.autosaveEnabled, "Autosave", GUILayout.ExpandWidth (true));

		EditorGUILayout.LabelField ("Last saved: " + Math.Floor (stw1.Elapsed.TotalMinutes) + " minutes ago");
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();

		if (GUI.changed) {
			AutoSavePreferences.autosaveEnabled = autosaveEnabled;
			AutoSavePreferences.SavePreferences ();

		}
	}	
}
