using System;
using System.Collections.Generic;
using RubyCase.Common;
using RubyCase.Core.Event;
using UnityEngine;

namespace RubyCase.Runtime.Controllers
{
	public class EjectedCollector
	{
		public GameEnums.PrismType CollectedPrismType { get; private set; }
		public int Capacity { get; private set; }
		public bool IsFull => _ejectedObjects.Count >= Capacity;

		readonly List<GameObject> _ejectedObjects = new List<GameObject>();

		public EjectedCollector(int capacity, GameEnums.PrismType prismType)
		{
			Capacity = capacity;
			CollectedPrismType = prismType;
		}

		public void AddEjectedObject(GameObject ejectedObject)
		{
			_ejectedObjects.Add(ejectedObject);
			GameEventManager.Instance.Fire(new EjectedAdded() { prismType = CollectedPrismType, remainingCapacity = Capacity - _ejectedObjects.Count });

			if(IsFull)
			{
				GameEventManager.Instance.Fire(new EjectedCollectorFull() { ejectedCollector = this });
			}
		}

		public Vector3 GetAvailablePosition(Vector3 targetPos, Vector3 prismScale)
		{
			var count = _ejectedObjects.Count < 20 ? _ejectedObjects.Count : 20;
			return new Vector3(targetPos.x, targetPos.y + prismScale.y * count, targetPos.z);
		}
	}
}