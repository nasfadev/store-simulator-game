using UnityEngine;
using System.Collections.Generic;
using Lean.Common;
using CW.Common;

namespace Lean.Localization
{
	/// <summary>This component manages a global list of translations for easy access.
	/// Translations are gathered from the <b>prefabs</b> list, as well as from any active and enabled <b>LeanSource</b> components in the scene.</summary>
	[ExecuteInEditMode]
	[HelpURL(HelpUrlPrefix + "LeanLocalization")]
	[AddComponentMenu(ComponentPathPrefix + "Localization")]
	public class LeanLocalization : MonoBehaviour
	{
		public enum DetectType
		{
			None,
			SystemLanguage,
			CurrentCulture,
			CurrentUICulture
		}

		public enum SaveLoadType
		{
			None,
			WhenChanged,
			WhenChangedAlt
		}

		public const string HelpUrlPrefix = LeanCommon.HelpUrlPrefix + "LeanLocalization#";

		public const string ComponentPathPrefix = "Lean/Localization/Lean ";

		/// <summary>All active and enabled LeanLocalization components.</summary>
		public static List<LeanLocalization> Instances = new List<LeanLocalization>();

		public static Dictionary<string, LeanToken> CurrentTokens = new Dictionary<string, LeanToken>();

		public static Dictionary<string, LeanLanguage> CurrentLanguages = new Dictionary<string, LeanLanguage>();

		public static Dictionary<string, string> CurrentAliases = new Dictionary<string, string>();

		/// <summary>Dictionary of all the phrase names mapped to their current translations.</summary>
		public static Dictionary<string, LeanTranslation> CurrentTranslations = new Dictionary<string, LeanTranslation>();

		/// <summary>The language that is currently being used by this instance.</summary>
		[LeanLanguageName]
		[SerializeField]
		private string currentLanguage;

		/// <summary>How should the cultures be used to detect the user's device language?</summary>
		public DetectType DetectLanguage { set { detectLanguage = value; } get { return detectLanguage; } } [SerializeField] private DetectType detectLanguage = DetectType.SystemLanguage;

		/// <summary>If the application is started and no language has been loaded or auto detected, this language will be used.</summary>
		public string DefaultLanguage { set { defaultLanguage = value; } get { return defaultLanguage; } } [SerializeField] [LeanLanguageName] private string defaultLanguage;

		/// <summary>This allows you to control if/how this component's <b>CurrentLanguage</b> setting should save/load.
		/// None = Only the <b>DetectLanguage</b> and <b>DefaultLanguage</b> settings will be used.
		/// WhenChanged = If the <b>CurrentLanguage</b> gets manually changed, automatically save/load it to PlayerPrefs?
		/// 
		/// NOTE: This save data can be cleared with <b>ClearSave</b> context menu option.</summary>
		public SaveLoadType SaveLoad { set { saveLoad = value; } get { return saveLoad; } } [SerializeField] private SaveLoadType saveLoad = SaveLoadType.WhenChanged;

		/// <summary>This stores all prefabs and folders managed by this LeanLocalization instance.</summary>
		public List<LeanPrefab> Prefabs { get { if (prefabs == null) prefabs = new List<LeanPrefab>(); return prefabs; } } [SerializeField] private List<LeanPrefab> prefabs;

		/// <summary>Called when the language or translations change.</summary>
		public static event System.Action OnLocalizationChanged;

		private static bool pendingUpdates;

		private static Dictionary<string, LeanTranslation> tempTranslations = new Dictionary<string, LeanTranslation>();

		private static List<LeanSource> tempSources = new List<LeanSource>(1024);

		/// <summary>Change the current language of this instance?</summary>
		public string CurrentLanguage
		{
			set
			{
				if (currentLanguage != value)
				{
					currentLanguage = value;

					if (saveLoad != SaveLoadType.None)
					{
						SaveNow();
					}

					UpdateTranslations();
				}
			}

			get
			{
				return currentLanguage;
			}
		}

		/// <summary>When rebuilding translations this method is called from any <b>LeanSource</b> components that define a token.</summary>
		public static void RegisterToken(string name, LeanToken token)
		{
			if (string.IsNullOrEmpty(name) == false && token != null && CurrentTokens.ContainsKey(name) == false)
			{
				CurrentTokens.Add(name, token);
			}
		}

