using System.Collections.Generic;
using System.Linq;

namespace Classes.Showcase
{
	public partial class ShowcaseAttribute
	{
		public string FilterType
		{
			get { return Filter.GetByID(ShowcaseFilterID).Type; }
		}

		/// <summary>
		/// List of all attribute values assigned to a ShowcaseItem and an attribute
		/// Populated only by GetAttributesAndValueByShowcaseItemID
		/// </summary>
		public List<ShowcaseAttributeValue> ShowcaseAttributeValues { get; set; }

		/// <summary>
		/// Gets all attributes and values for a showcase item
		/// </summary>
		public static List<ShowcaseAttribute> GetAttributesAndValuesByShowcaseItemID(int? showcaseItemID, int? showcaseID = null, bool active = true)
		{
			List<ShowcaseAttribute> objects;
			string key = cacheKeyPrefix + "GetAttributesAndValuesByShowcaseItemID_" + showcaseItemID + "_" + showcaseID + "_" + active;

			List<ShowcaseAttribute> tmpClass = null;

			if (Cache.IsEnabled)
				tmpClass = Cache[key] as List<ShowcaseAttribute>;

			if (tmpClass != null)
				objects = tmpClass;
			else
			{
				List<ShowcaseAttributeValue> values = new List<ShowcaseAttributeValue>();
				using (Entities entity = new Entities())
				{
					var itemQuery = entity.ShowcaseAttribute.Where(a => !active || a.Active);
					if (showcaseID.HasValue)
						itemQuery = itemQuery.Where(a => a.ShowcaseID == showcaseID.Value && (a.ShowcaseFilterID == (int)FilterTypes.Distance || a.ShowcaseFilterID == (int)FilterTypes.DistanceRange || a.ShowcaseAttributeValue.Any(v => v.ShowcaseItemAttributeValue.Any(s => s.ShowcaseItem.Active))));
					else if (showcaseItemID.HasValue)
						itemQuery = itemQuery.Where(a => a.ShowcaseAttributeValue.Any(v => v.ShowcaseItemAttributeValue.Any(s => s.ShowcaseItem.Active && s.ShowcaseItemID == showcaseItemID.Value)));
					objects = itemQuery.ToList();

					var valueQuery = entity.ShowcaseAttributeValue.Where(v => !active || v.ShowcaseAttribute.Active);
					if (showcaseID.HasValue)
						valueQuery = valueQuery.Where(v => v.ShowcaseAttribute.ShowcaseID == showcaseID.Value && (v.ShowcaseAttribute.ShowcaseFilterID == (int)FilterTypes.Distance || v.ShowcaseAttribute.ShowcaseFilterID == (int)FilterTypes.DistanceRange || v.ShowcaseItemAttributeValue.Any(s => s.ShowcaseItem.Active)) && v.DisplayInFilters);
					else if (showcaseItemID.HasValue)
						valueQuery = valueQuery.Where(v => v.ShowcaseItemAttributeValue.Any(s => s.ShowcaseItem.Active && s.ShowcaseItemID == showcaseItemID.Value));

					values = valueQuery.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Value).ToList();
				}

				foreach (ShowcaseAttribute obj in objects)
				{
					obj.ShowcaseAttributeValues = values.Where(v => v.ShowcaseAttributeID == obj.ShowcaseAttributeID).ToList();
				}

				objects = objects.OrderBy(a => a.DisplayOrder).ToList();

				Cache.Store(key, objects);
			}
			return objects;
		}


		/// <summary>
		/// Gets all attribute/filter a showcase item
		/// </summary>
		public static List<AttributeIDFilterID> GetAttributeIDAndFiltersByShowcaseID(int showcaseID)
		{
			List<AttributeIDFilterID> objects;
			string key = cacheKeyPrefix + "GetAttributeIDAndFiltersByShowcaseID_" + showcaseID;

			List<AttributeIDFilterID> tmpClass = null;

			if (Cache.IsEnabled)
				tmpClass = Cache[key] as List<AttributeIDFilterID>;

			if (tmpClass != null)
				objects = tmpClass;
			else
			{
				List<AttributeIDFilterID> values = new List<AttributeIDFilterID>();


				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseAttribute.Where
								(
									a =>
										a.Active
										&& a.ImportItemAttribute
										&& a.ShowcaseID == showcaseID
										&&
										(
											a.ShowcaseFilterID == (int)Classes.Showcase.FilterTypes.CheckBoxList
											|| a.ShowcaseFilterID == (int)Classes.Showcase.FilterTypes.DropDown
											|| a.ShowcaseFilterID == (int)Classes.Showcase.FilterTypes.ListBox
											|| a.ShowcaseFilterID == (int)Classes.Showcase.FilterTypes.RadioButtonList
											//|| a.ShowcaseFilterID == (int)Classes.Showcase.FilterTypes.RadioButtonGrid
										)
								)
								.Select(a => new AttributeIDFilterID() { AttributeID = a.ShowcaseAttributeID, FilterID = a.ShowcaseFilterID })
								.Distinct()
								.OrderBy(a => a.AttributeID)
								.ToList();
				}

				Cache.Store(key, objects);
			}
			return objects;
		}


		/// <summary>
		/// Gets all attribute/filter a showcase item
		/// </summary>
		public static int GetAttributeIDByShowcaseIDAndAttributeTitle(int showcaseID, string attributeName)
		{
			int objects;
			string key = cacheKeyPrefix + "GetAttributeIDByShowcaseIDAndAttributeTitle_" + showcaseID + "_" + attributeName;

			int? tmpClass = null;

			if (Cache.IsEnabled)
				tmpClass = Cache[key] as int?;

			if (tmpClass != null)
				objects = tmpClass.Value;
			else
			{
				using (Entities entity = new Entities())
				{
					objects = entity.ShowcaseAttribute.Where
								(
									a =>
										a.Active
										&& a.ShowcaseID == showcaseID
										&& a.Title == attributeName
								)
								.Select(a => a.ShowcaseAttributeID)
								.FirstOrDefault();
				}

				Cache.Store(key, objects);
			}
			return objects;
		}
	}
}