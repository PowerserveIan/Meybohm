using System;
using System.Collections.Generic;
using System.Linq;
using Classes.MLS;
using Powerserve.Meybohm.Model;

public partial class offices : BasePage
{
	protected string m_OfficesJS;

	public override void SetComponentInformation()
	{
		ComponentName = "Office";
		ComponentAdminPage = "m-l-s/admin-office.aspx";
	}

	protected override void SetCssAndJs()
	{
		m_AdditionalJavaScriptFiles = uxJavaScripts;
	}

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		Classes.ContentManager.CMMicrosite currentMicrosite = Classes.ContentManager.CMSHelpers.GetCurrentRequestCMSMicrosite();
		if (currentMicrosite != null)
			MasterPageFile = "~/microsite.master";
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			Office.Filters filterList = new Office.Filters();
			filterList.FilterOfficeActive = true.ToString();
			filterList.FilterOfficeIsMeybohm = true.ToString();

            List<OfficeInfo> offices = OfficeInfo.GetList();
			uxOffices.DataSource = offices;
			uxOffices.DataBind();

			m_OfficesJS = string.Empty;
			foreach (OfficeInfo o in offices)
			{
				m_OfficesJS += "new markerItem(" + o.OfficeLatitude + ", " + o.OfficeLongitude + ", \"" + GetMarkerContent(o) + "\"),";
			}
			m_OfficesJS = m_OfficesJS.TrimEnd(',');
		}
	}

	private string GetMarkerContent(OfficeInfo o)
	{
		return string.Format("{0}<br /> {1}<br /> {2}, {3}, {4}<br /> Phone: {5}<br /> Fax: {6}", o.OfficeName, o.OfficeAddress, o.OfficeCity, o.OfficeState, o.OfficeZip, o.OfficePhone, o.OfficeFax);
	}
}