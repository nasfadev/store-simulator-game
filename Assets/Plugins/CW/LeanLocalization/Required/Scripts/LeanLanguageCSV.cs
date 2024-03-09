using UnityEngine;
using System.Collections.Generic;
using CW.Common;

namespace Lean.Localization
{
	/// <summary>This component will load localizations from a CSV file. By default they should be in the format:
	/// Phrase Name Here = Translation Here // Optional Comment Here
	/// NOTE: This component only handles loading one CSV file from one language. If you have multiple languages, then you must make multiple CSV files for each, and a matching <b>LeanLanguageCSV</b> component to load each.</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanLocalization.HelpUrlPrefix + "LeanLanguageCSV")]
	[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Language CSV")]
	public class LeanLanguageCSV : LeanSource
	{
		[System.Serializable]
		public class Entry
		{
			public string Name;
			public string Text;
		}

		public enum FormatType
		{
			Legacy,
			Comma,
			Semicolon
		}

		public enum CacheType
		{
			LoadImmediately,
			LazyLoad,
			LazyLoadAndUnload,
			LazyLoadAndUnloadPrimaryOnly
		}

		/// <summary>The text asset that contains all the translations.</summary>
		public TextAsset Source;

		/// <summary>The format of the CSV data.
		/// See the inspector for examples of what the different formats look like.</summary>
		public FormatType Format = FormatType.Comma;

		/// <summary>The language of the translations in the source file.</summary>
		[LeanLanguageName]
		public string Language;

		/// <summary>This allows you to control when the CSV file is loaded or unloaded. The lower down you set this, the lower your app's memory usage will be. However, setting it too low means you can miss translations if you haven't translated absolutely every phrase in every language, so I recommend you use <b>LoadImmediately</b> unless you have LOTS of translations.
		/// LoadImmediately = Regardless of the language, the CSV will load when this component activates, and then it will be kept in memory until this component is destroyed.
		/// LazyLoad = The CSV file will only load when the <b>CurrentLanguage</b> or <b>DefaultLanguage</b> matches the CSV language, and then it will be kept in memory until this component is destroyed.
		/// LazyLoadAndUnload = Like <b>LazyLoad</b>, but translations will be unloaded if the <b>CurrentLanguage</b> or <b>DefaultLanguage</b> differs from the CSV language.
		/// LazyLoadAndUnloadPrimaryOnly = Like <b>LazyLoadAndUnload</b>, but only the <b>CurrentLanguage</b> will be used, the <b>DefaultLanguage</b> will be ignored.</summary>
		public CacheType Cache;

		/// <summary>This stores all currently loaded translations from this CSV file.</summary>
		public List<Entry> Entries { get { if (entries == null) entries = new List<Entry>(); return entries; } } [SerializeField] private List<Entry> entries;

		/// <summary>The characters used to separate each translation.</summary>
		private static readonly char[] newlineCharacters = new char[] { '\r', '\n' };

		private static Stack<Entry> entryPool = new Stack<Entry>();

		public override void Register(string primaryLanguage, string defaultLanguage)
		{
			// Lazy load only?
			switch (Cache)
			{
				case CacheType.LazyLoad:
				{
					if (Language != primaryLanguage && Language != defaultLanguage)
					{
						return;
					}
				}
				break;

				case CacheType.LazyLoadAndUnload:
				{
					if (Language != primaryLanguage && Language != defaultLanguage)
					{
						DoClear();

						return;
					}
				}
				break;

				case CacheType.LazyLoadAndUnloadPrimaryOnly:
				{
					if (Language != primaryLanguage)
					{
						DoClear();

						return;
					}
				}
				break;
			}

			if (entries == null || entries.Count == 0)
			{
				if (Application.isPlaying == true)
				{
					DoLoadFromSource();
				}
			}

			if (entries != null)
			{
				for (var i = entries.Count - 1; i >= 0; i--)
				{
					var entry       = entries[i];
					var translation = LeanLocalization.RegisterTranslation(entry.Name);

					translation.Register(Language, this);

					if (Language == primaryLanguage)
					{
						translation.Data    = entry.Text;
						translation.Primary = true;
					}
					else if (Language == defaultLanguage && translation.Primary == false)
					{
						translation.Data = entry.Text;
					}
				}
			}
		}

		/// <summary>This will unload all translations from this component.</summary>
		[ContextMenu("Clear")]
		public void Clear()
		{
			if (entries != null)
			{
				DoClear();

				// Update translations?
				foreach (var localization in LeanLocalization.Instances)
				{
					if (localization.CurrentLanguage == Language)
					{
						LeanLocalization.UpdateTranslations();

						break;
					}
				}
			}
		}

		/// <summary>This will load all translations from the CSV file into this component.</summary>
		[ContextMenu("Load From Source")]
		public void LoadFromSource()
		{
			if (Source != null && string.IsNullOrEmpty(Language) == false)
			{
				DoLoadFromSource();

				// Update translations?
				foreach (var localization in LeanLocalization.Instances)
				{
					if (localization.CurrentLanguage == Language)
					{
						LeanLocalization.UpdateTranslations();

						break;
					}
				}
			}
		}

		private void DoClear()
		{
			if (entries != null)
			{
				entries.Clear();
			}
		}

		private void DoLoadFromSource()
		{
			if (Source != null && string.IsNullOrEmpty(Language) == false)
			{
				for (var i = Entries.Count - 1; i >= 0; i--) // NOTE: Property
				{
					entryPool.Push(entries[i]);
				}

				entries.Clear();

				switch (Format)
				{
					case FormatType.Legacy: LoadLegacy(); break;
					case FormatType.Comma: LoadSeparated(","); break;
					case FormatType.Semicolon: LoadSeparated(";"); break;
				}
			}
		}

		private void LoadLegacy()
		{
			// Split file into lines, and loop through them all
			var lines = Source.text.Split(newlineCharacters, System.StringSplitOptions.RemoveEmptyEntries);

			for (var i = 0; i < lines.Length; i++)
			{
				var line        = lines[i];
				var equalsIndex = line.IndexOf(" = ");

				// Only consider lines with the Separator character
				if (equalsIndex != -1)
				{
					var name = line.Substring(0, equalsIndex).Trim();
					var text = line.Substring(equalsIndex + " = ".Length).Trim();

					// Does this entry have a comment?
					if (string.IsNullOrEmpty(" // ") == false)
					{
						var commentIndex = text.LastIndexOf(" // ");

						if (commentIndex != -1)
						{
							text = text.Substring(0, commentIndex).Trim();
						}
					}

					// Replace newline markers with actual newlines
					if (string.IsNullOrEmpty("\\n") == false)
					{
						text = text.Replace("\\n", System.Environment.NewLine);
					}

					var entry = entryPool.Count > 0 ? entryPool.Pop() : new Entry();

					entry.Name = name;
					entry.Text = text;

					entries.Add(entry);
				}
			}
		}

		private void LoadSeparated(string delimeter)
		{
			var text = Source.text;

			text = text.Replace("\r\n", "\n");
			text = text.Replace("\n\r", "\n");
			text = text.Replace("\r", "\n");

			// Split file into lines, and loop through them all
			var lines = text.Split(newlineCharacters);
			var entry = default(Entry);

			for (var i = 0; i < lines.Length; i++)
			{
				var line = lines[i];

				if (entry == null)
				{
					var delimIndex = line.IndexOf(delimeter);

					if (delimIndex == -1)
					{
						throw new System.InvalidOperationException("The specified CSV file contained an invalid entry on line " + i);
					}

					entry = entryPool.Count > 0 ? entryPool.Pop() : new Entry();

					entry.Name = line.Substring(0, delimIndex);
					entry.Text = line.Substring(delimIndex + 1);

					entries.Add(entry);
				}
				else
				{
					entry.Text += "\n" + line;
				}

				if (entry != null)
				{
					var count = QuoteCount(entry.Text);

					if (count == 0)
					{
						entry = null;
					}
					else if (count % 2 == 0)
					{
						if (entry.Text.StartsWith("\"") == true && entry.Text.EndsWith("\"") == true)
						{
							entry.Text = entry.Text.Substring(1, entry.Text.Length - 2).Replace("\"\"", "\"");

							entry = null;
						}
						else
						{
							throw new System.InvalidOperationException("The specified CSV file contained an invalid entry on line " + i);
						}
					}
				}
			}
		}

		private static int QuoteCount(string s)
		{
			var count = 0;

			foreach (var c in s)
			{
				if (c == '\"')
				{
					count += 1;
				}
			}

			return count;
		}
	}
}

#if UNITY_EDITOR
namespace Lean.Localization.Editor
{
	using UnityEditor;
	using TARGET = LeanLanguageCSV;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(LeanLanguageCSV), true)]
	public class LeanLanguageCSV_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("Source", "The text asset that contains all the translations.");
			Draw("Format", "The format of the CSV data.\n\nSee the inspector for examples of what the different formats look like.");
			Draw("Language", "The language of the translations in the source file.");
			Draw("Cache", "This allows you to control when the CSV file is loaded or unloaded. The lower down you set this, the lower your app's memory usage will be. However, setting it too low means you can miss translations if you haven't translated absolutely every phrase in every language, so I recommend you use <b>LoadImmediately</b> unless you have LOTS of translations.\n\nLoadImmediately = Regardless of the language, the CSV will load when this component activates, and then it will be kept in memory until this component is destroyed.\n\nLazyLoad = The CSV file will only load when the <b>CurrentLanguage</b> or <b>DefaultLanguage</b> matches the CSV language, and then it will be kept in memory until this component is destroyed.\n\nLazyLoadAndUnload = Like <b>LazyLoad</b>, but translations will be unloaded if the <b>CurrentLanguage</b> or <b>DefaultLanguage</b> differs from the CSV language.\n\nLazyLoadAndUnloadPrimaryOnly = Like <b>LazyLoadAndUnload</b>, but only the <b>CurrentLanguage</b> will be used, the <b>DefaultLanguage</b> will be ignored.");

			Separator();

			BeginDisabled(true);
				switch (tgt.Format)
				{
					case LeanLanguageCSV.FormatType.Legacy: UnityEditor.EditorGUILayout.TextArea("Hello = こんにちは // Comment\nCollectItem = アイテム \\n 集めました // Comment here", GUILayout.Height(50)); break;
					case LeanLanguageCSV.FormatType.Comma: UnityEditor.EditorGUILayout.TextArea("Hello,こんにちは\nCollectItem,\"アイテム\n集めました\""); break;
					case LeanLanguageCSV.FormatType.Semicolon: UnityEditor.EditorGUILayout.TextArea("Hello;こんにちは\nCollectItem;\"アイテム\n集めました\""); break;
				}
			EndDisabled();

			Separator();

			UnityEditor.EditorGUILayout.BeginHorizontal();
				if (Any(tgts, t => t.Entries.Count > 0))
				{
					if (GUILayout.Button("Clear") == true)
					{
						Each(tgts, t => t.Clear(), true);
					}
				}
				if (GUILayout.Button("Load Now") == true)
				{
					Each(tgts, t => t.LoadFromSource(), true);
				}
				//if (GUILayout.Button("Export") == true)
				//{
				//	Each(tgts, t => t.ExportTextAsset());
				//}
			UnityEditor.EditorGUILayout.EndHorizontal();

			if (tgts.Length == 1)
			{
				var entries = tgt.Entries;

				if (entries.Count > 0)
				{
					Separator();

					BeginDisabled(true);
						foreach (var entry in entries)
						{
							UnityEditor.EditorGUILayout.TextField(entry.Name, entry.Text);
						}
					EndDisabled();
				}
			}
		}
	}
}
#endif