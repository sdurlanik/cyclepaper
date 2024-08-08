using System;
using UnityEngine;

namespace RubyCase.Scriptables
{
	[CreateAssetMenu(fileName = "MotorcycleData", menuName = "ScriptableObjects/MotorcycleData", order = 2)]
	public class MotorcycleData : ScriptableObject
	{
		public string displayName;
		public int price;
		public Sprite bikeIcon;
		public float wheelSpeed = 10f;
		public float wheelRadius = 0.5f;
		public float tiltSpeed = 100f;
		public float wheelRayCheckInterval = 0.05f;
		public Vector3 initialRotation;
		public Vector3 minRotation;
		public bool isBought;
		public bool isSelected;
	}
}