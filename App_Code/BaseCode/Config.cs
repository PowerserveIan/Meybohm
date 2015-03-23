using System;
using System.Reflection;
using BaseCode;
using BaseCode.Configuration;

public class Config
{
	public enum SiteStatusOptions
	{
		Live,
		Preview,
		Local,
		Unset
	}

	public static SiteStatusOptions SiteStatus
	{
		get
		{
			bool found = false;
			foreach (FieldInfo fi in typeof(SiteStatusOptions).GetFields())
			{
				if (fi.Name == Globals.Settings.SiteStatus)
					return (SiteStatusOptions)fi.GetValue(null);
			}
			if (!found)
				throw new Exception("Unrecognized Production Mode");
			return SiteStatusOptions.Unset;
		}
	}

	public static SiteWideSettings Settings
	{
		get { return Globals.Settings; }
	}
}