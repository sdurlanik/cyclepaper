using RubyCase.Runtime.Controllers;

namespace RubyCase.Scriptables
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "ProductionLineData", menuName = "ScriptableObjects/ProductionLineData")]
	public class ProductionLineData : ScriptableObject
	{
		public BoxController boxPrefab;
		public GameObject rejectedBoxPrefab;
		public float spawnInterval = 2f;
		public float moveSpeed = 2f;
		public float speedAddition = 0.1f;

		void OnDestroy()
		{
			moveSpeed = 2f;
		}
	}
}