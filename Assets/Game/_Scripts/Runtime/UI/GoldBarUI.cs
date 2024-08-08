using RubyCase.Common;
using RubyCase.Core.Event;
using RubyCase.Runtime.Managers;
using TMPro;
using UnityEngine;

namespace RubyCase.Runtime.UI
{
	public class GoldBarUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI goldText;

		void OnEnable()
		{
			GameEventManager.Instance.On<GoldChanged>(UpdateGoldText);
		}

		void OnDisable()
		{
			GameEventManager.Instance.Off<GoldChanged>(UpdateGoldText);
		}

		void UpdateGoldText(GoldChanged e)
		{
			goldText.text = GameManager.Instance.CurrentGold.ToString();
		}
	}
}