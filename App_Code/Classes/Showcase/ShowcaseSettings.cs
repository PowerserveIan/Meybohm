using System;
using System.Configuration;
using Classes.ConfigurationSettings;

namespace Classes.Showcase
{
	public partial class Settings
	{
		/// <summary>
		/// Version number of the component
		/// </summary>
		public static string VersionNumber
		{
			get { return ConfigurationManager.AppSettings["Showcase_componentVersion"]; }
		}

		/// <summary>
		/// Display style of Showcase item attributes on the frontend (either above the description or in their own tab)
		/// </summary>
		public static AttributeDisplays AttributeDisplayStyle
		{
			get
			{
				string display;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					display = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_attributeDisplayStyle") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_attributeDisplayStyle"] : "";
				else
					display = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_attributeDisplayStyle") ? SiteSettings.GetSettingKeyValuePair()["Showcase_attributeDisplayStyle"] : "";

				if (String.IsNullOrEmpty(display) || display.Equals("Nowhere", StringComparison.OrdinalIgnoreCase))
					return AttributeDisplays.None;
				if (display.Equals("Tabs", StringComparison.OrdinalIgnoreCase))
					return AttributeDisplays.Tabs;
				return AttributeDisplays.Description;
			}
		}

		/// <summary>
		/// Display style of filters on the listing page
		/// </summary>
		public static FiltersDisplays FilterDisplayStyle
		{
			get
			{
				string display;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					display = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_filterDisplayStyle") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_filterDisplayStyle"] : "";
				else
					display = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_filterDisplayStyle") ? SiteSettings.GetSettingKeyValuePair()["Showcase_filterDisplayStyle"] : "";

				if (String.IsNullOrEmpty(display) || !display.Equals("Left", StringComparison.OrdinalIgnoreCase))
					return FiltersDisplays.Top;
				return FiltersDisplays.Left;
			}
		}

		/// <summary>
		/// Text that will appear as the label of the attributes tab
		/// </summary>
		public static string AttributeTabText
		{
			get
			{
				string text;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					text = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_attributeTabText") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_attributeTabText"] : "";
				else
					text = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_attributeTabText") ? SiteSettings.GetSettingKeyValuePair()["Showcase_attributeTabText"] : "";

				if (String.IsNullOrEmpty(text))
					return "Attributes";
				return text;
			}
		}

		/// <summary>
		/// Automatically play Youtube videos on the frontend
		/// </summary>
		public static bool AutoplayVideos
		{
			get
			{
				string playVideos;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					playVideos = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_autoplayVideos") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_autoplayVideos"] : "";
				else
					playVideos = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_autoplayVideos") ? SiteSettings.GetSettingKeyValuePair()["Showcase_autoplayVideos"] : "";

				if (String.IsNullOrEmpty(playVideos))
					return true;
				return Convert.ToBoolean(playVideos);
			}
		}

		/// <summary>
		/// Turns on/off filtering on the frontend
		/// </summary>
		public static bool EnableFilters
		{
			get
			{
				string enableFilters;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					enableFilters = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_enableFilters") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_enableFilters"] : "";
				else
					enableFilters = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_enableFilters") ? SiteSettings.GetSettingKeyValuePair()["Showcase_enableFilters"] : "";

				if (String.IsNullOrEmpty(enableFilters))
					return true;
				return Convert.ToBoolean(enableFilters);
			}
		}

		/// <summary>
		/// The number of filters that will display by default.
		/// Warning: could potentially break design of showcase if changed.
		/// </summary>
		public static int NumberFiltersVisible
		{
			get
			{
				string numberFilters;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					numberFilters = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_numberFiltersVisible") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_numberFiltersVisible"] : "";
				else
					numberFilters = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_numberFiltersVisible") ? SiteSettings.GetSettingKeyValuePair()["Showcase_numberFiltersVisible"] : "";

				if (String.IsNullOrEmpty(numberFilters))
					return 3;
				return Convert.ToInt32(numberFilters);
			}
		}

		/// <summary>
		/// Adds a required address field to each Showcase Item.  Will display map on frontend.
		/// </summary>
		public static bool EnableGoogleMaps
		{
			get
			{
				string enableGoogleMaps;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					enableGoogleMaps = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_googleMaps") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_googleMaps"] : "";
				else
					enableGoogleMaps = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_googleMaps") ? SiteSettings.GetSettingKeyValuePair()["Showcase_googleMaps"] : "";

				if (String.IsNullOrEmpty(enableGoogleMaps))
					return true;
				return Convert.ToBoolean(enableGoogleMaps);
			}
		}

		/// <summary>
		/// Ability to have multiple data sets in separate showcases
		/// </summary>
		public static bool MultipleShowcases
		{
			get { return Convert.ToBoolean(ConfigurationManager.AppSettings["Showcase_multipleShowcases"]); }
		}

		/// <summary>
		/// Toggles statistics tracking for showcase items
		/// </summary>
		public static bool EnableStatisticsTracking
		{
			get
			{
				if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Showcase_enableStatTracking"]))
					return false;
				return Convert.ToBoolean(ConfigurationManager.AppSettings["Showcase_enableStatTracking"]);
			}
		}

		/// <summary>
		/// If set, filters will be hidden at startup and the user must click a button to expand them.
		/// </summary>
		public static bool HideFiltersInSlideout
		{
			get
			{
				string hideFilters;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					hideFilters = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_hideFiltersInSlideout") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_hideFiltersInSlideout"] : "";
				else
					hideFilters = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_hideFiltersInSlideout") ? SiteSettings.GetSettingKeyValuePair()["Showcase_hideFiltersInSlideout"] : "";

				if (String.IsNullOrEmpty(hideFilters))
					return false;
				return Convert.ToBoolean(hideFilters);
			}
		}

		public static int DistanceForNearbyLocations
		{
			get
			{
				string distance;
				int? currentShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID();
				if (currentShowcaseID.HasValue)
					distance = ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value).ContainsKey("Showcase_distanceForNearbyLocations") ? ShowcaseSiteSettings.GetSettingKeyValuePair(currentShowcaseID.Value)["Showcase_distanceForNearbyLocations"] : "";
				else
					distance = SiteSettings.GetSettingKeyValuePair().ContainsKey("Showcase_distanceForNearbyLocations") ? SiteSettings.GetSettingKeyValuePair()["Showcase_distanceForNearbyLocations"] : "";

				if (String.IsNullOrEmpty(distance))
					return 5;
				return Convert.ToInt32(distance);
			}
		}

		public static int AikenExistingPropertyTypeAttributeID
		{
			get
			{
				return Convert.ToInt32(ConfigurationManager.AppSettings["Showcase_aikenExistingPropertyTypeAttributeID"]);
			}
		}

		public static int AugustaExistingPropertyTypeAttributeID
		{
			get
			{
				return Convert.ToInt32(ConfigurationManager.AppSettings["Showcase_augustaExistingPropertyTypeAttributeID"]);
			}
		}
	}
}