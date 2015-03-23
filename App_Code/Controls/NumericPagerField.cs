using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Controls352
{
	public class NumericPagerFieldCustom : NumericPagerField
	{
		private string m_MicrositedUrl;
		protected string MicrositedUrl
		{
			get
			{
				if (string.IsNullOrWhiteSpace(m_MicrositedUrl))
					m_MicrositedUrl = base.GetQueryStringNavigateUrl(1337).Replace("=1337", "={0}").Replace(HttpContext.Current.Request.Path, HttpContext.Current.Request.RawUrl.Split('?')[0]);
				return m_MicrositedUrl;
			}
		}

		public override void CreateDataPagers(DataPagerFieldItem container, int startRowIndex, int maximumRows, int totalRowCount, int fieldIndex)
		{
			HtmlGenericControl ul = new HtmlGenericControl("ul");
			ul.Attributes["class"] = "clearfix";
			int num = startRowIndex / maximumRows;
			int num2 = (startRowIndex / (this.ButtonCount * maximumRows)) * this.ButtonCount;
			int num3 = (num2 + this.ButtonCount) - 1;
			int num4 = ((num3 + 1) * maximumRows) - 1;
			for (int i = 0; (i < this.ButtonCount) && (totalRowCount > ((i + num2) * maximumRows)); i++)
			{
				HtmlGenericControl li = new HtmlGenericControl("li");
				if ((i + num2) == num)
				{
					Label child = new Label();
					child.Text = ((i + num2) + 1).ToString(CultureInfo.InvariantCulture);
					if (!string.IsNullOrEmpty(this.CurrentPageLabelCssClass))
					{
						child.CssClass = this.CurrentPageLabelCssClass;
					}
					li.Controls.Add(child);
				}
				else
				{
					int num7 = (i + num2) + 1;
					int num8 = i + num2;
					li.Controls.Add(string.IsNullOrEmpty(base.DataPager.QueryStringField) ? CreateNumericButton(num7.ToString(CultureInfo.InvariantCulture), fieldIndex.ToString(CultureInfo.InvariantCulture), num8.ToString(CultureInfo.InvariantCulture)) : CreateNumericLink(num8));
				}
				ul.Controls.Add(li);
			}
			container.Controls.Add(ul);
		}

		private Control CreateNumericButton(string buttonText, string commandArgument, string commandName)
		{
			IButtonControl control;
			switch (this.ButtonType)
			{
				case ButtonType.Button:
					control = new Button();
					break;

				default:
					control = new LinkButton();
					break;
			}
			control.Text = buttonText;
			control.CausesValidation = false;
			control.CommandName = commandName;
			control.CommandArgument = commandArgument;
			WebControl control2 = control as WebControl;
			if ((control2 != null) && !string.IsNullOrEmpty(this.NumericButtonCssClass))
			{
				control2.CssClass = this.NumericButtonCssClass;
			}
			return (control as Control);
		}

		private HyperLink CreateNumericLink(int pageIndex)
		{
			int pageNumber = pageIndex + 1;
			HyperLink link = new HyperLink
			{
				Text = pageNumber.ToString(CultureInfo.InvariantCulture),
				NavigateUrl = string.Format(MicrositedUrl, pageNumber)
			};
			if (!string.IsNullOrEmpty(this.NumericButtonCssClass))
			{
				link.CssClass = this.NumericButtonCssClass;
			}
			return link;
		}
	}
}