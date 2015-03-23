<%@ Page Language="C#" ContentType="text/plain" ValidateRequest="false" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="Moxiecode.Manager" %>
<%@ Import Namespace="Moxiecode.Manager.Utils" %>
<%
	if (Session["old_imagemanager.filesystem.rootpath"] == null)
		Session["old_imagemanager.filesystem.rootpath"] = Session["imagemanager.filesystem.rootpath"];
	Hashtable args = new Hashtable();
	string id, method, prefix = "im";
	ManagerEngine man = null;

	// Use install if it exists
	if (Directory.Exists(Server.MapPath("../install")))
	{
		Response.Write("{\"result\":null,\"id\":null,\"error\":{\"errstr\":\"You need to run the installer or rename/remove the \"install\" directory.\",\"errfile\":\"\",\"errline\":null,\"errcontext\":\"\",\"level\":\"FATAL\"}}");
		Response.End();
	}

	// Parse JSON
	try
	{
		JSONRpcCall call;

		if (Request["json_data"] != null)
			call = JSON.ParseRPC(new System.IO.StringReader(Request["json_data"]));
		else
			call = JSON.ParseRPC(new System.IO.StreamReader(Request.InputStream));

		// Get JSON items
		id = call.Id;
		method = call.Method;
		args = call.Args;
		if (method.Equals("im.getMediaInfo") && args["path"] != null)
		{
			string path = args["path"].ToString().Replace("../", "");
			string folderPath = path.Replace("{0}", "");
			folderPath = folderPath.Substring(0, folderPath.LastIndexOf('/') + 1);
			if (!String.IsNullOrEmpty(folderPath))
			{
				Session["imagemanager.rootpath"] =
				Session["imagemanager.filesystem.rootpath"] = "~/" + folderPath;

				args["path"] = path.Replace(folderPath, "");
			}
			if (args["path"].ToString() == "{0}" && args["url"] != null)
			{
				args["path"] = args["path"] + args["url"].ToString().Split('/')[args["url"].ToString().Split('/').Length - 1];
				args.Remove("url");
			}
		}

		// Parse prefix
		if (method != null)
		{
			Match match = Regex.Match(method, @"^([a-z]+)\.(.*)");
			prefix = match.Groups[1].Value;
			method = match.Groups[2].Value;
		}

		if (call.Id != null)
		{
			// Setup file manager
			man = new ManagerEngine(prefix);

			if (man.IsAuthenticated)
			{
				// Initialize
				man.DispatchEvent(EventType.Init);
				man.DispatchEvent(EventType.BeforeRPC, method, args);

				// Call RPC
				Moxiecode.Manager.Utils.JSON.SerializeRPC(
					id,
					null,
					man.ExecuteEvent(EventType.RPC, method, args),
					Response.OutputStream
				);
			}
			else
				Response.Write("{\"result\":{\"login_url\":\"" + man.Config["authenticator.login_page"] + "\"},\"id\":null,\"error\":{\"errstr\":\"Access denied by authenicator.\",\"errfile\":\"\",\"errline\":null,\"errcontext\":\"\",\"level\":\"AUTH\"}}");
		}
		if (method.Equals("insertFiles") && Session["old_imagemanager.filesystem.rootpath"] != null)
		{
			Session["imagemanager.rootpath"] =
			Session["imagemanager.filesystem.rootpath"] = Session["old_imagemanager.filesystem.rootpath"];
			Session["old_imagemanager.filesystem.rootpath"] = null;
		}
	}
	catch (ManagerException ex)
	{
		Response.Write("{\"result\":null,\"id\":null,\"error\":{\"errstr\":\"" + StringUtils.Escape(ex.Message) + "\",\"errfile\":\"\",\"errline\":null,\"errcontext\":\"\",\"level\":\"" + ex.LevelName + "\"}}");

		// Log error
		man = new ManagerEngine(prefix);
		man.Logger.Error(ex.ToString());
	}
	catch (Exception ex)
	{
		Response.Write("{\"result\":null,\"id\":null,\"error\":{\"errstr\":\"" + StringUtils.Escape(ex.Message) + "\",\"errfile\":\"\",\"errline\":null,\"errcontext\":\"\",\"level\":\"FATAL\"}}");

		// Log error
		man = new ManagerEngine(prefix);
		man.Logger.Error(ex.ToString());
	}	
%>