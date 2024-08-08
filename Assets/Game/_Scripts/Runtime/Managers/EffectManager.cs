using System;
using System.Collections.Generic;
using RubyCase.Common;
using RubyCase.Core;
using UnityEngine;

namespace RubyCase.Runtime.Managers
{
	[Serializable]
	public class Effect
	{
		public GameEnums.EffectType effectType;
		public GameObject effectPrefab;
	}

	public class EffectManager : Singleton<EffectManager>
	{
		[SerializeField] List<Effect> effects;

		public GameObject PlayEffect(GameEnums.EffectType effectType, Vector3 position)
		{
			var effect = effects.Find(e => e.effectType == effectType);
			if(effect != null)
			{
				return Instantiate(effect.effectPrefab, position, effect.effectPrefab.transform.rotation);
			}

			return null;
		}
	}
}