using System;

namespace BaseCode
{
	/// <summary>
	/// Thin wrapper for enum parsing
	/// </summary>
	public static class EnumParser
	{
		/// <summary>
		/// This will search through enum values and try to find a match
		/// </summary>
		/// <typeparam name="tEnum">the type of the enum</typeparam>
		/// <param name="theVal">value you want to find</param>
		/// <returns>if found, an enum in the type you specify</returns>
		public static tEnum Parse<tEnum>(
			string theVal) where tEnum : struct
		{
			return (tEnum)Enum.Parse(
				typeof (tEnum), theVal);
		}

		/// <summary>
		/// This will search through enum values and try to find a match; if none is found the default you provide will be returned
		/// </summary>
		/// <typeparam name="tEnum">the type of the enum</typeparam>
		/// <param name="theVal">value you want to find</param>
		/// <param name="theDef">the default value if no match is found</param>
		/// <returns>if found, an enum in the type you specify or the default you passed in</returns>
		public static tEnum Parse<tEnum>(
			string theVal, tEnum theDef) where tEnum : struct
		{
			try
			{
				return Parse<tEnum>(theVal);
			}
			catch (ArgumentException)
			{
				return theDef;
			}
		}
	}
}