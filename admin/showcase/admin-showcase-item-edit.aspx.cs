using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using BaseCode;
using Classes.Showcase;
using Classes.StateAndCountry;
using Settings = Classes.Showcase.Settings;

public partial class Admin_AdminShowcaseItemEdit : BaseEditPage
{
	public ShowcaseItem ShowcaseItemEntity { get; set; }

	protected List<ShowcaseAttributeValue> AllAttributesAssignedToItem
	{
		get
		{
			if (ViewState["allAttributesAssignedToItem"] == null)
				return ShowcaseAttributeValue.GetAllAttributeValuesByShowcaseItemID(EntityId);
			return (List<ShowcaseAttributeValue>)(ViewState["allAttributesAssignedToItem"]);
		}
		set { ViewState["allAttributesAssignedToItem"] = value; }
	}

	protected override void OnInit(EventArgs e)
	{
		m_Header = uxHeader;
		m_SavePanel = uxPanel;
		m_ButtonContainer = uxButtonContainer;
		m_LinkToListingPage = "admin-showcase-item.aspx";
		int temp;
		if (!String.IsNullOrEmpty(Request.QueryString["ShowcaseID"]) && Int32.TryParse(Request.QueryString["ShowcaseID"], out temp))
			ShowcaseHelpers.SetUsersCurrentShowcaseID(temp);
		if (!ShowcaseHelpers.GetCurrentShowcaseID().HasValue && (!ShowcaseHelpers.IsShowcaseAdmin() || EntityId == 0))
			Response.Redirect("~/admin/showcase/admin-showcases.aspx");
		else if (!ShowcaseHelpers.GetCurrentShowcaseID().HasValue && EntityId > 0)
		{
			ShowcaseItemEntity = ShowcaseItem.GetByID(EntityId);
			if (ShowcaseItemEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(ShowcaseItemEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);
			ShowcaseHelpers.SetUsersCurrentShowcaseID(ShowcaseItemEntity.ShowcaseID);
		}
		m_ClassName = (Settings.MultipleShowcases && ShowcaseHelpers.UserCanManageOtherShowcases() ? Showcases.GetByID(ShowcaseHelpers.GetCurrentShowcaseID().Value).Title + " " : "") + " Property";
		base.OnInit(e);
		m_SaveAndAddNewButton.Visible = m_AddNewButton.Visible = !ShowcaseHelpers.IsCurrentShowcaseMLS();
		m_ValidationSummary.DisplayMode = ValidationSummaryDisplayMode.BulletList;
		uxAttributeRepeater.ItemDataBound += uxAttributeRepeater_ItemDataBound;
		uxOwnerNameCV.ServerValidate += uxOwnerNameCV_ServerValidate;
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		if (!IsPostBack)
		{
			List<Classes.Media352_MembershipProvider.UserInfo> allAgents = Classes.Media352_MembershipProvider.UserInfo.GetAllAgents();
			uxAgentID.DataSource = ShowcaseHelpers.IsCurrentShowcaseMLS() ? allAgents : allAgents.Where(u => u.StaffTypeID == (int)Classes.Media352_MembershipProvider.StaffTypes.RentalRealtor).ToList();
			uxOpenHouseAgentID.DataSource = allAgents;
			uxAgentID.DataTextField = uxOpenHouseAgentID.DataTextField = "FirstAndLast";
			uxAgentID.DataValueField = uxOpenHouseAgentID.DataValueField = "UserID";
			uxAgentID.DataBind();
			uxOpenHouseAgentID.DataBind();

			uxNeighborhoodID.DataSource = Classes.MLS.Neighborhood.GetAll().OrderBy(n => n.Name);
			uxNeighborhoodID.DataTextField = "Name";
			uxNeighborhoodID.DataValueField = "NeighborhoodID";
			uxNeighborhoodID.DataBind();

			uxOfficeID.DataSource = Classes.MLS.Office.GetAll();
			uxOfficeID.DataTextField = "Name";
			uxOfficeID.DataValueField = "OfficeID";
			uxOfficeID.DataBind();

			if (EntityId > 0)
			{
				ShowcaseItemEntity = ShowcaseItem.GetByID(EntityId);
				if (ShowcaseItemEntity == null || (!ShowcaseHelpers.IsShowcaseAdmin() && !ShowcaseUser.ShowcaseUserGetByShowcaseID(ShowcaseItemEntity.ShowcaseID).Exists(s => s.UserID == Helpers.GetCurrentUserID())))
					Response.Redirect(m_LinkToListingPage + ReturnQueryString);

				LoadData();
				uxCollectionPlaceHolder.Visible = true;
				uxEditCollection.NavigateUrl = "~/admin/showcase/admin-media-collection.aspx?FilterMediaCollectionShowcaseItemID=" + EntityId;
                uxEditPropertyStatus.NavigateUrl = "~/admin/showcase/admin-showcase-item-statusEdit.aspx?id=" + EntityId;
			}
			else if (ShowcaseHelpers.IsCurrentShowcaseMLS())
				Response.Redirect(m_LinkToListingPage + ReturnQueryString);
			else
				NewRecord = true;

			uxLocationPlaceHolder.Visible = Settings.EnableGoogleMaps;

			//SEO code
			if (EntityId > 0)
			{
				uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(ShowcaseItemEntity.ShowcaseItemID));
				uxSEOData.LoadControlData();
			}
			else
				uxSEOData.LoadControlData(true);
		}
		if (ShowcaseHelpers.IsCurrentShowcaseMLS())
		{
			uxImage.ReadOnly =
			uxAddress.ReadOnly =
			uxSummary.ReadOnly =
			uxTitle.ReadOnly = true;

			uxAgentID.Enabled =
			uxNeighborhoodID.Enabled =
			uxOfficeID.Enabled =
			uxRentedLI.Visible =
			uxRentalPlaceHolder.Visible =
			uxAvailabilityDatePH.Visible = false;
		}
		else
			uxOpenHousePlaceHolder.Visible = uxMLSIDPH.Visible = false;

		
			uxNewHomeSoldPlaceHolder.Visible = (ShowcaseItemEntity != null)&& ShowcaseItemEntity.NewHome && ShowcaseHelpers.IsCurrentShowcaseMLS();
	}