		/// <summary>When rebuilding translations this method is called from any <b>LeanSource</b> components that define a transition.</summary>
		public static LeanTranslation RegisterTranslation(string name)
		{
			var translation = default(LeanTranslation);

			if (string.IsNullOrEmpty(name) == false && CurrentTranslations.TryGetValue(name, out translation) == false)
			{
				if (tempTranslations.TryGetValue(name, out translation) == true)
				{
					tempTranslations.Remove(name);

					CurrentTranslations.Add(name, translation);
				}
				else
				{
					translation = new LeanTranslation(name);

					CurrentTranslations.Add(name, translation);
				}
			}

			return translation;
		}

		[ContextMenu("Clear Save")]
		public void ClearSave()
		{
			PlayerPrefs.DeleteKey("LeanLocalization.CurrentLanguage");

			PlayerPrefs.Save();
		}

		[ContextMenu("Clear Save Alt")]
		public void ClearSaveAlt()
		{
			PlayerPrefs.DeleteKey("LeanLocalization.CurrentLanguageAlt");

			PlayerPrefs.Save();
		}

		private void SaveNow()
		{
			if (saveLoad == SaveLoadType.WhenChanged)
			{
				PlayerPrefs.SetString("LeanLocalization.CurrentLanguage", currentLanguage);
			}
			else if (saveLoad == SaveLoadType.WhenChangedAlt)
			{
				PlayerPrefs.SetString("LeanLocalization.CurrentLanguageAlt", currentLanguage);
			}

			PlayerPrefs.Save();
		}

		private void LoadNow()
		{
			if (saveLoad == SaveLoadType.WhenChanged)
			{
				currentLanguage = PlayerPrefs.GetString("LeanLocalization.CurrentLanguage");
			}
			else if (saveLoad == SaveLoadType.WhenChangedAlt)
			{
				currentLanguage = PlayerPrefs.GetString("LeanLocalization.CurrentLanguageAlt");
			}
		}

		/// <summary>This sets the current language using the specified language name.</summary>
		public void SetCurrentLanguage(string newLanguage)
		{
			CurrentLanguage = newLanguage;
		}

		/// <summary>This sets the current language of all instances using the specified language name.</summary>
		public static void SetCurrentLanguageAll(string newLanguage)
		{
			foreach (var instance in Instances)
			{
				instance.CurrentLanguage = newLanguage;
			}
		}

		/// <summary>This returns the <b>CurrentLanguage</b> value from the first <b>LeanLocalization</b> instance in the scene if it exists, or null.</summary>
		public static string GetFirstCurrentLanguage()
		{
			if (Instances.Count > 0)
			{
				return Instances[0].CurrentLanguage;
			}

			return null;
		}

		public static LeanLocalization GetOrCreateInstance()
		{
			if (Instances.Count == 0)
			{
				new GameObject("LeanLocalization").AddComponent<LeanLocalization>();
			}

			return Instances[0];
		}

		/// <summary>This adds the specified UnityEngine.Object to this LeanLocalization instance, allowing it to be registered as a prefab.</summary>
		public void AddPrefab(Object root)
		{
			for (var i = Prefabs.Count - 1; i >= 0; i--) // NOTE: Property
			{
				if (prefabs[i].Root == root)
				{
					return;
				}
			}

			var prefab = new LeanPrefab();

			prefab.Root = root;

			prefabs.Add(prefab);
		}

		/// <summary>This calls <b>AddLanguage</b> on the first active and enabled LeanLocalization instance, or creates one first.</summary>
		public static LeanLanguage AddLanguageToFirst(string name)
		{
			return GetOrCreateInstance().AddLanguage(name);
		}

		/// <summary>This creates a new token with the specified name, and adds it to the current GameObject.</summary>
		public LeanLanguage AddLanguage(string name)
		{
			if (string.IsNullOrEmpty(name) == false)
			{
				var root     = new GameObject(name);
				var language = root.AddComponent<LeanLanguage>();

				root.transform.SetParent(transform, false);

				return language;
			}

			return null;
		}

		/// <summary>This calls <b>AddToken</b> on the first active and enabled LeanLocalization instance, or creates one first.</summary>
		public static LeanToken AddTokenToFirst(string name)
		{
			return GetOrCreateInstance().AddToken(name);
		}

