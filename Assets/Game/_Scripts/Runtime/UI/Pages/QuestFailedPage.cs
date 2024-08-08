using RubyCase.Runtime.Managers;

namespace RubyCase.Runtime.UI.Pages
{
	public class QuestFailedPage : PageBase
	{
		public void OnRetryButton()
		{
			GUIManager.Instance.pageController.ClosePage();
			GameManager.Instance.StartLevel();
		}

		public void OnBikeSelectionButton()
		{
			GUIManager.Instance.pageController.OpenPage<BikeSelectionPage>();

		}
	}
}