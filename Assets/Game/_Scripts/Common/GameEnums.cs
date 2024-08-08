using UnityEngine;

namespace RubyCase.Common
{
	public class GameEnums : MonoBehaviour
	{
		public enum PrismType
		{
			Yellow = 0,
			Blue = 1,
		}

		public enum EffectType
		{
			WheelSmoke = 0,
			Fireworks = 1,
			PrismSmoke = 2,
			ObstacleHit = 3,
		}

		public enum HapticType
		{
			VeryLightImpact,
			LightImpact,
			MediumImpact,
			HeavyImpact,
		}
	}
}