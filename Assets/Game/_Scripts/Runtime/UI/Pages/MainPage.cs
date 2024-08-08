using RubyCase.Runtime.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace RubyCase.Runtime.UI.Pages
{
	public class MainPage : PageBase
	{
		[SerializeField] Button startButton;

		public void OnStartButton()
		{
			GUIManager.Instance.pageController.ClosePage();
			GameManager.Instance.StartLevel();
			startButton.interactable = false;
		}
	}
}