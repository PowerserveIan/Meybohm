<%@ WebService Language="C#" Class="StateAndCountryWebMethods" %>

using System.Collections.Generic;
using System.Web.Script.Services;
using System.Web.Services;
using Classes.StateAndCountry;

[WebService(Namespace = "http://352media.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class StateAndCountryWebMethods : WebService
{
	[WebMethod]
	public List<State> GetStatesByCountryID(int countryID, bool useShipTo)
	{
		return State.GetStatesByCountryIDWithShipTo(countryID, useShipTo);
	}
}