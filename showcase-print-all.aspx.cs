using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Showcase;

public partial class showcase_print_all : BasePage
{
	protected override void SetCssAndJs()
	{
		Helpers.GetCSSCode(uxPrintCSS);
	}

	public override void SetComponentInformation()
	{
	}

	protected int m_ShowcaseID
	{
		get
		{
			int id;
			if (!String.IsNullOrEmpty(Request.QueryString["ShowcaseID"]) && Int32.TryParse(Request.QueryString["ShowcaseID"], out id))
				return id;
			return 0;
		}
	}

	private string m_DefaultFilters
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["Filters"]) && !m_IsRental)
				return Server.UrlDecode(Request.QueryString["Filters"]);
			return "";
		}
	}

	protected string m_AgentID
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["AgentID"]))
				return Server.UrlDecode(Request.QueryString["AgentID"]);
			return "";
		}
	}

	protected string m_Address
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["Address"]))
				return Server.UrlDecode(Request.QueryString["Address"]);
			return "";
		}
	}

	protected string m_Distance
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["Distance"]))
				return Server.UrlDecode(Request.QueryString["Distance"]);
			return "";
		}
	}

	protected bool? m_OpenHouse
	{
		get
		{
			bool temp;
			if (!String.IsNullOrEmpty(Request.QueryString["OpenHouse"]) && Boolean.TryParse(Request.QueryString["OpenHouse"], out temp))
				return temp;
			return null;
		}
	}

	protected string m_SearchText
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["SearchText"]))
				return Server.UrlDecode(Request.QueryString["SearchText"]);
			return "";
		}
	}

	protected bool m_SortDirection
	{
		get
		{
			bool temp;
			if (!String.IsNullOrEmpty(Request.QueryString["SortDirection"]) && Boolean.TryParse(Request.QueryString["SortDirection"], out temp))
				return temp;
			return false;
		}
	}

	protected string m_SortField
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["SortField"]))
				return Server.UrlDecode(Request.QueryString["SortField"]);
			return "ListPrice";
		}
	}

	protected int m_PageSize
	{
		get
		{
			if (m_IsRental)
				return 0;
			int temp;
			if (!String.IsNullOrEmpty(Request.QueryString["PageSize"]) && Int32.TryParse(Request.QueryString["PageSize"], out temp))
				return temp;
			return 20;
		}
	}

	protected int m_PageNumber
	{
		get
		{
			if (m_IsRental)
				return 1;
			int temp;
			if (!String.IsNullOrEmpty(Request.QueryString["PageNumber"]) && Int32.TryParse(Request.QueryString["PageNumber"], out temp))
				return temp;
			return 1;
		}
	}

	protected bool m_IsRental { get { return m_ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes || m_ShowcaseID == (int)MeybohmShowcases.AugustaRentalHomes; } }

	protected string m_MarketPath;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			m_MarketPath = m_ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes || m_ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes ? "aiken/" : "augusta/";

			ShowcaseItem.Filters filterList = new ShowcaseItem.Filters();
			int minDistance;
			int maxDistance;
			if (!m_IsRental && !String.IsNullOrWhiteSpace(m_Address) && !String.IsNullOrWhiteSpace(m_Distance) && m_Distance.Split(':').Length > 1 && Int32.TryParse(m_Distance.Split(':')[0], out minDistance) && Int32.TryParse(m_Distance.Split(':')[1], out maxDistance))
			{
				decimal? latitude;
				decimal? longitude;
				Helpers.GetLatLong(m_Address, out latitude, out longitude);
				if (latitude.HasValue && longitude.HasValue)
				{
					filterList.AddressLat = latitude;
					filterList.AddressLong = longitude;
					filterList.MinDistance = minDistance;
					filterList.MaxDistance = maxDistance;
				}
			}
			filterList.FilterShowcaseItemActive = true.ToString();
			if (!String.IsNullOrWhiteSpace(m_AgentID))
				filterList.FilterShowcaseItemAgentID = m_AgentID;
			filterList.FilterShowcaseItemShowcaseID = m_ShowcaseID.ToString();
			if (!m_IsRental)
			{
				filterList.OpenHouse = m_OpenHouse;
				if (!String.IsNullOrWhiteSpace(m_SearchText))
					filterList.SearchText = m_SearchText;
			}

			uxHomes.DataSource = ShowcaseItem.GetPagedFilteredShowcaseItems((m_PageNumber - 1) * m_PageSize, m_PageSize, m_DefaultFilters, m_SortField, m_SortDirection, filterList);
			uxHomes.DataBind();
		}
	}
}