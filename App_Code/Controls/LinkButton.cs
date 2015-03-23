using System;
using System.Web.UI.WebControls;

namespace Controls
{
	public class LinkButtonDefault : LinkButton
	{
		private const string _addClickScript = "addClickFunction('{0}');";

		private const string _addClickFunctionScript =
			@"  function addClickFunction(id) {{
            var b = document.getElementById(id);
            if (b && typeof(b.click) == 'undefined') b.click = function() {{
                var result = true; if (b.onclick) result = b.onclick();
                if (typeof(result) == 'undefined' || result) {{ eval(b.getAttribute('href')); }}
            }}}};";

		protected override void OnLoad(EventArgs e)
		{
			Page.ClientScript.RegisterStartupScript(GetType(), "addClickFunctionScript",
			                                        _addClickFunctionScript, true);

			string script = String.Format(_addClickScript, ClientID);
			Page.ClientScript.RegisterStartupScript(GetType(), "click_" + ClientID,
			                                        script, true);
			base.OnLoad(e);
		}
	}
}