	protected override void Save()
	{
		ShowcaseItemEntity = EntityId > 0 ? ShowcaseItem.GetByID(EntityId) : new ShowcaseItem();
		if (ShowcaseItemEntity.NewHome)
		{
			uxSoldHomeControl.Required = uxSoldHomeIsSold.Checked;
			Page.Validate("");
		}
		uxImage.CommitChanges();
		if (IsValid)
		{
			ShowcaseItemEntity.Active = uxActive.Checked;
			ShowcaseItemEntity.Featured = uxFeatured.Checked;
			if (!ShowcaseHelpers.IsCurrentShowcaseMLS())
			{
				uxAddress.Save();

				ShowcaseItemEntity.AgentID = !String.IsNullOrEmpty(uxAgentID.SelectedValue) ? (int?)Convert.ToInt32(uxAgentID.SelectedValue) : null;
				ShowcaseItemEntity.AvailabilityDate = uxAvailabilityDate.SelectedDate.HasValue ? (DateTime?)Helpers.ConvertClientTimeToUTC(uxAvailabilityDate.SelectedDate.Value) : null;
				ShowcaseItemEntity.Image = uxImage.FileName;
				ShowcaseItemEntity.NeighborhoodID = !String.IsNullOrEmpty(uxNeighborhoodID.SelectedValue) ? (int?)Convert.ToInt32(uxNeighborhoodID.SelectedValue) : null;
				ShowcaseItemEntity.OfficeID = !String.IsNullOrEmpty(uxOfficeID.SelectedValue) ? (int?)Convert.ToInt32(uxOfficeID.SelectedValue) : null;
				ShowcaseItemEntity.Rented = uxRented.Checked;
				ShowcaseItemEntity.Summary = uxSummary.Text;
				ShowcaseItemEntity.Title = uxTitle.Text;

				ShowcaseItemEntity.AddressID = uxAddress.AddressID.Value;
				ShowcaseItemEntity.ShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID().Value;
			}
			else
			{
				ShowcaseItemEntity.OpenHouseAgentID = !String.IsNullOrEmpty(uxOpenHouseAgentID.SelectedValue) ? (int?)Convert.ToInt32(uxOpenHouseAgentID.SelectedValue) : null;
			}

			ShowcaseItemEntity.Website = (!String.IsNullOrEmpty(uxWebsite.Text) && !uxWebsite.Text.StartsWith("http") ? "http://" : "") + uxWebsite.Text;
			ShowcaseItemEntity.VirtualTourURL = (!String.IsNullOrEmpty(uxVirtualTourURL.Text) && !uxVirtualTourURL.Text.StartsWith("http") ? "http://" : "") + uxVirtualTourURL.Text;
			ShowcaseItemEntity.Directions = uxDirections.Text;
			ShowcaseItemEntity.EmailAddresses = uxEmailAddresses.Text;
			ShowcaseItemEntity.StatsSentToAgent = uxSendStatsTo.SelectedValue == "Both" || uxSendStatsTo.SelectedValue == "Agent";
			ShowcaseItemEntity.StatsSentToOwner = uxSendStatsTo.SelectedValue == "Both" || uxSendStatsTo.SelectedValue == "Owner";

			ShowcaseItemEntity.Save();
			EntityId = ShowcaseItemEntity.ShowcaseItemID;

			//Save Rental data
			if (!ShowcaseHelpers.IsCurrentShowcaseMLS())
			{
				ShowcaseItemRental rentalEntity = NewRecord ? new ShowcaseItemRental { ShowcaseItemID = EntityId } : ShowcaseItemRental.ShowcaseItemRentalGetByShowcaseItemID(EntityId).FirstOrDefault();
				if (rentalEntity == null)
					rentalEntity = new ShowcaseItemRental { ShowcaseItemID = EntityId };
				rentalEntity.CompanyName = uxCompanyName.Text;
				rentalEntity.ContactName = uxContactName.Text;
				rentalEntity.ContactPhone = uxContactPhone.Text;
				rentalEntity.LeaseBeginDate = uxLeaseBegins.SelectedDate.HasValue ? (DateTime?)Helpers.ConvertClientTimeToUTC(uxLeaseBegins.SelectedDate.Value) : null;
				rentalEntity.OwnerName = uxOwnerName.Text;
				rentalEntity.Save();
			}
			else
			{
				if (!uxRecurring.Checked)
				{
					List<OpenHouse> openHouses = OpenHouse.OpenHouseGetByShowcaseItemID(EntityId);
					if (uxBeginDate.SelectedDate.HasValue)
					{
						OpenHouse currentOpenHouse = openHouses.FirstOrDefault();
						if (currentOpenHouse == null)
							currentOpenHouse = new OpenHouse { ShowcaseItemID = EntityId };
						currentOpenHouse.BeginDate = Helpers.ConvertClientTimeToUTC(uxBeginDate.SelectedDate.Value);
						currentOpenHouse.EndDate = uxEndDate.SelectedDate.HasValue ? (DateTime?)Helpers.ConvertClientTimeToUTC(uxEndDate.SelectedDate.Value) : null;
						currentOpenHouse.Save();
						openHouses.ForEach(h => { if (h.OpenHouseID != currentOpenHouse.OpenHouseID) h.Delete(); });
					}
					else
						openHouses.ForEach(h => { h.Delete(); });
				}
				if (ShowcaseItemEntity.NewHome && uxSoldHomeIsSold.Checked)
				{
					uxSoldHomeControl.ShowcaseItemID = EntityId;
					uxSoldHomeControl.SaveData();
				}
			}

			//SEO saving should not be done until the new product has been created
			if (ShowcaseItemEntity.ShowcaseItemID > 0)
			{
				uxSEOData.PageLinkFormatterElements.Clear();
				uxSEOData.PageLinkFormatterElements.Add(Convert.ToString(ShowcaseItemEntity.ShowcaseItemID));
				if (String.IsNullOrEmpty(uxSEOData.Title))
					uxSEOData.Title = uxTitle.Text;
				uxSEOData.SaveControlData();
			}

			if (NewRecord)
			{
				uxEditCollection.NavigateUrl = "~/admin/showcase/admin-media-collection.aspx?itemID=" + ShowcaseItemEntity.ShowcaseItemID;
				uxCollectionPlaceHolder.Visible = true;
				PopulateAttributes();
				m_CollapseFormAfterSave = false;
			}
			else
				SaveAttributes();
			m_ClassTitle = ShowcaseItemEntity.Title;
		}
	}

