using System;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using RubyCase.Common;
using RubyCase.Core.Event;
using RubyCase.Runtime.Managers;
using RubyCase.Scriptables;
using UnityEngine;

namespace RubyCase.Runtime.Controllers
{
	public class MotorcycleController : MonoBehaviour
	{
		[SerializeField] QuestController questController;
		[SerializeField] Transform wheelTransform;
		[SerializeField] MotorcycleData selectedMotorcycleData;

		readonly List<EjectedCollector> _ejectedCollectors = new List<EjectedCollector>();
		bool _isWheelActive;
		bool _isTilting;
		float _timer;

		void OnEnable()
		{
			GameEventManager.Instance.On<ScreenTouchDown>(OnScreenTouchDown);
			GameEventManager.Instance.On<ScreenTouchUp>(OnScreenTouchUp);
			GameEventManager.Instance.On<SelectBike>(OnSelectBike);
			GameEventManager.Instance.On<QuestCompleted>(OnQuestCompleted);
			GameEventManager.Instance.On<GameOver>(OnGameOver);
		}

		void OnDisable()
		{
			GameEventManager.Instance.Off<ScreenTouchDown>(OnScreenTouchDown);
			GameEventManager.Instance.Off<ScreenTouchUp>(OnScreenTouchUp);
			GameEventManager.Instance.Off<SelectBike>(OnSelectBike);
			GameEventManager.Instance.Off<QuestCompleted>(OnQuestCompleted);
			GameEventManager.Instance.Off<GameOver>(OnGameOver);
		}

		void OnGameOver(GameOver e) => ToggleTilting(false);
		void OnQuestCompleted(QuestCompleted e) => ToggleTilting(false);
		void OnSelectBike(SelectBike e) => SetBikeData(e.bikeData);
		void OnScreenTouchUp(ScreenTouchUp e) => ToggleTilting(false);
		void OnScreenTouchDown(ScreenTouchDown e) => ToggleTilting(true);

		public void InitMotorCycle()
		{
			InitializeEjectedCollectors();
			_timer = selectedMotorcycleData.wheelRayCheckInterval;
		}

		public void SetBikeData(MotorcycleData data)
		{
			selectedMotorcycleData = data;
		}

		void InitializeEjectedCollectors()
		{
			_ejectedCollectors.Clear();

			Array prismTypes = Enum.GetValues(typeof(GameEnums.PrismType));
			int questCount = questController.allQuests[0].productionQuests.Count;

			for(int i = 0; i < questCount && i < prismTypes.Length; i++)
			{
				var prismType = (GameEnums.PrismType) prismTypes.GetValue(i);
				Debug.Log($"Processing PrismType: {prismType}"); // Debug log to verify the correct prism type

				var prismCount = questController.GetPrismCountByType(prismType);
				Debug.Log($"PrismCount for {prismType}: {prismCount}"); // Debug log to check the count

				var ejectedCollector = new EjectedCollector(prismCount, prismType);
				_ejectedCollectors.Add(ejectedCollector);
			}
		}

		void Update()
		{

			if(_isWheelActive)
			{
				RotateWheel();
				CheckCollision();
			}

			if(_isTilting)
				TiltDown();
			else if(!_isWheelActive)
				RestoreTilt();
		}

		public void ToggleTilting(bool activate)
		{
			_isTilting = activate;
			_isWheelActive = activate;
		}

		void RotateWheel()
		{
			if(wheelTransform != null)
			{
				wheelTransform.localRotation *= Quaternion.Euler(selectedMotorcycleData.wheelSpeed * Time.deltaTime, 0, 0);
			}
		}

		void TiltDown()
		{
			var normalizedX = (transform.localRotation.eulerAngles.x > 180)
				? transform.localRotation.eulerAngles.x - 360
				: transform.localRotation.eulerAngles.x;

			if(normalizedX <= selectedMotorcycleData.minRotation.x)
				transform.localRotation = Quaternion.Euler(selectedMotorcycleData.minRotation);
			else
				transform.localRotation *= Quaternion.Euler(-selectedMotorcycleData.tiltSpeed * Time.deltaTime, 0, 0);
		}

		void RestoreTilt()
		{
			transform.localRotation = Quaternion.RotateTowards(
				transform.localRotation,
				Quaternion.Euler(selectedMotorcycleData.initialRotation),
				selectedMotorcycleData.tiltSpeed * Time.deltaTime
			);
		}

		void CheckCollision()
		{
			if(Physics.Raycast(wheelTransform.position, -transform.up, out var hit, selectedMotorcycleData.wheelRadius))
			{
				if(hit.collider != null && hit.collider.CompareTag("Prism"))
				{
					var boxController = hit.collider.GetComponentInParent<BoxController>();
					if(CanEject(boxController.prismType))
						TryEjectPrism(boxController);
					else
					{
						ToggleTilting(false);
						HapticManager.Instance?.Play(GameEnums.HapticType.LightImpact);
					}

					_isTilting = false;
				}
			}
			else if(_isWheelActive) // If the wheel is active and there is no collision
			{
				_isTilting = true;
				_timer = selectedMotorcycleData.wheelRayCheckInterval;
			}
		}

		void TryEjectPrism(BoxController boxController)
		{

			_timer += Time.deltaTime;

			if(_timer >= selectedMotorcycleData.wheelRayCheckInterval)
			{
				float speed = selectedMotorcycleData.wheelSpeed * 0.01f; // todo move the hardcoded value to the scriptable object
				boxController.EjectPrism(GetEjectedCollectorByType(boxController.prismType), speed);
				HapticManager.Instance?.Play(GameEnums.HapticType.MediumImpact);
				_timer = 0f;
			}
		}

		EjectedCollector GetEjectedCollectorByType(GameEnums.PrismType prismType)
		{
			return _ejectedCollectors.Find(collector => collector.CollectedPrismType == prismType);
		}

		bool CanEject(GameEnums.PrismType prismType)
		{
			var collector = _ejectedCollectors.Find(collector => collector.CollectedPrismType == prismType);
			return !collector.IsFull;
		}

		void HandleObstacleHit()
		{
			ToggleTilting(false);
			GameEventManager.Instance.Fire(new ObstacleHit());

			EffectManager.Instance.PlayEffect(GameEnums.EffectType.ObstacleHit, wheelTransform.transform.position - transform.up * selectedMotorcycleData.wheelRadius);

			Debug.Log("Hit an obstacle!");
		}

		void OnTriggerEnter(Collider other)
		{
			if(other.CompareTag("Obstacle"))
			{
				HandleObstacleHit();
			}
		}
	}
}