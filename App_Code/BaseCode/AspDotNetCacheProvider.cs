using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace BaseCode
{
	public class AspDotNetCacheProvider : ICacheProvider
	{
		private readonly HttpContextBase m_HttpContext;
		private readonly bool m_EnableCaching;
		private readonly int m_CacheDuration;
		private static readonly EmptyCacheItem m_EmptyCacheItem = new EmptyCacheItem();

		public AspDotNetCacheProvider(HttpContextBase httpContext, bool enableCaching, int? cacheDuration = null)
		{
			m_HttpContext = httpContext;
			m_EnableCaching = enableCaching;
			m_CacheDuration = cacheDuration.HasValue ? cacheDuration.Value : BaseCode.Globals.Settings.DefaultCacheDuration;
		}

		public object this[string key]
		{
			get { return Get(key); }
			set { Store(key, value); }
		}

		public object Get(string key)
		{
			return m_HttpContext.Cache[key];
		}

		public void Store(string key, object obj)
		{
			if (m_EnableCaching)
			{
				if (obj != null)
					m_HttpContext.Cache.Insert(key, obj, null, DateTime.Now.AddSeconds(m_CacheDuration), TimeSpan.Zero);
				else
					m_HttpContext.Cache.Insert(key, m_EmptyCacheItem, null, DateTime.Now.AddSeconds(m_CacheDuration), TimeSpan.Zero);
			}
		}

		public object Remove(string key)
		{
			return Cache.Remove(key);
		}


		public System.Web.Caching.Cache Cache
		{
			get { return m_HttpContext.Cache; }
		}
		/// <summary>
		/// from http://www.aspdotnetfaq.com/Faq/How-to-clear-your-ASP-NET-applications-Cache.aspx
		/// </summary>
		public void Purge(string prefix = null)
		{
			List<string> keys = new List<string>();
			// retrieve application Cache enumerator
			IDictionaryEnumerator enumerator = Cache.GetEnumerator();
			// copy all keys that currently exist in Cache
			if (prefix != null)
			{
				while (enumerator.MoveNext())
				{
					string key = enumerator.Key.ToString();
					if (key.StartsWith(prefix))
						keys.Add(key);
				}
			}
			else
			{
				keys.Add(enumerator.Key.ToString());
			}
			// delete every key from cache
			foreach (string key in keys)
				Cache.Remove(key);
		}

		public bool IsEnabled
		{
			get { return m_HttpContext != null && m_EnableCaching; }
		}

		public bool IsEmptyCacheItem(string key)
		{
			return Get(key) is EmptyCacheItem;
		}

		private class EmptyCacheItem
		{
			public EmptyCacheItem()
			{
			}
		}
	}
}