		/// <summary>This creates a new token with the specified name, and adds it to the current GameObject.</summary>
		public LeanToken AddToken(string name)
		{
			if (string.IsNullOrEmpty(name) == false)
			{
				var root  = new GameObject(name);
				var token = root.AddComponent<LeanToken>();

				root.transform.SetParent(transform, false);

				return token;
			}

			return null;
		}

		/// <summary>This allows you to set the value of the token with the specified name.
		/// If no token exists and allowCreation is enabled, then one will be created for you.</summary>
		public static void SetToken(string name, string value, bool allowCreation = true)
		{
			if (string.IsNullOrEmpty(name) == false)
			{
				var token = default(LeanToken);

				if (CurrentTokens.TryGetValue(name, out token) == true)
				{
					token.Value = value;
				}
				else if (allowCreation == true)
				{
					token = AddTokenToFirst(name);

					token.Value = value;
				}
			}
		}

		/// <summary>This allows you to get the value of the token with the specified name.
		/// If no token exists, then the defaultValue will be returned.</summary>
		public static string GetToken(string name, string defaultValue = null)
		{
			var token = default(LeanToken);

			if (string.IsNullOrEmpty(name) == false)
			{
				if (CurrentTokens.TryGetValue(name, out token) == true)
				{
					return token.Value;
				}
			}

			return defaultValue;
		}

		/// <summary>This calls <b>AddPhrase</b> on the first active and enabled LeanLocalization instance, or creates one first.</summary>
		public static LeanPhrase AddPhraseToFirst(string name)
		{
			return GetOrCreateInstance().AddPhrase(name);
		}

		/// <summary>This creates a new phrase with the specified name, and adds it to the current GameObject.</summary>
		public LeanPhrase AddPhrase(string name)
		{
			if (string.IsNullOrEmpty(name) == false)
			{
				var root   = new GameObject(name);
				var phrase = root.AddComponent<LeanPhrase>();

				root.transform.SetParent(transform, false);

				return phrase;
			}

			return null;
		}

		/// <summary>This will return the translation with the specified name, or null if none was found.</summary>
		public static LeanTranslation GetTranslation(string name)
		{
			var translation = default(LeanTranslation);

			if (string.IsNullOrEmpty(name) == false)
			{
				CurrentTranslations.TryGetValue(name, out translation);
			}

			return translation;
		}

		/// <summary>This will return the translated string with the specified name, or the fallback if none is found.</summary>
		public static string GetTranslationText(string name, string fallback = null, bool replaceTokens = true)
		{
			var translation = default(LeanTranslation);

			if (string.IsNullOrEmpty(name) == false && CurrentTranslations.TryGetValue(name, out translation) == true && translation.Data is string)
			{
				fallback = (string)translation.Data;
			}

			if (replaceTokens == true)
			{
				fallback = LeanTranslation.FormatText(fallback);
			}

			return fallback;
		}

		/// <summary>This will return the translated UnityEngine.Object with the specified name, or the fallback if none is found.</summary>
		public static T GetTranslationObject<T>(string name, T fallback = null)
			where T : Object
		{
			var translation = default(LeanTranslation);

			if (string.IsNullOrEmpty(name) == false && CurrentTranslations.TryGetValue(name, out translation) == true && translation.Data is T)
			{
				return (T)translation.Data;
			}

			return fallback;
		}

		/// <summary>This rebuilds the dictionary used to quickly map phrase names to translations for the current language.</summary>
		public static void UpdateTranslations(bool forceUpdate = true)
		{
			if (pendingUpdates == true || forceUpdate == true)
			{
				pendingUpdates = false;

				// Copy previous translations to temp dictionary
				tempTranslations.Clear();

				foreach (var pair in CurrentTranslations)
				{
					var translation = pair.Value;

					translation.Clear();

					tempTranslations.Add(pair.Key, translation);
				}

				// Clear currents
				CurrentTokens.Clear();
				CurrentLanguages.Clear();
				CurrentAliases.Clear();
				CurrentTranslations.Clear();

				// Rebuild all currents
				foreach (var instance in Instances)
				{
					instance.RegisterAll();
				}

				// Notify changes?
				if (OnLocalizationChanged != null)
				{
					OnLocalizationChanged();
				}
			}
		}

		/// <summary>If you call this method, then UpdateTranslations will be called next Update.</summary>
		public static void DelayUpdateTranslations()
		{
			pendingUpdates = true;

#if UNITY_EDITOR
			// Go through all enabled phrases
			for (var i = 0; i < Instances.Count; i++)
			{
			//	UnityEditor.EditorUtility.SetDirty(Instances[i].gameObject);
			}
#endif
		}

