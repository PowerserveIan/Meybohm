using System.Collections.Generic;
using System.Linq;

namespace Classes.ContentManager
{
	public partial class SMItem
	{
		/// <summary>
		/// Only populated when using a Menu plugin, DO NOT use this as a shortcut
		/// </summary>
		public string LinkToPage { get; set; }
	}

	public class MenuPlugin
	{
		public static List<SMItem> GetAdditionalSMItems(List<SMItem> originalSMItems)
		{
			return new List<SMItem>();
		}
	}
}