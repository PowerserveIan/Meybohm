using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;

namespace BaseCode
{
	public abstract class BaseGeneratedClass
	{
		protected virtual void DeleteSEO()
		{
		}

		protected virtual void DeleteSearch()
		{
		}

		protected virtual void SaveSearch()
		{
		}

		protected virtual void ClearRelatedCacheItems()
		{
		}

		/// <summary>
		/// Used to hold the item count of total records.  Used by the object paging specifc methods
		/// </summary>
		protected static int m_ItemCount;

		protected static string GetCacheFilterText(IEnumerable<KeyValuePair<string, object>> filters, string searchText, IEnumerable<string> includeList = null)
		{
			string cachingFilterText = filters.Aggregate(string.Empty, (current, kvpFilter) => current + (kvpFilter.Key + "_" + kvpFilter.Value.ToString()));

			if (!String.IsNullOrEmpty(searchText))
				cachingFilterText += "_" + searchText;
			cachingFilterText += GetCacheIncludeText(includeList);
			return cachingFilterText;
		}

		protected static string GetCacheIncludeText(IEnumerable<string> includeList = null)
		{
			return includeList != null && includeList.Any() ? "_Include_" + string.Join(",", includeList) : string.Empty;
		}

		protected static IQueryable<T> AddIncludes<T>(DbSet<T> itemQuery, IEnumerable<string> includeList = null) where T : class
		{
			return AddIncludes(itemQuery.AsQueryable(), includeList);
		}

		protected static IQueryable<T> AddIncludes<T>(IQueryable<T> itemQuery, IEnumerable<string> includeList = null) where T : class
		{
			if (includeList == null)
				return itemQuery;
			foreach (string include in includeList)
				itemQuery = itemQuery.Include(include);

			return itemQuery;
		}

		protected static IQueryable<T> SetupOrderByClause<T>(DbSet<T> itemQuery, string sortField, bool sortDirection) where T : class
		{
			return SetupOrderByClause(itemQuery.AsQueryable(), sortField, sortDirection);
		}

		protected static IQueryable<T> SetupOrderByClause<T>(IQueryable<T> itemQuery, string sortField, bool sortDirection)
		{
			if (string.IsNullOrWhiteSpace(sortField))
				return itemQuery;

			return itemQuery.OrderBy(GetOrderByClause(sortField, sortDirection));
		}

		protected static IQueryable<T> SetupQuery<T>(DbSet<T> itemQuery, string entityName, IEnumerable<KeyValuePair<string, object>> filters, string searchText, IEnumerable<string> likeSearchProperties, string sortField = null, bool sortDirection = true, IEnumerable<string> includeList = null) where T : class
		{
			return SetupQuery(itemQuery.AsQueryable(), entityName, filters, searchText, likeSearchProperties, sortField, sortDirection, includeList);
		}

		protected static IQueryable<T> SetupQuery<T>(IQueryable<T> itemQuery, string entityName, IEnumerable<KeyValuePair<string, object>> filters, string searchText, IEnumerable<string> likeSearchProperties, string sortField = null, bool sortDirection = true, IEnumerable<string> includeList = null) where T : class
		{
			itemQuery = AddIncludes(itemQuery, includeList);
			itemQuery = SetupWhereClause(itemQuery, entityName, filters, searchText, likeSearchProperties);
			itemQuery = SetupOrderByClause(itemQuery, sortField, sortDirection);
			return itemQuery;
		}

		protected static IQueryable<T> SetupWhereClause<T>(DbSet<T> itemQuery, string entityName, IEnumerable<KeyValuePair<string, object>> filters, string searchText, IEnumerable<string> likeSearchProperties) where T : class
		{
			return SetupWhereClause(itemQuery.AsQueryable(), entityName, filters, searchText, likeSearchProperties);
		}

		protected static IQueryable<T> SetupWhereClause<T>(IQueryable<T> itemQuery, string entityName, IEnumerable<KeyValuePair<string, object>> filters, string searchText, IEnumerable<string> likeSearchProperties)
		{
			List<object> queryParams = null;
			string whereClause = GetWhereClause(entityName, filters, searchText, likeSearchProperties, out queryParams);
			if (string.IsNullOrWhiteSpace(whereClause) || queryParams == null)
				return itemQuery;

			return itemQuery.Where(whereClause, queryParams.ToArray());
		}

		protected static string GetWhereClause(string entityName, IEnumerable<KeyValuePair<string, object>> filters, string searchText, IEnumerable<string> likeSearchProperties, out List<object> queryParams)
		{
			if ((filters == null || filters.Count() < 1) && (string.IsNullOrEmpty(searchText) || (likeSearchProperties == null || likeSearchProperties.Count() < 1)))
			{
				queryParams = null;
				return null;
			}

			queryParams = new List<object>();
			string whereClause = string.Empty;
			int paramItemID = -1;
			foreach (KeyValuePair<string, object> kvpFilter in filters)
			{
				if (kvpFilter.Value is string && kvpFilter.Value.ToString() == "")
					whereClause += "it." + kvpFilter.Key.Replace("@Filter" + entityName, "") + " = NULL";
				else
				{
					whereClause += "it." + kvpFilter.Key.Replace("@Filter" + entityName, "") + " = @" + (++paramItemID).ToString();
					queryParams.Add(kvpFilter.Value);
				}
				whereClause += " && ";
			}
			if (!String.IsNullOrWhiteSpace(searchText))
			{
				whereClause += "((" + String.Join(" ) || ( ", likeSearchProperties.Select(p => "it." + p + ".Contains(@" + (++paramItemID).ToString() + ")").ToArray()) + "))";
				foreach (string temp in likeSearchProperties)
					queryParams.Add(searchText);
			}
			whereClause = whereClause.Trim().TrimEnd('&').Trim();

			return whereClause;
		}

		protected static string GetOrderByClause(string sortField, bool sortDirection)
		{
			return "it." + sortField + (sortDirection ? " ASC" : " DESC");
		}

		protected static void SaveEntity(string entitySetName, object entityObj)
		{
			using (Entities entity = new Entities())
			{
				var objectContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)entity).ObjectContext;
				EntityKey key = objectContext.CreateEntityKey(entitySetName, entityObj);

				bool? isNewRecord = IsKeyForNewObject(key.EntityKeyValues.First().Value.ToString());

				if (isNewRecord.HasValue)
				{
					if (!isNewRecord.Value)
					{
						entity.Set(entityObj.GetType()).Attach(entityObj);
						entity.Entry(entityObj).State = EntityState.Modified;
					}
					else
					{
						entity.Set(entityObj.GetType()).Add(entityObj);
						entity.Entry(entityObj).State = EntityState.Added;
					}

					entity.SaveChanges();
				}
				else
					throw new Exception("Unsupported item key value for SaveEntity()");
			}
		}

		/// <summary>
		/// Checks if the key value is an int or guid and if the value is for a new record
		/// </summary>
		/// <returns></returns>
		private static bool? IsKeyForNewObject(string valueToCheck)
		{
			int itemIDInt;
			if (int.TryParse(valueToCheck, out itemIDInt))
				return (itemIDInt == 0);
			Guid itemIDGuid;
			if (Guid.TryParse(valueToCheck, out itemIDGuid))
				return (itemIDGuid == Guid.Empty);
			return null;
		}
	}
}