		/// <summary>Set the instance, merge old instance, and update translations.</summary>
		protected virtual void OnEnable()
		{
			Instances.Add(this);

			UpdateTranslations();
		}

		/// <summary>Unset instance?</summary>
		protected virtual void OnDisable()
		{
			Instances.Remove(this);

			UpdateTranslations();
		}

		protected virtual void Update()
		{
			UpdateTranslations(false);
		}

#if UNITY_EDITOR
		// Inspector modified?
		protected virtual void OnValidate()
		{
			UpdateTranslations();
		}
#endif

		private void RegisterAll()
		{
			GetComponentsInChildren(tempSources);

			// First pass
			if (prefabs != null)
			{
				foreach (var prefab in prefabs)
				{
					foreach (var source in prefab.Sources)
					{
						source.Register();
					}
				}
			}

			foreach (var source in tempSources)
			{
				source.Register();
			}

			// Update language (depends on first pass)
			UpdateCurrentLanguage();

			// Second pass
			if (prefabs != null)
			{
				foreach (var prefab in prefabs)
				{
					foreach (var source in prefab.Sources)
					{
						source.Register(currentLanguage, defaultLanguage);
					}
				}
			}

			foreach (var source in tempSources)
			{
				source.Register(currentLanguage, defaultLanguage);
			}

			tempSources.Clear();
		}

		private void UpdateCurrentLanguage()
		{
			// Load saved language?
			if (saveLoad != SaveLoadType.None)
			{
				LoadNow();
			}

			// Find language by culture?
			if (string.IsNullOrEmpty(currentLanguage) == true)
			{
				switch (detectLanguage)
				{
					case DetectType.SystemLanguage:
					{
						CurrentAliases.TryGetValue(Application.systemLanguage.ToString(), out currentLanguage);
					}
					break;

					case DetectType.CurrentCulture:
					{
						var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

						if (cultureInfo != null)
						{
							CurrentAliases.TryGetValue(cultureInfo.Name, out currentLanguage);
						}
					}
					break;

					case DetectType.CurrentUICulture:
					{
						var cultureInfo = System.Globalization.CultureInfo.CurrentUICulture;

						if (cultureInfo != null)
						{
							CurrentAliases.TryGetValue(cultureInfo.Name, out currentLanguage);
						}
					}
					break;
				}
			}

			// Use default language?
			if (string.IsNullOrEmpty(currentLanguage) == true)
			{
				currentLanguage = defaultLanguage;
			}
		}

#if UNITY_EDITOR
		/// <summary>This exports all text phrases in the LeanLocalization component for the Language specified by this component.</summary>
		[ContextMenu("Export CurrentLanguage To CSV (Comma Format)")]
		private void ExportTextAsset()
		{
			if (string.IsNullOrEmpty(currentLanguage) == false)
			{
				// Find where we want to save the file
				var path = UnityEditor.EditorUtility.SaveFilePanelInProject("Export Text Asset for " + currentLanguage, currentLanguage, "csv", "");

				// Make sure we didn't cancel the panel
				if (string.IsNullOrEmpty(path) == false)
				{
					DoExportTextAsset(path);
				}
			}
		}

		private void DoExportTextAsset(string path)
		{
			var data = "";
			var gaps = false;

			// Add all phrase names and existing translations to lines
			foreach (var pair in CurrentTranslations)
			{
				var translation = pair.Value;

				if (gaps == true)
				{
					data += System.Environment.NewLine;
				}

				data += pair.Key + ",\"";
				gaps  = true;

				if (translation.Data is string)
				{
					var text = (string)translation.Data;

					// Replace all new line permutations with the new line token
					text = text.Replace("\r\n", "\n");
					text = text.Replace("\n\r", "\n");
					text = text.Replace("\r", "\n");

					data += text;
				}

				data += "\"";
			}

			// Write text to file
			using (var file = System.IO.File.OpenWrite(path))
			{
				var encoding = new System.Text.UTF8Encoding();
				var bytes    = encoding.GetBytes(data);

				file.Write(bytes, 0, bytes.Length);
			}

			// Import asset into project
			UnityEditor.AssetDatabase.ImportAsset(path);

			// Replace Source with new Text Asset?
			var textAsset = (TextAsset)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));

