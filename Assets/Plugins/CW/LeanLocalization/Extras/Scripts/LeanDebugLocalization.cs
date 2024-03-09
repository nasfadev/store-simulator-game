using UnityEngine;
using UnityEngine.Events;

namespace Lean.Localization
{
	[DefaultExecutionOrder(-100)]
	[HelpURL(LeanLocalization.HelpUrlPrefix + "LeanLocalization")]
	[AddComponentMenu("")]
	public class LeanDebugLocalization : MonoBehaviour
	{
		[System.Serializable] public class StringEvent : UnityEvent<string> {}

		public StringEvent OnString { get { if (onString == null) onString = new StringEvent(); return onString; } } [SerializeField] private StringEvent onString;

		public void ClearSave()
		{
			PlayerPrefs.DeleteKey("LeanLocalization.CurrentLanguage");
		}

		public void ClearSaveAlt()
		{
			PlayerPrefs.DeleteKey("LeanLocalization.CurrentLanguageAlt");
		}

		protected virtual void OnEnable()
		{
			LeanLocalization.OnLocalizationChanged += HandleLocalizationChanged;
		}

		protected virtual void OnDisable()
		{
			LeanLocalization.OnLocalizationChanged -= HandleLocalizationChanged;
		}

		private void HandleLocalizationChanged()
		{
			var text = "";

			if (LeanLocalization.Instances.Count > 0)
			{
				var first = LeanLocalization.Instances[0];

				text += "LOOKING FOR: ";

				if (first.DetectLanguage == LeanLocalization.DetectType.SystemLanguage)
				{
					text += Application.systemLanguage.ToString();
				}
				else if (first.DetectLanguage == LeanLocalization.DetectType.CurrentCulture)
				{
					var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;

					if (cultureInfo != null)
					{
						text += cultureInfo.Name;
					}
				}
				else if (first.DetectLanguage == LeanLocalization.DetectType.CurrentCulture)
				{
					var cultureInfo = System.Globalization.CultureInfo.CurrentUICulture;

					if (cultureInfo != null)
					{
						text += cultureInfo.Name;
					}
				}

				text += "\n\n";

				var load = "";

				if (first.SaveLoad == LeanLocalization.SaveLoadType.WhenChanged)
				{
					load = PlayerPrefs.GetString("LeanLocalization.CurrentLanguage");
				}
				else if (first.SaveLoad == LeanLocalization.SaveLoadType.WhenChanged)
				{
					load = PlayerPrefs.GetString("LeanLocalization.CurrentLanguageAlt");
				}

				if (string.IsNullOrEmpty(load) == false)
				{
					text += "LOADING PREVIOUSLY SAVED: " + load;
				}

				text += "\n\nALIASES:\n";

				foreach (var alias in LeanLocalization.CurrentAliases)
				{
					text += alias.Key + " = " + alias.Value + "\n";
				}

				text += "\n\nDETECTED: " + first.CurrentLanguage;
			}

			if (onString != null)
			{
				onString.Invoke(text);
			}
		}
	}
}