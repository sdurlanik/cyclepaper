using System.Collections.Generic;
using RubyCase.Common;
using RubyCase.Core;
using RubyCase.Core.Event;
using RubyCase.Runtime.Managers;
using RubyCase.Scriptables;
using UnityEngine;

namespace RubyCase.Runtime.UI.Pages
{
	public class BikeSelectionPage : PageBase
	{
		[SerializeField] List<MotorcycleData> allBikeData;
		[SerializeField] BikeItemUI exampleBikeItemUI;

		public override void OnPageOpened()
		{
			base.OnPageOpened();
			InitBikeItemUI();
		}

		void InitBikeItemUI()
		{
			exampleBikeItemUI.transform.parent.ClearActiveChildren();

			foreach(var bikeData in allBikeData)
			{
				var bikeItemUI = Instantiate(exampleBikeItemUI, exampleBikeItemUI.transform.parent);
				bikeItemUI.gameObject.SetActive(true);
				bikeItemUI.SetBikeItemUI(bikeData);
				bikeItemUI.OnBikeSelected += SelectBike;
				bikeItemUI.OnBikeBought += BuyBike;
			}
		}

		void SelectBike(MotorcycleData bikeData)
		{
			foreach(var data in allBikeData)
			{
				data.isSelected = false;
			}

			bikeData.isSelected = true;
			InitBikeItemUI();

			GameEventManager.Instance.Fire(new SelectBike() { bikeData = bikeData });
		}

		void BuyBike(MotorcycleData bikeData)
		{
			if(GameManager.Instance.CurrentGold >= bikeData.price)
			{
				GameManager.Instance.CurrentGold -= bikeData.price;
				bikeData.isBought = true;
				SelectBike(bikeData);

				InitBikeItemUI();
			}
		}

		public void OnContinueButton()
		{
			GUIManager.Instance.pageController.ClosePage();
			GameManager.Instance.StartLevel();
		}

		void OnDestroy()
		{
			foreach(var bikeData in allBikeData)
			{
				bikeData.isSelected = false;
				bikeData.isBought = false;
			}

			allBikeData[0].isSelected = true;
			allBikeData[0].isBought = true;
		}
	}
}