	protected override void LoadData()
	{
		uxActive.Checked = ShowcaseItemEntity.Active;
		if (uxAgentID.Items.FindByValue(ShowcaseItemEntity.AgentID.ToString()) != null)
			uxAgentID.Items.FindByValue(ShowcaseItemEntity.AgentID.ToString()).Selected = true;
		uxAvailabilityDate.SelectedDate = ShowcaseItemEntity.AvailabilityDateClientTime;
		uxImage.FileName = ShowcaseItemEntity.Image;
		uxMLSID.Text = ShowcaseItemEntity.MlsID.ToString();
		if (uxNeighborhoodID.Items.FindByValue(ShowcaseItemEntity.NeighborhoodID.ToString()) != null)
			uxNeighborhoodID.Items.FindByValue(ShowcaseItemEntity.NeighborhoodID.ToString()).Selected = true;
		if (uxOfficeID.Items.FindByValue(ShowcaseItemEntity.OfficeID.ToString()) != null)
			uxOfficeID.Items.FindByValue(ShowcaseItemEntity.OfficeID.ToString()).Selected = true;
		uxRented.Checked = ShowcaseItemEntity.Rented;
		uxSummary.Text = ShowcaseItemEntity.Summary;
		uxTitle.Text = ShowcaseItemEntity.Title;
		uxFeatured.Checked = ShowcaseItemEntity.Featured;

		uxWebsite.Text = ShowcaseItemEntity.Website;
		uxVirtualTourURL.Text = ShowcaseItemEntity.VirtualTourURL;
		uxDirections.Text = ShowcaseItemEntity.Directions;

		uxEmailAddresses.Text = ShowcaseItemEntity.EmailAddresses;
		if (ShowcaseItemEntity.StatsSentToAgent && ShowcaseItemEntity.StatsSentToOwner)
			uxSendStatsTo.Items.FindByValue("Both").Selected = true;
		else if (ShowcaseItemEntity.StatsSentToAgent)
			uxSendStatsTo.Items.FindByValue("Agent").Selected = true;
		else if (ShowcaseItemEntity.StatsSentToOwner)
			uxSendStatsTo.Items.FindByValue("Owner").Selected = true;

		uxAddress.AddressID = ShowcaseItemEntity.AddressID;
		uxAddress.Load();

		PopulateAttributes();

		uxMLSData.Visible = ShowcaseItemEntity.MlsID.HasValue;

		//Load Rental data
		if (!ShowcaseHelpers.IsCurrentShowcaseMLS())
		{
			ShowcaseItemRental rentalEntity = ShowcaseItemRental.ShowcaseItemRentalGetByShowcaseItemID(EntityId).FirstOrDefault();
			if (rentalEntity == null)
				return;
			uxCompanyName.Text = rentalEntity.CompanyName;
			uxContactName.Text = rentalEntity.ContactName;
			uxContactPhone.Text = rentalEntity.ContactPhone;
			uxLeaseBegins.SelectedDate = rentalEntity.LeaseBeginDateClientTime;
			uxOwnerName.Text = rentalEntity.OwnerName;
		}

		if (ShowcaseItemEntity.NewHome)
		{
			uxSoldHomeControl.ShowcaseItemID = EntityId;
			uxSoldHomeControl.LoadData();
			uxSoldHomeIsSold.Checked = uxSoldHomeControl.SoldHomeEntity != null;
		}

		if (uxOpenHouseAgentID.Items.FindByValue(ShowcaseItemEntity.OpenHouseAgentID.ToString()) != null)
			uxOpenHouseAgentID.Items.FindByValue(ShowcaseItemEntity.OpenHouseAgentID.ToString()).Selected = true;
		BindRecurringOpenHouses();
	}


