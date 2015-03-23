using System.Collections.Generic;

namespace Classes.StateAndCountry
{
	public partial class State
	{
		/// <summary>
		/// Sets all ShipTo values to the shipTo parameter for either Country or State, determined by the toggleState parameter
		/// </summary>
		/// <param name="toggleState">If true, toggle state, if false toggle country</param>
		/// <param name="shipTo">Set all to this value</param>
		public static void ToggleShipTo(bool toggleState, bool shipTo)
		{
			using (Entities entity = new Entities())
			{
				entity.StateAndCountry_ToggleShipTo(toggleState, shipTo);
			}
			if (toggleState)
				ClearCache();
			else
				BaseCode.Helpers.PurgeCacheItems("StateAndCountry_Country_");
		}

		public static List<State> GetStatesByCountryIDWithShipTo(int countryID, bool useShipTo)
		{
			if (useShipTo)
			{
				List<State> stateList = StateGetByCountryID(countryID, "Name");
				foreach (State s in stateList)
				{
					if (!s.ShipTo)
						s.Name = s.Name + " (cannot be shipped to)";
				}
				return stateList;
			}
			return StateGetByCountryID(countryID, "Name");
		}
	}
}