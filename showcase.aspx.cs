using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Showcase;

public partial class Showcase : BasePage
{
	protected int m_FilterCount;
	protected int numberOfFiltersPerRow;
	protected bool showMap;

	protected int? m_ShowcaseItemID
	{
		get
		{
			int temp;
			if (!String.IsNullOrEmpty(Request.QueryString["id"]) && Int32.TryParse(Request.QueryString["id"], out temp))
			{
				ShowcaseItem itemEntity = ShowcaseItem.GetByID(temp);
				if (itemEntity != null && itemEntity.Active)
					return temp;
			}
			return null;
		}
	}

	protected int m_ShowcaseID
	{
		get
		{
			if (ViewState["ShowcaseID"] == null)
				ViewState["ShowcaseID"] = ShowcaseHelpers.GetCurrentShowcaseID();
			return Convert.ToInt32(ViewState["ShowcaseID"]);
		}
	}

	private string m_DefaultFilters
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["Filters"]))
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
			return "null";
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

	protected int m_PageSize
	{
		get
		{
			int temp;
			if (!String.IsNullOrEmpty(Request.QueryString["PageSize"]) && Int32.TryParse(Request.QueryString["PageSize"], out temp))
				return temp;
			return 20;
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

	protected string m_SortField
	{
		get
		{
			if (!String.IsNullOrEmpty(Request.QueryString["SortField"]))
				return Server.UrlDecode(Request.QueryString["SortField"]);
			return "ListPrice";
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

	protected string m_FilterlessUrl
	{
		get
		{
			NameValueCollection myQueryString = Request.QueryString.Duplicate();
			myQueryString.Remove("id");
			myQueryString.Remove("title");
			myQueryString.Remove("Filters");
			myQueryString.Remove("Address");
			myQueryString.Remove("Distance");
			myQueryString.Remove("OpenHouse");
			myQueryString.Remove("SearchText");
			myQueryString.Remove("SortField");
			myQueryString.Remove("PageSize");
			return Request.AppRelativeCurrentExecutionFilePath.Replace("~/", Helpers.RootPath) + Helpers.WriteQueryString(myQueryString, new System.Text.StringBuilder());
		}
	}

	protected bool IsExistingHomesShowcase
	{
		get { return m_ShowcaseID == (int)MeybohmShowcases.AikenExistingHomes || m_ShowcaseID == (int)MeybohmShowcases.AugustaExistingHomes; }
	}



	protected bool IsRentalsShowcase
	{
		get { return m_ShowcaseID == (int)MeybohmShowcases.AikenRentalHomes || m_ShowcaseID == (int)MeybohmShowcases.AugustaRentalHomes; }
	}

	protected Dictionary<int, string> m_DefaultFilterValues = new Dictionary<int, string>();
	protected string m_FilterKnockoutScript = "";

	protected override void SetCssAndJs()
	{
		showMap = Settings.EnableGoogleMaps;
		if (showMap)
			uxJavaScripts.Text += ",~/tft-js/core/showcase-maps.js";
		m_AdditionalCssFiles = uxCSSFiles;
		m_AdditionalJavaScriptFiles = uxJavaScripts;
		EnableWhiteSpaceCompression = false;
		NewHomePage = Request.QueryString["showcaseid"] == ((int)NewMeybohmShowcases.AikenNewHomes).ToString() || Request.QueryString["showcaseid"] == ((int) NewMeybohmShowcases.AugustaNewHomes).ToString();
	}

	public override void SetComponentInformation()
	{
		ComponentName = "Showcase";
		ComponentAdminPage = "showcase/admin-showcase-item.aspx";
		CanonicalLink = Helpers.RootPath + "showcase.aspx?showcaseid=" + m_ShowcaseID;
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		uxFilterRepeater.ItemDataBound += uxFilterRepeater_ItemDataBound;
		numberOfFiltersPerRow = Settings.FilterDisplayStyle == FiltersDisplays.Left ? 1 : 2;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			uxAddSavedSearch.ShowcaseID = m_ShowcaseID;
			uxAddSavedSearch.NewProperties = Request.QueryString["showcaseid"] == ((int)NewMeybohmShowcases.AikenNewHomes).ToString() || Request.QueryString["showcaseid"] == ((int)NewMeybohmShowcases.AugustaNewHomes).ToString();
			if (!String.IsNullOrEmpty(m_DefaultFilters))
			{
				string[] allDefaultFilters = m_DefaultFilters.Split('|');
				foreach (string filter in allDefaultFilters)
				{
					string[] split = filter.Split(':');
					if (split.Length == 2)
						m_DefaultFilterValues.Add(Convert.ToInt32(split[0]), split[1]);
				}
			}
			List<ShowcaseAttribute> tempAttributeList = new List<ShowcaseAttribute>();
			tempAttributeList.AddRange(ShowcaseAttribute.GetAttributesAndValuesByShowcaseItemID(null, m_ShowcaseID));
			if (!showMap)
				tempAttributeList = tempAttributeList.Where(f => f.ShowcaseFilterID != (int)FilterTypes.Distance && f.ShowcaseFilterID != (int)FilterTypes.DistanceRange).ToList();
			m_FilterCount = tempAttributeList.Count;
			uxFilterRepeater.DataSource = tempAttributeList;
			uxFilterRepeater.DataBind();

			uxFilterPlaceHolder.Visible = Settings.EnableFilters && m_FilterCount > 0;
			NameValueCollection myQueryString = Request.QueryString.Duplicate();
			myQueryString.Remove("id");
			myQueryString.Remove("title");
			uxLinkToThisPage.Text = Request.AppRelativeCurrentExecutionFilePath.Replace("~/", Helpers.RootPath) + Helpers.WriteQueryString(myQueryString, new System.Text.StringBuilder());
			if (!String.IsNullOrWhiteSpace(m_SearchText))
				uxSearchText.Text = m_SearchText;
			uxToggleFilters.Visible = Settings.HideFiltersInSlideout;

			bool openHouse;
			if (!String.IsNullOrWhiteSpace(Request.QueryString["OpenHouse"]) && Boolean.TryParse(Request.QueryString["OpenHouse"], out openHouse))
				uxOpenHouse.Items.FindByValue(openHouse.ToString().ToLower()).Selected = true;

			uxOpenHouseFilterPH.Visible = IsExistingHomesShowcase ;

			int agentID;
			if (!String.IsNullOrWhiteSpace(m_AgentID) && m_AgentID != "null" && Int32.TryParse(m_AgentID, out agentID))
			{
				Classes.Media352_MembershipProvider.UserInfo agentInfo = Classes.Media352_MembershipProvider.UserInfo.UserInfoGetByUserID(agentID).FirstOrDefault();
				uxAgentImage.Visible = !String.IsNullOrWhiteSpace(agentInfo.Photo);
				if (uxAgentImage.Visible)
				{
					uxAgentImage.ImageUrl = Helpers.ResizedImageUrl(agentInfo.Photo, Globals.Settings.UploadFolder + "agents/", uxAgentImage.ResizerWidth, uxAgentImage.ResizerHeight, true);
					uxAgentImage.AlternateText = agentInfo.FirstAndLast;
				}
				uxAgentName.Text = agentInfo.FirstAndLast;
				uxAgentPhone.Text = (agentInfo.PrimaryPhone == "Office Phone" ? agentInfo.OfficePhone : agentInfo.PrimaryPhone == "Home Phone" ? agentInfo.HomePhone : agentInfo.CellPhone);
				uxAgentName2.Text = agentInfo.FirstName + "'s Listings ";
			}
			else
				uxAgentPH.Visible = false;

			uxRentPricePerMonth.Visible = IsRentalsShowcase;
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		//Register Showcase Web Service
		ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
		if (scriptManager.Services.Count(s => s.Path == "~/tft-services/Showcase/ShowcaseWebMethods.asmx") == 0)
			scriptManager.Services.Add(new ServiceReference { InlineScript = true, Path = "~/tft-services/Showcase/ShowcaseWebMethods.asmx" });
	}

	private void uxFilterRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		ShowcaseAttribute item = ((ShowcaseAttribute)e.Item.DataItem);
		if (EnumParser.Parse<FilterTypes>(item.ShowcaseFilterID.ToString()) == FilterTypes.Distance ||
			EnumParser.Parse<FilterTypes>(item.ShowcaseFilterID.ToString()) == FilterTypes.DistanceRange)
		{
			if (!String.IsNullOrEmpty(m_Address))
			{
				TextBox uxFilterAddress = (TextBox)e.Item.FindControl("uxFilterAddress");
				uxFilterAddress.Text = m_Address;
			}
		}
		m_FilterKnockoutScript += "viewModel.FilterList.push(new filterItem(" + item.ShowcaseAttributeID + @", """ + EnumParser.Parse<FilterTypes>(item.ShowcaseFilterID.ToString()).ToString() + @""", """ + (m_DefaultFilterValues.ContainsKey(item.ShowcaseAttributeID) ? m_DefaultFilterValues[item.ShowcaseAttributeID] : "") + @"""));";
	}

	protected void uxRadioButtonGrid_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		ShowcaseAttribute currentItem = (ShowcaseAttribute)((RepeaterItem)e.Item.NamingContainer.Parent.Parent).DataItem;
		RadioButtonList uxRadioButtons = (RadioButtonList)e.Item.FindControl("uxRadioButtons");
		Label uxAttributeValue = (Label)e.Item.FindControl("uxAttributeValue");
		bool foundOne = false;
		if (uxRadioButtons.Items.Count > 2)
		{
			if (!String.IsNullOrEmpty(m_DefaultFilters) && m_DefaultFilterValues.ContainsKey(currentItem.ShowcaseAttributeID))
			{
				string[] values = m_DefaultFilterValues[currentItem.ShowcaseAttributeID].Split(',');

				if (values.Contains(uxAttributeValue.Text + "[Yes]"))
				{
					uxRadioButtons.Items[0].Selected = true;
					foundOne = true;
				}
				else if (values.Contains(uxAttributeValue.Text + "[No]"))
				{
					uxRadioButtons.Items[1].Selected = true;
					foundOne = true;
				}
			}

			if (!foundOne)
				uxRadioButtons.Items[2].Selected = true;
		}
	}

	protected void FilterList_DataBound(object sender, EventArgs e)
	{
		if (((Control)sender).ClientID.Contains("uxFilterRangeSlider"))
		{
			DropDownList slider = (DropDownList)sender;
			Literal uxMaximumAmount = (Literal)slider.NamingContainer.FindControl("uxMaximumAmount");
			Literal uxMinimumAmount = (Literal)slider.NamingContainer.FindControl("uxMinimumAmount");
			if (slider.Items.Count > 0)
			{
				if (slider.ClientID.Contains("uxFilterRangeSlider2"))
				{
					slider.Items[slider.Items.Count - 1].Selected = true;
					uxMaximumAmount.Text = slider.Items[slider.Items.Count - 1].Text;
				}
				else
				{
					slider.Items[0].Selected = true;
					uxMinimumAmount.Text = slider.Items[0].Text;
				}
			}
		}
		ListControl listEntity = (ListControl)sender;
		if (!String.IsNullOrEmpty(m_Distance) && listEntity.NamingContainer.FindControl("uxFilterAddress") != null && listEntity.NamingContainer.FindControl("uxFilterAddress").Visible)
		{
			string[] dValues = m_Distance.Split(':');
			if (dValues[0] == "null" && dValues.Length > 1)
				dValues[0] = dValues[1];
			if (listEntity.ClientID.Contains("uxFilterRangeSlider"))
			{
				if (listEntity.ClientID.Contains("uxFilterRangeSlider2") && listEntity.Items.FindByValue((dValues.Length == 2 ? dValues[1] : dValues[0])) != null)
				{
					listEntity.ClearSelection();
					listEntity.Items.FindByValue((dValues.Length == 2 ? dValues[1] : dValues[0])).Selected = true;
					Literal uxMaximumAmount = (Literal)listEntity.NamingContainer.FindControl("uxMaximumAmount");
					uxMaximumAmount.Text = dValues.Length == 2 ? dValues[1] : dValues[0];
				}
				else
				{
					listEntity.ClearSelection();
					if (listEntity.Items.FindByValue(dValues[0]) != null)
						listEntity.Items.FindByValue(dValues[0]).Selected = true;
					Literal uxMinimumAmount = (Literal)listEntity.NamingContainer.FindControl("uxMinimumAmount");
					if (uxMinimumAmount != null)
						uxMinimumAmount.Text = dValues[0];
				}
			}
			else if (listEntity.ClientID.Contains("Slider") && listEntity.Items.FindByValue(dValues[0]) != null)
			{
				listEntity.ClearSelection();
				listEntity.Items.FindByValue(dValues[0]).Selected = true;
			}
		}
		if (!String.IsNullOrEmpty(m_DefaultFilters))
		{
			int attributeID = Convert.ToInt32(((HiddenField)listEntity.NamingContainer.FindControl("uxAttributeID")).Value);
			if (m_DefaultFilterValues.ContainsKey(attributeID))
			{
				string[] values = m_DefaultFilterValues[attributeID].Split(',');
				bool foundOne = false;

				if (listEntity.ClientID.Contains("uxFilterRangeSlider") && (values[0].Contains("<") || values[0].Contains(">")))
				{
					values = m_DefaultFilterValues[attributeID].Split('<');
					if (values.Length == 2 && listEntity.ClientID.Contains("uxFilterRangeSlider2") && listEntity.Items.FindByValue(values[1]) != null)
					{
						listEntity.ClearSelection();
						listEntity.Items.FindByValue(values[1]).Selected = true;
						Literal uxMaximumAmount = (Literal)listEntity.NamingContainer.FindControl("uxMaximumAmount");
						uxMaximumAmount.Text = values[1];
					}
					values = values[0].Split('>');
					if (values.Length == 2 && !listEntity.ClientID.Contains("uxFilterRangeSlider2") && listEntity.Items.FindByValue(values[1]) != null)
					{
						listEntity.ClearSelection();
						listEntity.Items.FindByValue(values[1]).Selected = true;
						Literal uxMinimumAmount = (Literal)listEntity.NamingContainer.FindControl("uxMinimumAmount");
						if (uxMinimumAmount != null)
							uxMinimumAmount.Text = values[1];
					}
				}
				else if (listEntity.ClientID.Contains("Slider") && listEntity.Items.FindByValue(m_DefaultFilterValues[attributeID]) != null)
				{
					listEntity.ClearSelection();
					listEntity.Items.FindByValue(m_DefaultFilterValues[attributeID]).Selected = true;
				}
				else
				{
					//Disable the default "All" functionality in hopes that the query string has not been tampered with
					if (listEntity.Items.FindByValue("all") != null)
					{
						listEntity.Items.FindByValue("all").Selected = false;
						listEntity.Items.FindByValue("all").Enabled = true;
					}
					foreach (ListItem listItem in listEntity.Items)
					{
						if (values.Contains(listItem.Text))
							listItem.Selected = foundOne = true;
					}
					if (!foundOne && listEntity.Items.FindByValue("all") != null)//The query string has been tampered with, reset to default functionality
					{
						listEntity.Items.FindByValue("all").Selected = true;
						listEntity.Items.FindByValue("all").Enabled = false;
					}
				}
			}
		}
	}

	protected double GetMinAttributeValue(ShowcaseAttribute attributeEntity)
	{
		if (attributeEntity.MinimumValue.HasValue)
			return Convert.ToDouble(attributeEntity.MinimumValue.Value);
		List<ShowcaseAttributeValue> values = attributeEntity.ShowcaseAttributeValues;
		
		try
        {
            return values.Count > 0 ? values.Min(s => Convert.ToDouble(s.Value)) : 0;
        }
        catch(Exception e)
        {
            return 0;
        }
	}

	protected double GetMaxAttributeValue(ShowcaseAttribute attributeEntity)
	{
		if (attributeEntity.MaximumValue.HasValue)
			return Convert.ToDouble(attributeEntity.MaximumValue.Value);
		List<ShowcaseAttributeValue> values = attributeEntity.ShowcaseAttributeValues;

        try
        {
            return values.Count > 0 ? values.Max(s => Convert.ToDouble(s.Value)) : 0;
        }
        catch(Exception e)
        {
            return 5;
        }
	}

	protected int GetSliderStepRange(ShowcaseAttribute attributeEntity, int numberOfSteps)
	{
		List<ShowcaseAttributeValue> values = attributeEntity.ShowcaseAttributeValues;
		if (values.Count == 0)
			return 1;
		int step = Convert.ToInt32(((GetMaxAttributeValue(attributeEntity) - GetMinAttributeValue(attributeEntity)) / numberOfSteps));
		return step > 1 ? step : 1;
	}

	protected bool IsRangeSlider(int filterID)
	{
		return filterID == (int)FilterTypes.RangeSlider || filterID == (int)FilterTypes.DistanceRange;
	}

	protected List<string> GetRangeSliderValues(ShowcaseAttribute attributeEntity, bool isRangeSlider)
	{
		if (!isRangeSlider)
			return new List<string>();
		int step = GetSliderStepRange(attributeEntity, 25);
		double i = GetMinAttributeValue(attributeEntity);
		double max = GetMaxAttributeValue(attributeEntity);
		bool modifiedStep = false;
		if (step > 1000)
		{
			if (step < 5000)
				step = 5000;
			else if (step < 10000)
				step = 10000;
			else if (step < 25000)
				step = 25000;
			else if (step < 50000)
				step = 50000;
			modifiedStep = true;
		}
		List<string> sliderValues = new List<string>();
		while (i <= max)
		{
			sliderValues.Add(i.ToString());
			if (modifiedStep && sliderValues.Count == 1)
				i = step;
			else
				i += step;
			if (sliderValues.Count > 25)
				break;
		}
		if (attributeEntity.MinimumValue.HasValue)
			sliderValues[0] = "< " + sliderValues[0];
		if (attributeEntity.MaximumValue.HasValue)
			sliderValues[sliderValues.Count - 1] = attributeEntity.MaximumValue.ToString().Replace(".00", "") + "+";
		return sliderValues;
	}


	protected string SetupAttributeFiltersForJavaScriptHiding()
	{
		return "[" + string.Join(",", ShowcaseAttribute.GetAttributeIDAndFiltersByShowcaseID(m_ShowcaseID).Select(a => a.GetForArray).ToArray()) + "]";
	}

	protected string SetupAttributeFiltersForJavaScriptShowing()
	{
		return "{" + string.Join(",", ShowcaseAttribute.GetAttributeIDAndFiltersByShowcaseID(m_ShowcaseID).Select(a => a.GetForArrayOjObjects).ToArray()) + "}";
	}
}