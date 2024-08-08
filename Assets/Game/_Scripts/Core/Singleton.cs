using UnityEngine;

namespace RubyCase.Core
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				if(_instance == null)
				{
					//Debug.LogError($"Singleton instance of {typeof(T)} is null. Ensure the script is attached to an active GameObject.");
				}

				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if(_instance == null)
			{
				_instance = this as T;
			}
			else if(_instance != this)
			{
				Destroy(gameObject);
				Debug.LogWarning($"Another instance of {typeof(T)} was found and destroyed.");
			}
		}
	}
}