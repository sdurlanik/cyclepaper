using UnityEngine;

namespace RubyCase.Scriptables
{
	[CreateAssetMenu(fileName = "BoxData", menuName = "ScriptableObjects/BoxData", order = 1)]
	public class BoxData : ScriptableObject
	{
		[SerializeField] Vector3 size = new Vector3(1, 0.05f, 1);
		[SerializeField] int prismCount = 20;

		public Vector3 Size => size;
		public int PrismCount => prismCount;
	}
}