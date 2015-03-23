using System;
using System.IO;
using System.Web.UI;

public partial class SendingEmails : Page
{
	protected bool m_IsSending;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);

		if (Request.QueryString["action"] == "updateprogress")
		{
			SetIsSending();
			if (m_IsSending)
			{
				BindJobs();
				StringWriter tw = new StringWriter();
				HtmlTextWriter writer = new HtmlTextWriter(tw);

				rpJobs.RenderControl(writer);
				Response.Write(tw.ToString());
			}
			else
				Response.Write("sent");
			Response.End();
		}
		else
		{
			if (Request.QueryString["action"] == "updatesent")
			{
				BindJobs();
				EmailSender.SendEmailJobs = null;
				StringWriter tw = new StringWriter();
				HtmlTextWriter writer = new HtmlTextWriter(tw);

				rpJobs.RenderControl(writer);
				Response.Write(tw.ToString());
				Response.End();
			}
		}
	}

	private void SetIsSending()
	{
		foreach (EmailSender es in EmailSender.SendEmailJobs)
		{
			if (es.IsSending)
				m_IsSending = true;
			break;
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		SetIsSending();
		BindJobs();
	}

	private void BindJobs()
	{
		rpJobs.DataSource = EmailSender.SendEmailJobs;
		rpJobs.DataBind();
	}
}