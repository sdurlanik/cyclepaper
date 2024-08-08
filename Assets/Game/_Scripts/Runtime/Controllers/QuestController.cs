using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lofelt.NiceVibrations;
using RubyCase.Common;
using RubyCase.Core.Event;
using RubyCase.Scriptables;
using UnityEngine;

namespace RubyCase.Runtime.Controllers
{
	public class QuestController : MonoBehaviour
	{
		int _currentQuestIndex = 0;
		public List<QuestData> allQuests;
		readonly List<ProductionQuest> _currentQuests = new List<ProductionQuest>();
		public List<ProductionQuest> CurrentQuests => _currentQuests;

		void OnEnable()
		{
			GameEventManager.Instance.On<EjectedCollectorFull>(OnEjectedCollectorFull);
		}

		void OnDisable()
		{
			GameEventManager.Instance.Off<EjectedCollectorFull>(OnEjectedCollectorFull);
		}

		public void InitCurrentQuests()
		{
			_currentQuests.Clear();
			var currentQuest = allQuests.Find(item => item.questIndex == _currentQuestIndex);
			_currentQuests.AddRange(currentQuest.productionQuests);
		}

		void OnEjectedCollectorFull(EjectedCollectorFull e)
		{
			_currentQuests.Remove(_currentQuests.Find(x => x.prismType == e.ejectedCollector.CollectedPrismType));

			if(_currentQuests.Count == 0)
			{
				var questData = allQuests.Find(x => x.questIndex == _currentQuestIndex);
				GameEventManager.Instance.Fire(new QuestCompleted { questData = questData });
				_currentQuestIndex++;

				if(_currentQuestIndex == allQuests.Count - 1)
				{
					_currentQuestIndex = 0;
				}
			}
		}

		public int GetPrismCountByType(GameEnums.PrismType prismType)
		{
			var data = allQuests.Find(x => x.questIndex == _currentQuestIndex);
			return data.productionQuests.Find(x => x.prismType == prismType).prismCount;
		}
	}
}