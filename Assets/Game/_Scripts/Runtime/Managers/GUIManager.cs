using System.Linq;
using RubyCase.Common;
using RubyCase.Core;
using RubyCase.Core.Event;
using RubyCase.Runtime.UI;
using RubyCase.Runtime.UI.Pages;

namespace RubyCase.Runtime.Managers
{
	public class GUIManager : Singleton<GUIManager>
	{
		public PageController pageController;

		void OnEnable()
		{
			GameEventManager.Instance.On<GameOver>(OnGameOver);
			GameEventManager.Instance.On<QuestCompleted>(OnQuestCompleted);
		}

		void OnDisable()
		{
			GameEventManager.Instance.Off<GameOver>(OnGameOver);
			GameEventManager.Instance.Off<QuestCompleted>(OnQuestCompleted);
		}

		void OnGameOver(GameOver e) => pageController.OpenPage<QuestFailedPage>();

		void OnQuestCompleted(QuestCompleted e)
		{
			var questCompletedPage = pageController.OpenPage<QuestCompletedPage>();
			((QuestCompletedPage) questCompletedPage).SetCoinText(e.questData.productionQuests.Sum(x => x.prismCount) * 10);
		}

		void Start()
		{
			pageController.OpenPage<MainPage>();
		}
	}
}