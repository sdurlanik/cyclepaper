using System.Collections.Generic;
using RubyCase.Runtime;
using UnityEngine;

namespace RubyCase.Scriptables
{
	[CreateAssetMenu(menuName = "ScriptableObjects/QuestData", fileName = "QuestData", order = 0)]
	public class QuestData : ScriptableObject
	{
		public int questIndex;
		public List<ProductionQuest> productionQuests;
	}
}