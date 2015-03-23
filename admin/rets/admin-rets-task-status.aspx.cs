using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Xml.Linq;
using Classes.Rets;

public partial class AdminRetsTaskStatus : BaseListingPage
{
	protected static string TaskID = null;
	protected override void OnInit(EventArgs e)
	{
	TaskID = Request.QueryString["id"];
		//Passing control references to base class
		m_Header = uxHeader;
		m_DefaultSortField = "TaskCompleteTime";
		m_DefaultSortDirection = false;
		m_LinkToEditPage = "admin-rets-task-status-edit.aspx?id=";
		m_FiltersPlaceHolder = uxFilterPlaceHolder;
		m_ClassName = "Rets Task Status";
		base.OnInit(e);
		m_AddButton.Visible = false;
		m_BreadCrumbTitle.Text = m_HeaderTitle.Text = m_HeaderTitle.Text.Replace(" Manager", "");
	}

	[WebMethod]
	public static ListingItemWithCount<CUSTOM_ELMAH_GetRETSErrors_New_Result> PageListing(int pageNumber, int pageSize, string searchText, string sortField, bool sortDirection)
	{
		RetsTaskStatus.Filters filterList = new RetsTaskStatus.Filters();
		List<CUSTOM_ELMAH_GetRETSErrors_New_Result> listItems = null;
		int totalCount = 0;
		using (Entities entity = new Entities())
		{
			listItems = new List<CUSTOM_ELMAH_GetRETSErrors_New_Result>(entity.CUSTOM_ELMAH_GetRETSErrors_New()).OrderByDescending(c=>c.ErrorTimeUtc).Skip((pageNumber-1) * pageSize).Take(pageSize).ToList();
		totalCount = entity.CUSTOM_ELMAH_GetRETSErrors_New().Count();
		}
		foreach (CUSTOM_ELMAH_GetRETSErrors_New_Result item in listItems)
		{
			XElement xmlTree = XElement.Parse(item.ErrorData);
		string detail = xmlTree.Attribute("detail").ToString();
		string[]  parts =  detail.Split(new string[]{"---&gt"},StringSplitOptions.None)[0].Split('|');
		Dictionary<string,string> bits = new Dictionary<string, string>();
		for(int n =2 ; n<parts.Count();n++)
		{
			bits.Add(parts[n].Split(':')[0].Trim(), parts[n].Split(':')[1].Trim());
		}
			string temp = null;
			item.Method		= parts[1];
			bits.TryGetValue("Mls ID", out temp);
			item.MlsID		=  temp??"Unknown";
			bits.TryGetValue("City", out temp);
			item.City = temp ?? "";
			temp = null;
			bits.TryGetValue("Showcase", out temp);
			item.Showcase = temp??"Unknown";
			temp = null;
			bits.TryGetValue("step of method", out temp);
			item.Step = temp ?? "";
			item.ErrorTimeUtc = BaseCode.Helpers.ConvertUTCToClientTime(item.ErrorTimeUtc);
		}
		return new ListingItemWithCount<CUSTOM_ELMAH_GetRETSErrors_New_Result> { Items = listItems, TotalCount = totalCount };

	}

	//[WebMethod]
	//public static void DeleteRecord(int id)
	//{
	//	RetsTaskStatus entity = RetsTaskStatus.GetByID(id);
	//	if (entity != null)
	//		entity.Delete();
	//}
}
//public partial class CUSTOM_ELMAH_GetRETSErrors_Result
//{
//	public string Method { get; set; }
//	public string Step { get; set; }
//	public string MlsID { get; set; }
//	public string City { get; set; }
//}
