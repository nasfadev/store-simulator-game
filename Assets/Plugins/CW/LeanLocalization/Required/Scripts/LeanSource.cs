using UnityEngine;
using System.Collections.Generic;

namespace Lean.Localization
{
	/// <summary>This is the base class used for all translation sources. When a translation source is built, it will populate the <b>LeanLocalization</b> class with its translation data.</summary>
	public abstract class LeanSource : MonoBehaviour
	{
		/// <summary>This allows you to register a token or language.</summary>
		public virtual void Register()
		{
		}

		/// <summary>This allows you to register a phrase based on the specified languages.</summary>
		public virtual void Register(string primaryLanguage, string secondaryLanguage)
		{
		}

		protected virtual void OnEnable()
		{
			LeanLocalization.DelayUpdateTranslations();
		}

		protected virtual void OnDisable()
		{
			LeanLocalization.DelayUpdateTranslations();
		}
	}
}