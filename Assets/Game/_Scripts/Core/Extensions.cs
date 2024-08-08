using UnityEngine;

namespace RubyCase.Core
{
	public static class GameObjectExtensions
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T component = gameObject.GetComponent<T>();
			if(component == null)
			{
				component = gameObject.AddComponent<T>();
			}

			return component;
		}
	}

	public static class TransformExtensions
	{
		public static void ClearChildren(this Transform transform)
		{
			foreach(Transform child in transform)
			{
				Object.Destroy(child.gameObject);
			}
		}

		public static void ClearActiveChildren(this Transform transform)
		{
			foreach(Transform child in transform)
			{
				if(child.gameObject.activeSelf)
					Object.Destroy(child.gameObject);
			}
		}
	}

	//generic singleton class
}