	protected void Recurring_Command(object sender, CommandEventArgs e)
	{
		if (e.CommandName == "Add")
		{
			uxNumWeeksRFV.Enabled =
			uxDaysOfWeekRFV.Enabled = uxRecurrencePattern.SelectedValue == "Weekly";
			uxNumMonthsRFV.Enabled = !uxNumWeeksRFV.Enabled;
			if (IsValid)
			{
				StringBuilder datesToAdd = new StringBuilder();
				DateTime dateStep = Helpers.ConvertClientTimeToUTC(uxRecurringStartDate.SelectedDate.Value);
				List<OpenHouse> existingRecurringOpenHouses = OpenHouse.OpenHouseGetByShowcaseItemID(EntityId);
				if (uxRecurrencePattern.SelectedValue == "Weekly")
				{
					List<string> selectedDaysOfWeek = new List<string>();
					foreach (ListItem li in uxDaysOfWeek.Items)
					{
						if (li.Selected)
							selectedDaysOfWeek.Add(li.Value);
					}
					while (dateStep <= Helpers.ConvertClientTimeToUTC(uxRecurringEndDate.SelectedDate.Value))
					{
						if (selectedDaysOfWeek.Contains(dateStep.DayOfWeek.ToString()) && !existingRecurringOpenHouses.Exists(ev => ev.BeginDate == dateStep.Date.Add(Helpers.ConvertClientTimeToUTC(uxRecurringStartDate.SelectedDate.Value).TimeOfDay) && (ev.EndDate.HasValue && ev.EndDate.Value == dateStep.Date.Add(Helpers.ConvertClientTimeToUTC(uxRecurringEndDate.SelectedDate.Value).TimeOfDay))))
							datesToAdd.Append(dateStep.ToString("MM-dd-yyyy") + Helpers.ConvertClientTimeToUTC(uxRecurringStartDate.SelectedDate.Value).ToString(" hh:mm:ss tt") + "," + dateStep.ToString("MM-dd-yyyy") + Helpers.ConvertClientTimeToUTC(uxRecurringEndDate.SelectedDate.Value).ToString(" hh:mm:ss tt") + "|");
						dateStep = dateStep.AddDays(1);
						if (dateStep.DayOfWeek == Helpers.ConvertClientTimeToUTC(uxRecurringStartDate.SelectedDate.Value).DayOfWeek)
							dateStep = dateStep.AddDays(7 * (Convert.ToInt32(uxNumWeeks.Text) - 1));
					}
				}
				else
				{
					while (dateStep <= Helpers.ConvertClientTimeToUTC(uxRecurringEndDate.SelectedDate.Value))
					{
						if (uxWeekNumber.SelectedValue != "5")
						{
							dateStep = new DateTime(dateStep.Year, dateStep.Month, 1, dateStep.Hour, dateStep.Minute, dateStep.Second);
							while (dateStep.DayOfWeek.ToString() != uxMonthlyDayOfWeek.SelectedValue && dateStep < Helpers.ConvertClientTimeToUTC(uxRecurringEndDate.SelectedDate.Value))
								dateStep = dateStep.AddDays(1);
							dateStep = dateStep.AddDays(7 * (Convert.ToInt32(uxWeekNumber.SelectedValue) - 1));
						}
						else
						{
							dateStep = new DateTime(dateStep.Year, dateStep.Month + 1, 1, dateStep.Hour, dateStep.Minute, dateStep.Second).AddDays(-1);
							while (dateStep.DayOfWeek.ToString() != uxMonthlyDayOfWeek.SelectedValue && dateStep > Helpers.ConvertClientTimeToUTC(uxRecurringStartDate.SelectedDate.Value))
								dateStep = dateStep.AddDays(-1);
						}
						if (!existingRecurringOpenHouses.Exists(ev => ev.BeginDate == dateStep.Date.Add(Helpers.ConvertClientTimeToUTC(uxRecurringStartDate.SelectedDate.Value).TimeOfDay) && (ev.EndDate.HasValue && ev.EndDate.Value == dateStep.Date.Add(Helpers.ConvertClientTimeToUTC(uxRecurringEndDate.SelectedDate.Value).TimeOfDay))))
							datesToAdd.Append(dateStep.ToString("MM-dd-yyyy") + Helpers.ConvertClientTimeToUTC(uxRecurringStartDate.SelectedDate.Value).ToString(" hh:mm:ss tt") + "," + dateStep.ToString("MM-dd-yyyy") + Helpers.ConvertClientTimeToUTC(uxRecurringEndDate.SelectedDate.Value).ToString(" hh:mm:ss tt") + "|");
						dateStep = dateStep.AddMonths(Convert.ToInt32(uxNumMonths.Text));
					}
				}
				if (!String.IsNullOrEmpty(datesToAdd.ToString()))
					OpenHouse.AddRecurringOpenHouses(datesToAdd.ToString().TrimEnd('|'), EntityId);
				uxRecurrencePattern.ClearSelection();
				uxRecurrencePattern.Items.FindByValue("Weekly").Selected = true;
				uxNumWeeks.Text =
				uxNumMonths.Text = @"1";
				uxDaysOfWeek.ClearSelection();
				uxWeekNumber.ClearSelection();
				uxMonthlyDayOfWeek.ClearSelection();
				uxRecurringStartDate.SelectedDate =
				uxRecurringEndDate.SelectedDate = null;
			}
		}
		else if (e.CommandName == "Delete")
		{
			OpenHouse entity = OpenHouse.GetByID(Convert.ToInt32(e.CommandArgument));
			if (entity != null)
				entity.Delete();
		}
		BindRecurringOpenHouses();
		Helpers.PageView.Anchor(Page, "openHouses");
	}