			if (textAsset != null)
			{
				UnityEditor.EditorGUIUtility.PingObject(textAsset);

				UnityEditor.EditorUtility.SetDirty(this);
			}
		}
#endif
	}
}

#if UNITY_EDITOR
namespace Lean.Localization.Editor
{
	using UnityEditor;
	using TARGET = LeanLocalization;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET))]
	public class LeanLocalization_Editor : CwEditor
	{
		class PresetLanguage
		{
			public string   Name;
			public string[] Cultures;
		}

		private static List<PresetLanguage> presetLanguages = new List<PresetLanguage>();

		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			LeanLocalization.UpdateTranslations();

			if (Draw("currentLanguage", "The language that is currently being used by this instance.") == true)
			{
				Each(tgts, t => t.CurrentLanguage = serializedObject.FindProperty("currentLanguage").stringValue, true);
			}
			Draw("saveLoad", "This allows you to control if/how this component's <b>CurrentLanguage</b> setting should save/load.\n\nNone = Only the <b>DetectLanguage</b> and <b>DefaultLanguage</b> settings will be used.\n\nWhenChanged = If the <b>CurrentLanguage</b> gets manually changed, automatically save/load it to PlayerPrefs?\n\nNOTE: This save data can be cleared with <b>ClearSave</b> context menu option.");

			Separator();

			Draw("detectLanguage", "How should the cultures be used to detect the user's device language?");
			BeginDisabled(true);
				BeginIndent();
					switch (tgt.DetectLanguage)
					{
						case LeanLocalization.DetectType.SystemLanguage:
							EditorGUILayout.TextField("SystemLanguage", Application.systemLanguage.ToString());
						break;
						case LeanLocalization.DetectType.CurrentCulture:
							EditorGUILayout.TextField("CurrentCulture", System.Globalization.CultureInfo.CurrentCulture.ToString());
						break;
						case LeanLocalization.DetectType.CurrentUICulture:
							EditorGUILayout.TextField("CurrentUICulture", System.Globalization.CultureInfo.CurrentUICulture.ToString());
						break;
					}
				EndIndent();
			EndDisabled();
			Draw("defaultLanguage", "If the application is started and no language has been loaded or auto detected, this language will be used.");

			Separator();

			DrawPrefabs(tgt);

			Separator();

			DrawLanguages();

			Separator();

			DrawTokens();

			Separator();

			DrawTranslations();
		}

		private void DrawPrefabs(TARGET tgt)
		{
			var rectA = Reserve();
			var rectB = rectA; rectB.xMin += EditorGUIUtility.labelWidth;
			EditorGUI.LabelField(rectA, "Prefabs", EditorStyles.boldLabel);
			var newPrefab = EditorGUI.ObjectField(rectB, "", default(Object), typeof(Object), false);
			if (newPrefab != null)
			{
				Undo.RecordObject(tgt, "Add Source");

				tgt.AddPrefab(newPrefab);

				DirtyAndUpdate();
			}

			BeginIndent();
				for (var i = 0; i < tgt.Prefabs.Count; i++)
				{
					DrawPrefabs(tgt, i);
				}
			EndIndent();
		}

		private int expandPrefab = -1;

		private void DrawPrefabs(TARGET tgt, int index)
		{
			var rectA   = Reserve();
			var rectB   = rectA; rectB.xMax -= 22.0f;
			var rectC   = rectA; rectC.xMin = rectC.xMax - 20.0f;
			var prefab  = tgt.Prefabs[index];
			var rebuilt = false;
			var expand  = EditorGUI.Foldout(new Rect(rectA.x, rectA.y, 20, rectA.height), expandPrefab == index, "");

			if (expand == true)
			{
				expandPrefab = index;
			}
			else if (expandPrefab == index)
			{
				expandPrefab = -1;
			}

			BeginDisabled(true);
				BeginError(prefab.Root == null);
					EditorGUI.ObjectField(rectB, prefab.Root, typeof(Object), false);
				EndError();
				if (prefab.Root != null)
				{
					Undo.RecordObject(tgt, "Rebuild Sources");

					rebuilt |= prefab.RebuildSources();

					if (expand == true)
					{
						var sources = prefab.Sources;

						BeginIndent();
							foreach (var source in sources)
							{
								EditorGUI.ObjectField(Reserve(), source, typeof(LeanSource), false);
							}
						EndIndent();
					}
				}
			EndDisabled();
			if (rebuilt == true)
			{
				DirtyAndUpdate();
			}
			if (GUI.Button(rectC, "X", EditorStyles.miniButton) == true)
			{
				Undo.RecordObject(tgt, "Remove Prefab");

				tgt.Prefabs.RemoveAt(index);

				DirtyAndUpdate();

				if (expand == true)
				{
					expandPrefab = -1;
				}
			}
		}

		private static string translationFilter;

		private LeanTranslation expandTranslation;

		private void DrawTranslations()
		{
			var rectA = Reserve();
			var rectB = rectA; rectB.xMin += EditorGUIUtility.labelWidth; rectB.xMax -= 37.0f;
			var rectC = rectA; rectC.xMin = rectC.xMax - 35.0f;
			EditorGUI.LabelField(rectA, "Translations", EditorStyles.boldLabel);
			translationFilter = EditorGUI.TextField(rectB, "", translationFilter);
			BeginDisabled(string.IsNullOrEmpty(translationFilter) == true || LeanLocalization.CurrentTranslations.ContainsKey(translationFilter) == true);
				if (GUI.Button(rectC, "Add", EditorStyles.miniButton) == true)
				{
					var phrase = LeanLocalization.AddPhraseToFirst(translationFilter);

					LeanLocalization.UpdateTranslations();

					Selection.activeObject = phrase;

					EditorGUIUtility.PingObject(phrase);
				}
			EndDisabled();

			if (LeanLocalization.CurrentTranslations.Count == 0 && string.IsNullOrEmpty(translationFilter) == true)
			{
				Info("Type in the name of a translation, and click the 'Add' button. Or, drag and drop a prefab that contains some.");
			}
			else
			{
				var total = 0;

				BeginIndent();
					foreach (var pair in LeanLocalization.CurrentTranslations)
					{
						var name = pair.Key;

						if (string.IsNullOrEmpty(translationFilter) == true || name.IndexOf(translationFilter, System.StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							var translation = pair.Value;
							var rectT       = Reserve();
							var expand      = EditorGUI.Foldout(new Rect(rectT.x, rectT.y, 20, rectT.height), expandTranslation == translation, "");

							if (expand == true)
							{
								expandTranslation = translation;
							}
							else if (expandTranslation == translation)
							{
								expandTranslation = null;
							}

							CalculateTranslation(pair.Value);

							var data = translation.Data;

							total++;

							BeginDisabled(true);
								BeginError(missing.Count > 0 || clashes.Count > 0);
									if (data is Object)
									{
										EditorGUI.ObjectField(rectT, name, (Object)data, typeof(Object), true);
									}
									else
									{
										EditorGUI.TextField(rectT, name, data != null ? data.ToString() : "");
									}
								EndError();

								if (expand == true)
								{
									BeginIndent();
										foreach (var entry in translation.Entries)
										{
											BeginError(clashes.Contains(entry.Language) == true);
												EditorGUILayout.ObjectField(entry.Language, entry.Owner, typeof(Object), true);
											EndError();
										}
									EndIndent();
								}
							EndDisabled();

							if (expand == true)
							{
								foreach (var language in missing)
								{
									Warning("This translation isn't defined for the " + language + " language.");
								}

								foreach (var language in clashes)
								{
									Warning("This translation is defined multiple times for the " + language + " language.");
								}
							}
						}
					}
				EndIndent();

				if (total == 0)
				{
					Info("No translation with this name exists, click the 'Add' button to create it.");
				}
			}
		}
		
		private static List<string> missing = new List<string>();

		private static List<string> clashes = new List<string>();

		private static void CalculateTranslation(LeanTranslation translation)
		{
			missing.Clear();
			clashes.Clear();

			foreach (var language in LeanLocalization.CurrentLanguages.Keys)
			{
				if (translation.Entries.Exists(e => e.Language == language) == false)
				{
					missing.Add(language);
				}
			}

			foreach (var entry in translation.Entries)
			{
				var language = entry.Language;

				if (clashes.Contains(language) == false)
				{
					if (translation.LanguageCount(language) > 1)
					{
						clashes.Add(language);
					}
				}
			}
		}

		private static string languagesFilter;

		private void DrawLanguages()
		{
			var rectA = Reserve();
			var rectB = rectA; rectB.xMin += EditorGUIUtility.labelWidth; rectB.xMax -= 37.0f;
			var rectC = rectA; rectC.xMin = rectC.xMax - 35.0f;
			EditorGUI.LabelField(rectA, "Languages", EditorStyles.boldLabel);
			languagesFilter = EditorGUI.TextField(rectB, "", languagesFilter);
			BeginDisabled(string.IsNullOrEmpty(languagesFilter) == true || LeanLocalization.CurrentLanguages.ContainsKey(languagesFilter) == true);
				if (GUI.Button(rectC, "Add", EditorStyles.miniButton) == true)
				{
					var language = LeanLocalization.AddLanguageToFirst(languagesFilter);

					LeanLocalization.UpdateTranslations();

					Selection.activeObject = language;

					EditorGUIUtility.PingObject(language);
				}
			EndDisabled();

			if (LeanLocalization.CurrentLanguages.Count > 0 || string.IsNullOrEmpty(languagesFilter) == false)
			{
				var total = 0;

				BeginIndent();
					BeginDisabled(true);
						foreach (var pair in LeanLocalization.CurrentLanguages)
						{
							if (string.IsNullOrEmpty(languagesFilter) == true || pair.Key.IndexOf(languagesFilter, System.StringComparison.InvariantCultureIgnoreCase) >= 0)
							{
								EditorGUILayout.ObjectField(pair.Key, pair.Value, typeof(Object), true); total++;
							}
						}
					EndDisabled();
				EndIndent();

				if (total == 0)
				{
					EditorGUILayout.HelpBox("No language with this name exists, click the 'Add' button to create it.", MessageType.Info);
				}
			}
		}

		private static string tokensFilter;

		private void DrawTokens()
		{
			var rectA = Reserve();
			var rectB = rectA; rectB.xMin += EditorGUIUtility.labelWidth; rectB.xMax -= 37.0f;
			var rectC = rectA; rectC.xMin = rectC.xMax - 35.0f;
			EditorGUI.LabelField(rectA, "Tokens", EditorStyles.boldLabel);
			tokensFilter = EditorGUI.TextField(rectB, "", tokensFilter);
			BeginDisabled(string.IsNullOrEmpty(tokensFilter) == true || LeanLocalization.CurrentTokens.ContainsKey(tokensFilter) == true);
				if (GUI.Button(rectC, "Add", EditorStyles.miniButton) == true)
				{
					var token = LeanLocalization.AddTokenToFirst(tokensFilter);

					LeanLocalization.UpdateTranslations();

					Selection.activeObject = token;

					EditorGUIUtility.PingObject(token);
				}
			EndDisabled();

			if (LeanLocalization.CurrentTokens.Count > 0 || string.IsNullOrEmpty(tokensFilter) == false)
			{
				var total = 0;

				BeginIndent();
					BeginDisabled(true);
						foreach (var pair in LeanLocalization.CurrentTokens)
						{
							if (string.IsNullOrEmpty(tokensFilter) == true || pair.Key.IndexOf(tokensFilter, System.StringComparison.InvariantCultureIgnoreCase) >= 0)
							{
								EditorGUILayout.ObjectField(pair.Key, pair.Value, typeof(Object), true); total++;
							}
						}
					EndDisabled();
				EndIndent();

				if (total == 0)
				{
					EditorGUILayout.HelpBox("No token with this name exists, click the 'Add' button to create it.", MessageType.Info);
				}
			}
		}

		private void AddLanguage(TARGET tgt, PresetLanguage presetLanguage)
		{
			Undo.RecordObject(tgt, "Add Language");

			tgt.AddLanguage(presetLanguage.Name);

			DirtyAndUpdate();
		}

		private static void AddPresetLanguage(string name, params string[] cultures)
		{
			var presetLanguage = new PresetLanguage();

			presetLanguage.Name     = name;
			presetLanguage.Cultures = cultures;

			presetLanguages.Add(presetLanguage);
		}

		[MenuItem("GameObject/Lean/Localization", false, 1)]
		private static void CreateLocalization()
		{
			var gameObject = new GameObject(typeof(LeanLocalization).Name);

			Undo.RegisterCreatedObjectUndo(gameObject, "Create LeanLocalization");

			gameObject.AddComponent<LeanLocalization>();

			Selection.activeGameObject = gameObject;
		}
	}
}
#endif