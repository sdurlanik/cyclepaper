using System;
using System.Collections.Generic;
using RubyCase.Common;
using RubyCase.Core;
using RubyCase.Core.Event;
using RubyCase.Scriptables;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RubyCase.Runtime.Controllers
{
	public class ProductionLineController : MonoBehaviour
	{
		[SerializeField] Transform spawnPoint;
		[SerializeField] Transform endPoint;
		[SerializeField] Transform yellowBoxTargetPoint;
		[SerializeField] Transform blueBoxTargetPoint;
		[SerializeField] Renderer bandRenderer;
		[SerializeField] List<BoxData> allBoxData;

		readonly List<GameObject> _activeBoxes = new List<GameObject>();
		BoxData _currentBoxData;
		ProductionLineData _productionLineData;
		float _spawnTimer = 0;
		bool _isLevelCompleted = true;
		int _currentBoxIndex = 0;
		int _fullEjectedCollectorPrismType = -1;

		void OnEnable()
		{
			GameEventManager.Instance.On<QuestCompleted>(OnQuestCompleted);
			GameEventManager.Instance.On<GameOver>(OnGameOver);
			GameEventManager.Instance.On<EjectedCollectorFull>(OnEjectedCollectorFull);

		}

		void OnDisable()
		{
			GameEventManager.Instance.Off<QuestCompleted>(OnQuestCompleted);
			GameEventManager.Instance.Off<GameOver>(OnGameOver);
			GameEventManager.Instance.Off<EjectedCollectorFull>(OnEjectedCollectorFull);
		}

		void OnGameOver(GameOver e) => ResetProductionLine();

		void OnQuestCompleted(QuestCompleted e)
		{
			ResetProductionLine();
			_fullEjectedCollectorPrismType = -1;
			_productionLineData.moveSpeed += _productionLineData.speedAddition;
			_currentBoxIndex++;

			if(_currentBoxIndex == allBoxData.Count - 1)
			{
				_currentBoxIndex = 0;
			}
		}

		void OnEjectedCollectorFull(EjectedCollectorFull e)
		{
			_fullEjectedCollectorPrismType = (int) e.ejectedCollector.CollectedPrismType;
		}

		void Update()
		{
			if(_isLevelCompleted)
				return;

			HandleSpawn();
			HandleBoxMovement();
			MoveBand();
		}

		public void InitProductLine(ProductionLineData productionLineData)
		{
			_productionLineData = productionLineData;
			_currentBoxData = GetCurrentBoxData();
			_isLevelCompleted = false;
		}

		void HandleSpawn()
		{
			_spawnTimer -= Time.deltaTime;
			if(_spawnTimer <= 0f)
			{
				SpawnBox();
				_spawnTimer = _productionLineData.spawnInterval;
			}

		}

		void SpawnBox()
		{
			var obstacleChance = Random.Range(0f, 100f);
			if(obstacleChance <= 30)
			{
				var box = Instantiate(_productionLineData.rejectedBoxPrefab, spawnPoint.position, Quaternion.identity);
				_activeBoxes.Add(box);
			}
			else
			{
				var prismTypeIndex = Random.Range(0, 2);
				if(_fullEjectedCollectorPrismType != -1)
					prismTypeIndex = _fullEjectedCollectorPrismType == 0 ? 1 : 0;

				var randomPrismType = (GameEnums.PrismType) prismTypeIndex;
				var boxController = Instantiate(_productionLineData.boxPrefab, spawnPoint.position, Quaternion.identity);
				boxController.InitializePrisms(_currentBoxData, GetTargetPoint(randomPrismType), randomPrismType);
				_activeBoxes.Add(boxController.gameObject);
			}
		}

		void HandleBoxMovement()
		{
			for(int i = _activeBoxes.Count - 1; i >= 0; i--)
			{
				var box = _activeBoxes[i];
				if(box == null)
				{
					_activeBoxes.RemoveAt(i);
					continue;
				}

				MoveBox(box.gameObject);

				if(Vector3.Distance(box.transform.position, endPoint.position) <= 0.1f)
				{
					Destroy(box.gameObject);
					_activeBoxes.RemoveAt(i);
				}
			}
		}

		void MoveBand()
		{
			bandRenderer.material.mainTextureOffset += new Vector2(0, -_productionLineData.moveSpeed / 3 * Time.deltaTime);
		}

		void MoveBox(GameObject box)
		{
			box.transform.position = Vector3.MoveTowards(box.transform.position, endPoint.position, _productionLineData.moveSpeed * Time.deltaTime);
		}

		Transform GetTargetPoint(GameEnums.PrismType prismType)
		{
			return prismType == GameEnums.PrismType.Yellow ? yellowBoxTargetPoint : blueBoxTargetPoint;
		}


		void ResetProductionLine()
		{
			_isLevelCompleted = true;
			_spawnTimer = 0;

			_activeBoxes.ForEach(Destroy);
			_activeBoxes.Clear();

			blueBoxTargetPoint.ClearChildren();
			yellowBoxTargetPoint.ClearChildren();
		}

		BoxData GetCurrentBoxData()
		{
			return allBoxData[_currentBoxIndex];
		}
	}
}