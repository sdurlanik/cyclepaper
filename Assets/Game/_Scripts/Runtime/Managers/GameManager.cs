using System.Linq;
using RubyCase.Common;
using RubyCase.Core;
using RubyCase.Core.Event;
using RubyCase.Runtime.Controllers;
using RubyCase.Runtime.UI;
using RubyCase.Scriptables;
using UnityEngine;

namespace RubyCase.Runtime.Managers
{
	public class GameManager : Singleton<GameManager>
	{
		[SerializeField] QuestController questController;
		[SerializeField] MotorcycleController motorcycleController;
		[SerializeField] HealthController healthController;
		[SerializeField] QuestsUI questsUI;
		[SerializeField] ProductionLineController productionLineController;
		[SerializeField] ProductionLineData productionLineData;

		int _currentGold = 0;
		public int CurrentGold
		{
			get
			{
				return _currentGold;
			}
			set
			{
				_currentGold = value;
				GameEventManager.Instance.Fire(new GoldChanged());
			}
		}

		void OnEnable() => GameEventManager.Instance.On<QuestCompleted>(OnQuestCompleted);
		void OnDisable() => GameEventManager.Instance.Off<QuestCompleted>(OnQuestCompleted);
		void OnQuestCompleted(QuestCompleted e) => CurrentGold += e.questData.productionQuests.Sum(x => x.prismCount) * 10;

		public void StartLevel()
		{
			productionLineController.InitProductLine(productionLineData);
			motorcycleController.InitMotorCycle();
			questController.InitCurrentQuests();
			healthController.InitHealthBar();
			questsUI.InitQuestsUI(questController);

			GameEventManager.Instance.Fire(new LevelStarted());
		}
	}
}