using System;
using System.IO;
using System.Net.Mail;
using BaseCode;
using Classes.Rets;
using Elmah;

public partial class RETSConnector_track_rets_success : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		int retsTaskID = 0;
		if (Request.QueryString["taskID"] != null)
		{
			retsTaskID = Convert.ToInt32(Request.QueryString["taskID"]);
		}
		int taskSuccessID = 1;
		if (Request.QueryString["success"] != null)
		{
			taskSuccessID = Convert.ToInt32(Request.QueryString["success"]);
		}
		DateTime taskCompleteTime = DateTime.UtcNow;
		if (retsTaskID > 0)
		{
			RetsTaskStatus retsTaskStatusEntity = new RetsTaskStatus { RetsStatusID = taskSuccessID, RetsTaskID = retsTaskID, TaskCompleteTime = taskCompleteTime };
			retsTaskStatusEntity.Save();
		}
		if(taskSuccessID>1)
		{

			MailMessage email = new MailMessage();
			email.From = new MailAddress(Globals.Settings.FromEmail);
			
			
			email.To.Add(Settings.RetsFailEmail1);
			if(!string.IsNullOrEmpty(Settings.RetsFailEmail2))
				email.To.Add(Settings.RetsFailEmail2);
			if(!string.IsNullOrEmpty(Settings.RetsFailEmail3))
				email.To.Add(Settings.RetsFailEmail2);
			email.IsBodyHtml = true;
			email.Body = "There has been an error with the " +RetsTask.GetByID(retsTaskID).TaskName + " at "+taskCompleteTime.ToString();
			email.Subject = "There has been an error from Rets Connector";
			SmtpClient smtp = new SmtpClient();
			smtp.Send(email);

		}
	}
}