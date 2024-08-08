using System.Collections;
using System.Collections.Generic;
using RubyCase.Common;
using RubyCase.Core.Event;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RubyCase.Runtime.Managers
{
	public class InputManager : MonoBehaviour
	{
		void Update()
		{
			// Check for touch input on mobile devices
			if(Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);

				if(EventSystem.current.IsPointerOverGameObject(touch.fingerId))
					return;

				if(touch.phase == TouchPhase.Began)
				{
					GameEventManager.Instance.Fire(new ScreenTouchDown());
				}
				else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					GameEventManager.Instance.Fire(new ScreenTouchUp());
				}
			}

			// Check for mouse input (for testing in the editor or desktop platforms)
			else
			{
				if(EventSystem.current.IsPointerOverGameObject())
					return;

				if(Input.GetMouseButtonDown(0))
				{
					GameEventManager.Instance.Fire(new ScreenTouchDown());

				}
				else if(Input.GetMouseButtonUp(0))
				{
					GameEventManager.Instance.Fire(new ScreenTouchUp());
				}
			}
		}
	}
}