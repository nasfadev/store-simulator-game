using System.Collections.Generic;
using UnityEngine;

namespace Lean.Localization
{
	/// <summary>The component works like <b>LeanToken</b>, but must be added to the child GameObject of the one that will be translated.</summary>
	[ExecuteInEditMode]
	[HelpURL(LeanLocalization.HelpUrlPrefix + "LeanLocalToken")]
	[AddComponentMenu(LeanLocalization.ComponentPathPrefix + "Local Token")]
	public class LeanLocalToken : LeanToken
	{
		private static List<LeanLocalToken> tempLocalTokens = new List<LeanLocalToken>();

		public static bool TryGetLocalToken(GameObject root, string name, ref LeanToken result)
		{
			if (root != null)
			{
				root.GetComponentsInChildren(tempLocalTokens);

				foreach (var localToken in tempLocalTokens)
				{
					if (localToken.name == name)
					{
						result = localToken;

						return true;
					}
				}
			}

			return false;
		}

		public override void Register()
		{
			// Don't register local tokens
			//LeanLocalization.RegisterToken(name, this);
		}
	}
}