using System.Collections;
using DG.Tweening;
using RubyCase.Scriptables;
using UnityEngine;

namespace RubyCase.Runtime.UI.Pages
{
	public abstract class PageBase : MonoBehaviour
	{
		[SerializeField] protected CanvasGroup canvasGroup;
		[SerializeField] protected GameObject dimmedBackground;
		[SerializeField] protected GameObject content;
		[SerializeField] protected PageAnimationSettings animationSettings;

		public virtual void OnPageOpened()
		{
			Debug.Log($"{GetType().Name} page opened.");
			gameObject.SetActive(true);

			canvasGroup.DOFade(1f, animationSettings.FadeDuration)
				.SetEase(animationSettings.FadeEase)
				.From(0);

			content.transform.DOScale(Vector3.one, animationSettings.ContentScaleDuration)
				.SetEase(animationSettings.ContentScaleEase)
				.From(Vector3.zero);
		}

		public virtual void OnPageClosed()
		{
			Debug.Log($"{GetType().Name} page closed.");

			canvasGroup.DOFade(0f, animationSettings.FadeDuration)
				.SetEase(animationSettings.FadeEase)
				.OnComplete(() => gameObject.SetActive(false));
		}
	}
}