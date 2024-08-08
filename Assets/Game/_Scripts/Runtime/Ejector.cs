using System;
using RubyCase.Common;
using RubyCase.Runtime.Managers;
using UnityEngine;

namespace RubyCase.Runtime
{
	public class Ejector : MonoBehaviour
	{
		Vector3 _startPosition;
		Vector3 _targetPosition;
		float _speed;
		float _timeElapsed;


		public void Initialize(Vector3 targetPos, float speed)
		{
			GetComponent<Collider>().enabled = false;

			_startPosition = transform.position;
			_targetPosition = targetPos;
			_speed = speed;
			_timeElapsed = 0f;
		}

		void Update()
		{
			_timeElapsed += Time.deltaTime;
			var newPosition = CalculatePosition(_timeElapsed);
			transform.position = newPosition;

			if(Vector3.Distance(transform.position, _targetPosition) < 0.01f)
			{
				EffectManager.Instance.PlayEffect(GameEnums.EffectType.PrismSmoke, transform.position);
				transform.position = _targetPosition;
				enabled = false;
			}
		}

		Vector3 CalculatePosition(float time) // path calculation with bezier curve
		{
			Vector3 p0 = _startPosition;
			Vector3 p2 = _targetPosition;

			Vector3 midpoint = (p0 + p2) / 2;

			float peakHeight = 0.5f;
			Vector3 p1 = new Vector3(midpoint.x, midpoint.y + peakHeight, midpoint.z);

			float totalDistance = (_targetPosition - _startPosition).magnitude;
			float travelDuration = totalDistance / _speed;

			float t = time / travelDuration;

			t = Mathf.Clamp01(t);

			Vector3 position = Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;

			return position;
		}


		void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			float simulationTime = 0f;
			Vector3 previousPosition = _startPosition;

			while(simulationTime < 5f)
			{
				simulationTime += Time.fixedDeltaTime;
				Vector3 newPosition = CalculatePosition(simulationTime);
				Gizmos.DrawLine(previousPosition, newPosition);
				previousPosition = newPosition;
			}
		}
	}
}