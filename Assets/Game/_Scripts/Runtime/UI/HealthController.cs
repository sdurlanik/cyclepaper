using System;
using System.Collections.Generic;
using Lofelt.NiceVibrations;
using RubyCase.Common;
using RubyCase.Core;
using RubyCase.Core.Event;
using RubyCase.Runtime.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace RubyCase.Runtime.UI
{
	public class HealthController : MonoBehaviour
	{
		[SerializeField] Image exampleHeart;
		[SerializeField] int maxHealth = 3;

		List<Image> _hearts = new List<Image>();
		int _currentHealth;

		void OnEnable()
		{
			GameEventManager.Instance.On<ObstacleHit>(OnHitObstacle);
		}

		void OnDisable()
		{
			GameEventManager.Instance.Off<ObstacleHit>(OnHitObstacle);
		}


		public void InitHealthBar()
		{
			_currentHealth = maxHealth;
			exampleHeart.transform.parent.ClearActiveChildren();
			_hearts.Clear();

			for(int i = 0; i < maxHealth; i++)
			{
				var heart = Instantiate(exampleHeart, exampleHeart.transform.parent);
				heart.gameObject.SetActive(true);
				_hearts.Add(heart);
			}
		}

		void OnHitObstacle(ObstacleHit e)
		{
			_currentHealth--;

			UpdateHealthBar();

			if(_currentHealth <= 0)
			{
				HandleGameOver();
			}
		}

		void UpdateHealthBar()
		{
			for(int i = 0; i < _hearts.Count; i++)
			{
				_hearts[i].gameObject.SetActive(i < _currentHealth);
			}
		}

		void HandleGameOver()
		{
			GameEventManager.Instance.Fire(new GameOver());
			Debug.Log("Game Over");
			HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);

		}
	}
}