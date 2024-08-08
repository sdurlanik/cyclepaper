using System;
using System.Collections.Generic;
using System.Linq;
using RubyCase.Runtime.UI.Pages;
using UnityEngine;

namespace RubyCase.Runtime.UI
{
	public class PageController : MonoBehaviour
	{
		Dictionary<string, PageBase> pages = new Dictionary<string, PageBase>();

		PageBase currentPage;

		void Start()
		{
			foreach(var page in GetComponentsInChildren<PageBase>(true))
			{
				pages.Add(page.GetType().Name, page);
				page.gameObject.SetActive(false);
			}
		}

		public PageBase OpenPage<T>() where T : PageBase
		{
			string pageName = typeof(T).Name;

			if(pages.TryGetValue(pageName, out PageBase page))
			{
				currentPage?.OnPageClosed();
				currentPage = page;
				currentPage.OnPageOpened();
			}
			else
			{
				Debug.LogWarning($"Page {pageName} not found!");
			}

			return currentPage;
		}

		// Belirtilen sayfayı kapat
		public void ClosePage()
		{
			currentPage?.OnPageClosed();
			currentPage = null;
		}
	}
}