	void BindRecurringOpenHouses()
	{
		if (!ShowcaseHelpers.IsCurrentShowcaseMLS())
			return;
		List<OpenHouse> recOpenHouses = OpenHouse.OpenHouseGetByShowcaseItemID(EntityId, "BeginDate");
		if (recOpenHouses.Count > 1)
		{
			uxRecurring.Checked = true;
			uxRecurringDates.DataSource = recOpenHouses;
			uxRecurringDates.DataBind();

			uxRecurringDates.Visible = uxRecurringDates.Items.Count > 0;

			uxQuickNavPH.Visible = recOpenHouses.Count > 12;
			if (uxQuickNavPH.Visible)
			{
				Dictionary<string, int> quickNavStrings = new Dictionary<string, int>();
				foreach (OpenHouse ev in recOpenHouses)
				{
					string key = ev.BeginDate.ToString("MMM-yyyy");
					if (!quickNavStrings.ContainsKey(key))
						quickNavStrings.Add(key, ev.OpenHouseID);
				}
				uxQuickNavMonths.DataSource = quickNavStrings;
				uxQuickNavMonths.DataBind();

				uxQuickNavYear.Text = recOpenHouses.First().BeginDate.Year.ToString();
				uxQuickNavYearPH.Visible = recOpenHouses.First().BeginDate.Year != recOpenHouses.Last().BeginDate.Year;
			}
		}
		else
		{
			uxRecurring.Checked = false;
			uxRecurringDates.DataSource = null;
			uxRecurringDates.DataBind();
			if (recOpenHouses.Count == 1)
			{
				uxBeginDate.SelectedDate = recOpenHouses.FirstOrDefault().BeginDateClientTime;
				uxEndDate.SelectedDate = recOpenHouses.FirstOrDefault().EndDateClientTime;
			}
			ClientScript.RegisterStartupScript(GetType(), "ToggleRecurring", "$(document).ready(function(){ToggleRecurring();});", true);
		}
	}

