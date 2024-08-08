using RubyCase.Runtime.Managers;
using TMPro;
using UnityEngine;

namespace RubyCase.Runtime.UI.Pages
{
	public class QuestCompletedPage : PageBase
	{
		[SerializeField] TextMeshProUGUI coinText;

		public void SetCoinText(int coin)
		{
			coinText.text = coin.ToString();
		}

		public void OnContinueButton()
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