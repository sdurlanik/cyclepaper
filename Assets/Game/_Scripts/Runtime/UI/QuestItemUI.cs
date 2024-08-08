using System;
using System.Collections;
using System.Collections.Generic;
using RubyCase.Common;
using RubyCase.Core.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RubyCase.Runtime.UI
{
	public class QuestItemUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI questType, remainCount;
		[SerializeField] Image questBg, doneIcon;
		[SerializeField] GameObject glowObject;

		GameEnums.PrismType _prismType;

		void OnEnable()
		{
			GameEventManager.Instance.On<EjectedAdded>(OnEjectedAdded);
		}

		void OnDisable()
		{
			GameEventManager.Instance.Off<EjectedAdded>(OnEjectedAdded);
		}

		void OnEjectedAdded(EjectedAdded e)
		{
			if(e.prismType != _prismType)
				return;

			UpdateRemainCount(e.remainingCapacity);
			if(e.remainingCapacity == 0)
				SetDone();
		}

		public void InitQuestItem(Sprite bgSprite, GameEnums.PrismType type, int remain)
		{
			_prismType = type;
			questBg.sprite = bgSprite;
			questType.text = type.ToString();
			remainCount.text = remain.ToString();
			doneIcon.gameObject.SetActive(false);
			glowObject.SetActive(false);
		}

		void UpdateRemainCount(int remain)
		{
			remainCount.text = remain.ToString();
		}

		void SetDone()
		{
			doneIcon.gameObject.SetActive(true);
			glowObject.SetActive(true);
			remainCount.gameObject.SetActive(false);
			questType.text = "DONE";
		}
	}
}