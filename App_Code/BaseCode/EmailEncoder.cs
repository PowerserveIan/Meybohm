using System;
using System.Text;

namespace BaseCode
{
	/// <summary>
	/// Written By Charles Cook
	/// 
	/// Takes an email address and obscures it to hide it from crawlers.
	/// </summary>
	public class EmailEncoder
	{
		public static string Encode(String address)
		{
			StringBuilder encoded = new StringBuilder();

			foreach (byte b in Encoding.ASCII.GetBytes(address))
			{
				encoded.Append("&#");
				encoded.Append(Convert.ToString(b));
				encoded.Append(";");
			}

			return Convert.ToString(encoded);
		}
	}
}