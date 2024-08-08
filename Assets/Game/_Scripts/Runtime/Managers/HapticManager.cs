using Lofelt.NiceVibrations;
using RubyCase.Common;
using RubyCase.Core;
using RubyCase.Core.Event;

namespace RubyCase.Runtime.Managers
{
	public class HapticManager : Singleton<HapticManager>
	{
		void OnEnable()
		{
			GameEventManager.Instance.On<SelectBike>(OnSelectBike);
			GameEventManager.Instance.On<LevelStarted>(OnLevelStarted);
			GameEventManager.Instance.On<QuestCompleted>(OnQuestCompleted);
			GameEventManager.Instance.On<EjectedCollectorFull>(OnEjectedCollectorFull);
			GameEventManager.Instance.On<ObstacleHit>(OnObstacleHit);

		}
		void OnDisable()
		{
			GameEventManager.Instance.Off<SelectBike>(OnSelectBike);
			GameEventManager.Instance.Off<LevelStarted>(OnLevelStarted);
			GameEventManager.Instance.Off<QuestCompleted>(OnQuestCompleted);
			GameEventManager.Instance.Off<EjectedCollectorFull>(OnEjectedCollectorFull);
			GameEventManager.Instance.Off<ObstacleHit>(OnObstacleHit);
		}

		void OnSelectBike(SelectBike e) => Play(GameEnums.HapticType.MediumImpact);
		void OnLevelStarted(LevelStarted e) => Play(GameEnums.HapticType.MediumImpact);
		void OnQuestCompleted(QuestCompleted e) => Play(GameEnums.HapticType.LightImpact);
		void OnEjectedCollectorFull(EjectedCollectorFull e) => Play(GameEnums.HapticType.LightImpact);
		void OnObstacleHit(ObstacleHit e) => Play(GameEnums.HapticType.LightImpact);


		public void Play(GameEnums.HapticType hapticType)
		{
			switch (hapticType)
			{
				case GameEnums.HapticType.VeryLightImpact:
					HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
					break;
				case GameEnums.HapticType.LightImpact:
					HapticPatterns.PlayPreset(HapticPatterns.PresetType.Warning);
					break;
				case GameEnums.HapticType.MediumImpact:
					HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
					break;
				case GameEnums.HapticType.HeavyImpact:
					HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
					break;
			}
		}
	}
}