using System;
using System.Collections;
using System.Collections.Generic;
using RubyCase.Common;
using RubyCase.Core;
using RubyCase.Core.Event;
using RubyCase.Runtime.Controllers;
using UnityEngine;

namespace RubyCase.Runtime.UI
{
	public class QuestsUI : MonoBehaviour
	{
		[SerializeField] QuestItemUI exampleQuestItemUI;
		[SerializeField] Sprite yellowBg, blueBg;
		[SerializeField] GameObject questTitleObject;

		List<QuestItemUI> _questItems = new List<QuestItemUI>();

		void OnEnable()
		{
			GameEventManager.Instance.On<QuestCompleted>(OnQuestCompleted);
			GameEventManager.Instance.On<GameOver>(OnGameOver);
		}


		void OnDisable()
		{
			GameEventManager.Instance.Off<GameOver>(OnGameOver);
			GameEventManager.Instance.Off<QuestCompleted>(OnQuestCompleted);
		}

		void OnQuestCompleted(QuestCompleted e)
		{
			ResetQuestUI();
		}

		void OnGameOver(GameOver e)
		{
			ResetQuestUI();
		}

		public void InitQuestsUI(QuestController questController)
		{

			var currentQuests = questController.CurrentQuests;
			foreach(var productionQuest in currentQuests)
			{
				var bg = productionQuest.prismType == GameEnums.PrismType.Yellow ? yellowBg : blueBg;
				var questItem = Instantiate(exampleQuestItemUI, exampleQuestItemUI.transform.parent);

				questItem.gameObject.SetActive(true);
				questItem.InitQuestItem(bg, productionQuest.prismType, productionQuest.prismCount);
				_questItems.Add(questItem);
			}

			questTitleObject.SetActive(true);
		}

		void ResetQuestUI()
		{
			exampleQuestItemUI.transform.parent.ClearActiveChildren();
			_questItems.Clear();
		}
	}
}