	private void uxAttributeRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		ShowcaseAttribute attributeEntity = (ShowcaseAttribute)e.Item.DataItem;
		List<ShowcaseAttributeValue> valuesForThisAttribute = AllAttributesAssignedToItem.Where(v => v.ShowcaseAttributeID == attributeEntity.ShowcaseAttributeID).ToList();
		if (valuesForThisAttribute.Any())
		{
			if (attributeEntity.Numeric)
			{
				TextBox uxAttributeValueTxt = (TextBox)e.Item.FindControl("uxAttributeValueTxt");
				uxAttributeValueTxt.Text = valuesForThisAttribute.FirstOrDefault().Value;
				uxAttributeValueTxt.ReadOnly = !String.IsNullOrEmpty(attributeEntity.MLSAttributeName) && ShowcaseHelpers.IsCurrentShowcaseMLS();
			}
			else if (attributeEntity.SingleItemValue)
			{
				DropDownList uxSingleAttributeValue = (DropDownList)e.Item.FindControl("uxSingleAttributeValue");
				if (uxSingleAttributeValue.Items.FindByValue(valuesForThisAttribute.FirstOrDefault().ShowcaseAttributeValueID.ToString()) != null)
					uxSingleAttributeValue.Items.FindByValue(valuesForThisAttribute.FirstOrDefault().ShowcaseAttributeValueID.ToString()).Selected = true;
			}
			else
			{
				CheckBoxList uxAttributeValues = (CheckBoxList)e.Item.FindControl("uxAttributeValues");
				foreach (ListItem l in uxAttributeValues.Items)
				{
					l.Selected = valuesForThisAttribute.Any(v => v.ShowcaseAttributeValueID.ToString() == l.Value);
				}
				uxAttributeValues.Enabled = String.IsNullOrEmpty(attributeEntity.MLSAttributeName) || !ShowcaseHelpers.IsCurrentShowcaseMLS();
			}
		}
	}

	protected void uxRadioButtonGrid_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		RadioButtonList uxRadioButtons = (RadioButtonList)e.Item.FindControl("uxRadioButtons");
		HiddenField uxValueID = (HiddenField)e.Item.FindControl("uxValueID");
		if (uxRadioButtons.Items.Count > 0)
		{
			uxRadioButtons.Items[0].Selected =
				AllAttributesAssignedToItem.Exists(v => v.ShowcaseAttributeValueID.ToString() == uxValueID.Value);
			uxRadioButtons.Items[1].Selected = !uxRadioButtons.Items[0].Selected;
		}
	}

	private void PopulateAttributes()
	{
		List<ShowcaseAttribute> allAttributes = ShowcaseAttribute.ShowcaseAttributePage(0, 0, "", "DisplayOrder", true, new ShowcaseAttribute.Filters { FilterShowcaseAttributeShowcaseID = ShowcaseHelpers.GetCurrentShowcaseID().ToString(), FilterShowcaseAttributeImportItemAttribute = true.ToString() }).Where(s => s.ShowcaseFilterID != (int)FilterTypes.Distance && s.ShowcaseFilterID != (int)FilterTypes.DistanceRange && (s.Numeric || ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(s.ShowcaseAttributeID).Any())).ToList();
		uxAttributeRepeater.DataSource = allAttributes;
		uxAttributeRepeater.DataBind();
		uxAttributesPlaceHolder.Visible = allAttributes.Count > 0;
	}

	private void SaveAttributes()
	{
		foreach (RepeaterItem rI in uxAttributeRepeater.Items)
		{
			ShowcaseAttribute attributeEntity = ShowcaseAttribute.GetByID(Convert.ToInt32(((HiddenField)rI.FindControl("uxAttributeID")).Value));
			if (!String.IsNullOrEmpty(attributeEntity.MLSAttributeName) && ShowcaseHelpers.IsCurrentShowcaseMLS())
				continue;
			if (attributeEntity.Numeric)
			{
				TextBox uxAttributeValueTxt = (TextBox)rI.FindControl("uxAttributeValueTxt");
				if (attributeEntity.MLSAttributeName == "Rental Price" && !String.IsNullOrEmpty(uxAttributeValueTxt.Text))
				{
					ShowcaseItemEntity.ListPrice = Convert.ToDecimal(uxAttributeValueTxt.Text);
					ShowcaseItemEntity.Save();
				}
				ShowcaseAttributeValue valueEntity = AllAttributesAssignedToItem.Find(v => v.ShowcaseAttributeID == attributeEntity.ShowcaseAttributeID);
				if (valueEntity == null && !String.IsNullOrEmpty(uxAttributeValueTxt.Text))
				{
					//Check if this value already exists for this attribute...don't readd it if it does
					valueEntity = ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(attributeEntity.ShowcaseAttributeID).Find(v => v.Value == uxAttributeValueTxt.Text);
					if (valueEntity == null)
					{
						valueEntity = new ShowcaseAttributeValue();
						valueEntity.ShowcaseAttributeID = attributeEntity.ShowcaseAttributeID;
						valueEntity.Value = uxAttributeValueTxt.Text;
						valueEntity.Save();
					}
					ShowcaseItemAttributeValue newItemValue = new ShowcaseItemAttributeValue();
					newItemValue.ShowcaseAttributeValueID = valueEntity.ShowcaseAttributeValueID;
					newItemValue.ShowcaseItemID = ShowcaseItemEntity.ShowcaseItemID;
					newItemValue.Save();
				}
				else if (valueEntity != null && valueEntity.Value != uxAttributeValueTxt.Text)
				{
					//Check for other items using the old value before changing it
					List<ShowcaseItemAttributeValue> otherItemsWithValue = ShowcaseItemAttributeValue.ShowcaseItemAttributeValueGetByShowcaseAttributeValueID(valueEntity.ShowcaseAttributeValueID);
					ShowcaseItemAttributeValue oldItemReference = otherItemsWithValue.Find(i => i.ShowcaseItemID == ShowcaseItemEntity.ShowcaseItemID);
					if (otherItemsWithValue.Exists(i => i.ShowcaseItemID != ShowcaseItemEntity.ShowcaseItemID))
					{
						if (!String.IsNullOrEmpty(uxAttributeValueTxt.Text))
						{
							//Before creating a new value, check and see if one exists already
							ShowcaseAttributeValue newValue = ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(attributeEntity.ShowcaseAttributeID).Find(v => v.Value == uxAttributeValueTxt.Text);
							if (newValue == null)
							{
								//Create a new Value
								newValue = new ShowcaseAttributeValue();
								newValue.Value = uxAttributeValueTxt.Text;
								newValue.ShowcaseAttributeID = attributeEntity.ShowcaseAttributeID;
								newValue.Save();
							}
							//else
								//No need for old value anymore, use already existing new value
							//	valueEntity.Delete();

							//Update reference in join table
							oldItemReference.ShowcaseAttributeValueID = newValue.ShowcaseAttributeValueID;
							oldItemReference.Save();
						}
						//else
							//Value has been removed, delete the reference in the join table
						//	oldItemReference.Delete();
					}
					else
					{
						if (!String.IsNullOrEmpty(uxAttributeValueTxt.Text))
						{
							//Before creating a new value, check and see if one exists already
							ShowcaseAttributeValue newValue = ShowcaseAttributeValue.ShowcaseAttributeValueGetByShowcaseAttributeID(attributeEntity.ShowcaseAttributeID).Find(v => v.Value == uxAttributeValueTxt.Text);
							if (newValue == null)
							{
								//Change the value
								valueEntity.Value = uxAttributeValueTxt.Text;
								valueEntity.Save();
							}
							else
							{
								//No need for old value entity, update join table reference
								oldItemReference.ShowcaseAttributeValueID = newValue.ShowcaseAttributeValueID;
								oldItemReference.Save();
								//Delete old value
								//valueEntity.Delete();
							}
						}
						//else
							//No need for value anymore...delete it
						//	valueEntity.Delete();
					}
				}
			}
			else
			{
				if (attributeEntity.ShowcaseFilterID != (int)FilterTypes.RadioButtonGrid)
				{
					if (attributeEntity.SingleItemValue)
					{
						DropDownList uxSingleAttributeValue = (DropDownList)rI.FindControl("uxSingleAttributeValue");
						int? showcaseAttributeValueID = !String.IsNullOrWhiteSpace(uxSingleAttributeValue.SelectedValue) ? (int?)Convert.ToInt32(uxSingleAttributeValue.SelectedValue) : null;
						List<ShowcaseItemAttributeValue> referenceTable = ShowcaseItemAttributeValue.GetItemValuesByAttributeAndShowcaseItemID(ShowcaseItemEntity.ShowcaseItemID, attributeEntity.ShowcaseAttributeID);
						if (showcaseAttributeValueID.HasValue)
						{
							if (!referenceTable.Any(r=>r.ShowcaseAttributeValueID == showcaseAttributeValueID.Value))
							{
								if (referenceTable.Any())
								{
									referenceTable.FirstOrDefault().ShowcaseAttributeValueID = showcaseAttributeValueID.Value;
									referenceTable.FirstOrDefault().Save();
								}
								else
								{
									ShowcaseItemAttributeValue newItemValue = new ShowcaseItemAttributeValue();
									newItemValue.ShowcaseItemID = ShowcaseItemEntity.ShowcaseItemID;
									newItemValue.ShowcaseAttributeValueID = showcaseAttributeValueID.Value;
									newItemValue.Save();
								}
							}
						}
						referenceTable.ForEach(r => { if (r.ShowcaseAttributeValueID != showcaseAttributeValueID) r.Delete(); });
					}
					else
					{
						CheckBoxList uxAttributeValues = (CheckBoxList)rI.FindControl("uxAttributeValues");
						foreach (ListItem l in uxAttributeValues.Items)
						{
							List<ShowcaseItemAttributeValue> referenceTable = ShowcaseItemAttributeValue.ShowcaseItemAttributeValuePage(0, 0, "", "", true, new ShowcaseItemAttributeValue.Filters { FilterShowcaseItemAttributeValueShowcaseAttributeValueID = l.Value, FilterShowcaseItemAttributeValueShowcaseItemID = ShowcaseItemEntity.ShowcaseItemID.ToString() });
							if (l.Selected)
							{
								//If the join table reference doesn't exist, create it
								if (referenceTable.Count == 0)
								{
									ShowcaseItemAttributeValue newItemValue = new ShowcaseItemAttributeValue();
									newItemValue.ShowcaseItemID = ShowcaseItemEntity.ShowcaseItemID;
									newItemValue.ShowcaseAttributeValueID = Convert.ToInt32(l.Value);
									newItemValue.Save();
								}
							}
							else if (referenceTable.Count == 1)
								//If the join table reference exists, delete it
								referenceTable[0].Delete();
						}
					}
				}
				else
				{
					Repeater uxRadioButtonGrid = (Repeater)rI.FindControl("uxRadioButtonGrid");
					foreach (RepeaterItem gridItem in uxRadioButtonGrid.Items)
					{
						HiddenField uxValueID = (HiddenField)gridItem.FindControl("uxValueID");
						RadioButtonList uxRadioButtons = (RadioButtonList)gridItem.FindControl("uxRadioButtons");
						if (uxRadioButtons.Items.Count > 0)
						{
							List<ShowcaseItemAttributeValue> referenceTable = ShowcaseItemAttributeValue.ShowcaseItemAttributeValuePage(0, 0, "", "", true, new ShowcaseItemAttributeValue.Filters { FilterShowcaseItemAttributeValueShowcaseAttributeValueID = uxValueID.Value, FilterShowcaseItemAttributeValueShowcaseItemID = ShowcaseItemEntity.ShowcaseItemID.ToString() });
							if (referenceTable.Count == 0 && uxRadioButtons.Items[0].Selected)
							{
								ShowcaseItemAttributeValue newItemValue = new ShowcaseItemAttributeValue();
								newItemValue.ShowcaseItemID = ShowcaseItemEntity.ShowcaseItemID;
								newItemValue.ShowcaseAttributeValueID = Convert.ToInt32(uxValueID.Value);
								newItemValue.Save();
							}
							else if (referenceTable.Count > 0 && !uxRadioButtons.Items[0].Selected)
								referenceTable.ForEach(a => a.Delete());
						}
					}
				}
			}
		}
		Helpers.PurgeCacheItems("Showcase_ShowcaseAttribute_GetAttributesAndValuesByShowcaseItemID_" + EntityId);
		Helpers.PurgeCacheItems("Showcase_ShowcaseAttributeValue_GetAllAttributeValuesByShowcaseItemID_" + EntityId);
	}

	void uxOwnerNameCV_ServerValidate(object source, ServerValidateEventArgs args)
	{
		args.IsValid = !String.IsNullOrWhiteSpace(uxOwnerName.Text) || !String.IsNullOrWhiteSpace(uxCompanyName.Text);
	}

	public void DateValidate(object source, ServerValidateEventArgs args)
	{
		if (uxBeginDate.SelectedDate > uxEndDate.SelectedDate)
			args.IsValid = false;
	}

	public void DateValidateRecurring(object source, ServerValidateEventArgs args)
	{
		if (uxRecurringStartDate.SelectedDate > uxRecurringEndDate.SelectedDate)
			args.IsValid = false;
	}
}