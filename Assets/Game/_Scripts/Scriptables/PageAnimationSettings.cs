using DG.Tweening;
using UnityEngine;

namespace RubyCase.Scriptables
{
	[CreateAssetMenu(menuName = "ScriptableObjects/PageAnimationSettings", fileName = "PageAnimationSettings", order = 0)]
	public class PageAnimationSettings : ScriptableObject
	{
		[SerializeField] float fadeDuration = 0.5f;
		[SerializeField] Ease fadeEase = Ease.Linear;

		[SerializeField] float contentScaleDuration = 0.5f;
		[SerializeField] Ease contentScaleEase = Ease.Linear;

		public float FadeDuration => fadeDuration;
		public Ease FadeEase => fadeEase;
		public float ContentScaleDuration => contentScaleDuration;
		public Ease ContentScaleEase => contentScaleEase;
	}
}