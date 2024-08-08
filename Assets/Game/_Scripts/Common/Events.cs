using RubyCase.Core.Event;
using RubyCase.Runtime.Controllers;
using RubyCase.Scriptables;

namespace RubyCase.Common
{
	public class LevelStarted : GameEvent
	{
	}

	public class GameOver : GameEvent
	{
	}

	public class GoldChanged : GameEvent
	{
	}

	public class QuestCompleted : GameEvent
	{
		public QuestData questData;
	}

	public class EjectedCollectorFull : GameEvent
	{
		public EjectedCollector ejectedCollector;
	}

	public class ObstacleHit : GameEvent
	{
	}

	public class EjectedAdded : GameEvent
	{
		public GameEnums.PrismType prismType;
		public int remainingCapacity;
	}

	public class ScreenTouchDown : GameEvent
	{
	}

	public class ScreenTouchUp : GameEvent
	{
	}

	public class SelectBike : GameEvent
	{
		public MotorcycleData bikeData;
	}
}