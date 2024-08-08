using System;
using System.Collections.Generic;
using System.Linq;
using RubyCase.Common;
using RubyCase.Core;
using RubyCase.Core.Event;
using RubyCase.Runtime.Managers;
using RubyCase.Scriptables;
using UnityEngine;

namespace RubyCase.Runtime.Controllers
{
	[Serializable]
	public class PrismMaterials
	{
		public Material material;
		public GameEnums.PrismType prismType;
	}

	public class BoxController : MonoBehaviour
	{
		public GameEnums.PrismType prismType;

		[SerializeField] GameObject prismPrefab;
		[SerializeField] List<PrismMaterials> prismMaterials;
		GameObject[] _prisms;
		Transform _targetTransform;


		void OnEnable()
		{
			GameEventManager.Instance.On<QuestCompleted>(OnQuestCompleted);
		}

		void OnDisable()
		{
			GameEventManager.Instance.Off<QuestCompleted>(OnQuestCompleted);
		}

		void OnQuestCompleted(QuestCompleted e)
		{
			//destroy all prisms
			foreach(var prism in _prisms)
			{
				Destroy(prism);
			}
		}

		public void InitializePrisms(BoxData boxData, Transform targetTransform, GameEnums.PrismType pType)
		{
			prismType = pType;
			_targetTransform = targetTransform;
			_prisms = new GameObject[boxData.PrismCount];

			for(int i = 0; i < boxData.PrismCount; i++)
			{
				var prism = Instantiate(prismPrefab, transform.position, Quaternion.identity, transform);
				prism.transform.localPosition = new Vector3(0, i * boxData.Size.y, 0);
				prism.transform.localScale = boxData.Size;
				ChangePrismMaterial(prism, pType);
				_prisms[i] = prism;
			}
		}

		public void EjectPrism(EjectedCollector ejectedCollector, float speed)
		{
			var prism = GetPrismWithIndex();
			prism.transform.SetParent(_targetTransform);

			var ejector = prism.GetOrAddComponent<Ejector>();

			ejectedCollector.AddEjectedObject(prism);
			var targetPos = ejectedCollector.GetAvailablePosition(_targetTransform.position, prism.transform.localScale);
			ejector.Initialize(targetPos, speed);

			if(!HasPrismLeft())
			{
				Destroy(gameObject);
			}

			EffectManager.Instance.PlayEffect(GameEnums.EffectType.WheelSmoke, prism.transform.position);
		}

		GameObject GetPrismWithIndex()
		{
			var prism = _prisms.Last(item => item != null);
			var index = Array.IndexOf(_prisms, prism);
			_prisms[index] = null;

			return prism;
		}

		void ChangePrismMaterial(GameObject prism, GameEnums.PrismType prismType)
		{
			var prismRenderer = prism.GetComponent<Renderer>();
			prismRenderer.material = prismMaterials.Find(item => item.prismType == prismType).material;
		}

		bool HasPrismLeft() => _prisms is { Length: > 0 } && _prisms.Any(item => item != null);
	}
}