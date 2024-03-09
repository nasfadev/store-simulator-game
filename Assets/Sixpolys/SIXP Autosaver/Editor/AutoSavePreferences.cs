using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class AutoSavePreferences
{
	//Settings
	public static bool loaded;
	public static bool autosaveEnabled;
	public static double saveInterval;
	public static bool logSaveEvent;
	public static bool saveAssets;
	public static bool saveOnHierarchyChanges;
	public static int hierarchyChangeCountTrigger;
	public static bool saveBeforeRun;
	public static bool saveUnnamedNewScene;

	#region Default values
	static bool AUTOSAVEENABLED = true;
	static int SAVEINTERVALINDEX = 4;
	static bool LOGSAVEEVENT = true;
	static bool SAVEASSETS = true;
	static bool SAVEONHIERARCHYCHANGES = false;
	static int HIERARCHYCHANGECOUNTTRIGGER = 30;
	static bool SAVEBEFORERUN = true;
	static bool SAVEUNNAMEDNEWSCENE = true;
	#endregion

	#region Keys
	static readonly string AUTOSAVE_ENABLED_KEY = "autosaveEnabled";
	static readonly string LOG_SAVE_EVENT_KEY = "logSaveEvent";
	static readonly string SAVE_INTERVAL_INDEX_KEY = "saveIntervalIndex";
	static readonly string SAVE_ASSETS_KEY = "saveAssets";
	static readonly string SAVE_ON_HIERARCHY_CHANGE_KEY = "saveOnHierarchyChange";
	static readonly string HIERARCHY_CHANGE_COUNT_TRIGGER_KEY = "hierarchyChangeCountTrigger";
	static readonly string SAVE_BEFORE_RUN_KEY = "saveBeforeRun";
	static readonly string SAVE_UNNAMED_NEW_SCENE_KEY = "saveUnnamedNewScene";
	#endregion

	// class values
	static string[] intervalValueStrings = {
		"1 Minute",
		"2 Minutes",
		"3 Minutes",
		"4 Minutes",
		"5 Minutes",
		"10 Minutes",
		"15 Minutes",
		"20 Minutes",
		"30 Minutes",
		"1 hour",
		"2 hours",
		"3 hours",
		"4 hours",
		"5 hours"
	};
	static double[] intervalValues = {
		60000,
		2 * 60000,
		3 * 60000,
		4 * 60000,
		5 * 60000,
		10 * 60000,
		15 * 60000,
		20 * 60000,
		30 * 60000,
		60 * 60000,
		120 * 60000,
		180 * 60000,
		240 * 60000,
		300 * 60000
	};
	static int saveIntervalIndex = 0;

	[PreferenceItem("Autosave")]
	public static void OnGUI ()
	{
		if (!loaded) {
			LoadPreferences ();
		}
		EditorGUILayout.BeginHorizontal ();
		if (AutoSave.logo != null) {
			GUILayout.Label (AutoSave.logo);
		}
		EditorGUILayout.BeginVertical ();
		autosaveEnabled = GUILayout.Toggle (autosaveEnabled, "Enabled");
		logSaveEvent = GUILayout.Toggle (logSaveEvent, "Log Save Events");
		saveIntervalIndex = EditorGUILayout.Popup (saveIntervalIndex, intervalValueStrings);
		EditorGUILayout.EndVertical ();
		EditorGUILayout.EndHorizontal ();
		saveAssets = GUILayout.Toggle (saveAssets, "Also save Assets");
		saveBeforeRun = GUILayout.Toggle (saveBeforeRun, "Save before run");
		saveUnnamedNewScene = GUILayout.Toggle (saveUnnamedNewScene, "Save new unnamed scenes");
		EditorGUILayout.HelpBox ("This will result in a prompt that asks for a name for the scene", MessageType.Info);
	
		saveOnHierarchyChanges = EditorGUILayout.BeginToggleGroup ("Save on hierarchy window changes", saveOnHierarchyChanges);
		EditorGUILayout.PrefixLabel ("Number of changes before save");
		hierarchyChangeCountTrigger = EditorGUILayout.IntSlider (hierarchyChangeCountTrigger, 1, 100);
		EditorGUILayout.EndToggleGroup ();

		if (GUI.changed) {
			SavePreferences ();
		}
	}

	public static void SavePreferences ()
	{
		EditorPrefs.SetBool (AUTOSAVE_ENABLED_KEY, autosaveEnabled);
		EditorPrefs.SetInt (SAVE_INTERVAL_INDEX_KEY, saveIntervalIndex);
		saveInterval = intervalValues [saveIntervalIndex];
		EditorPrefs.SetBool (LOG_SAVE_EVENT_KEY, logSaveEvent);
		EditorPrefs.SetBool (SAVE_ASSETS_KEY, saveAssets);
		EditorPrefs.SetBool (SAVE_ON_HIERARCHY_CHANGE_KEY, saveOnHierarchyChanges);
		EditorPrefs.SetInt (HIERARCHY_CHANGE_COUNT_TRIGGER_KEY, hierarchyChangeCountTrigger);
		EditorPrefs.SetBool (SAVE_BEFORE_RUN_KEY, saveBeforeRun);
		EditorPrefs.SetBool (SAVE_UNNAMED_NEW_SCENE_KEY, saveUnnamedNewScene);
		AutoSave.LoadPreferences ();
	}
	
	public static void LoadPreferences ()
	{
		autosaveEnabled = EditorPrefs.HasKey (AUTOSAVE_ENABLED_KEY) ? EditorPrefs.GetBool (AUTOSAVE_ENABLED_KEY) : AUTOSAVEENABLED;
		saveIntervalIndex = EditorPrefs.HasKey (SAVE_INTERVAL_INDEX_KEY) ? EditorPrefs.GetInt (SAVE_INTERVAL_INDEX_KEY) : SAVEINTERVALINDEX;
		saveInterval = intervalValues [saveIntervalIndex];
		logSaveEvent = EditorPrefs.HasKey (LOG_SAVE_EVENT_KEY) ? EditorPrefs.GetBool (LOG_SAVE_EVENT_KEY) : LOGSAVEEVENT;
		saveAssets = EditorPrefs.HasKey (SAVE_ASSETS_KEY) ? EditorPrefs.GetBool (SAVE_ASSETS_KEY) : SAVEASSETS;
		saveOnHierarchyChanges = EditorPrefs.HasKey (SAVE_ON_HIERARCHY_CHANGE_KEY) ? EditorPrefs.GetBool (SAVE_ON_HIERARCHY_CHANGE_KEY) : SAVEONHIERARCHYCHANGES;
		hierarchyChangeCountTrigger = EditorPrefs.HasKey (HIERARCHY_CHANGE_COUNT_TRIGGER_KEY) ? EditorPrefs.GetInt (HIERARCHY_CHANGE_COUNT_TRIGGER_KEY) : HIERARCHYCHANGECOUNTTRIGGER;
		saveBeforeRun = EditorPrefs.HasKey (SAVE_BEFORE_RUN_KEY) ? EditorPrefs.GetBool (SAVE_BEFORE_RUN_KEY) : SAVEBEFORERUN;
		saveUnnamedNewScene = EditorPrefs.HasKey (SAVE_UNNAMED_NEW_SCENE_KEY) ? EditorPrefs.GetBool (SAVE_UNNAMED_NEW_SCENE_KEY) : SAVEUNNAMEDNEWSCENE;
		loaded = true;
	}



}
