
using System.Collections.Generic;
using UnityEngine;
namespace ThirtySec
{

	public class FileDelegate : ThirtySec.Scripton<FileDelegate>
	{
        
		public  event System.Action<bool> onPause = delegate { };
		public  event System.Action onQuit = delegate { };

		public  List<string> openedFiles = new List<string>();

		void OnApplicationQuit()
		{
			onQuit.Invoke();
		}
		void OnApplicationPause(bool status)
		{
			onPause.Invoke(status);

		}
	


	}
}