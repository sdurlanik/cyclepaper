using System;
using RubyCase.Runtime.Managers;
using RubyCase.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RubyCase.Runtime.UI
{
	public class BikeItemUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI bikeNameText, bikePriceText;
		[SerializeField] Image bikeImage;
		[SerializeField] Button selectButton;

		public event Action<MotorcycleData> OnBikeSelected;
		public event Action<MotorcycleData> OnBikeBought;

		MotorcycleData _bikeData;

		public void SetBikeItemUI(MotorcycleData bikeData)
		{
			_bikeData = bikeData;

			bikeNameText.text = bikeData.displayName;
			bikeImage.sprite = bikeData.bikeIcon;

			if(bikeData.isBought && !bikeData.isSelected)
			{
				bikePriceText.text = "Select";
				selectButton.interactable = true;
			}
			else if(bikeData.isSelected)
			{
				selectButton.interactable = false;
				bikePriceText.text = "Current";
			}
			else
			{
				bikePriceText.text = bikeData.price.ToString();
				selectButton.interactable = GameManager.Instance.CurrentGold >= bikeData.price;
			}
		}

		public void OnSelectClicked()
		{
			if(!_bikeData.isBought)
			{
				OnBikeBought?.Invoke(_bikeData);
			}
			else
			{
				OnBikeSelected?.Invoke(_bikeData);
			}
		}
	}
}