using System.Web.UI;
using System.Web.UI.WebControls;

namespace TFT.WebControls
{
	public class CompareValidator : System.Web.UI.WebControls.CompareValidator
	{
		private bool m_OverwriteDisplayType;
		public ValidatorDisplay Display
		{
			get { return base.Display; }
			set { m_OverwriteDisplayType = true; base.Display = value; }
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!CssClass.Contains("validator"))
				CssClass += " validator";
			if (!CssClass.Contains("compareValidator"))
				CssClass += " compareValidator";
			CssClass = CssClass.Trim();
			if (!m_OverwriteDisplayType)
				base.Display = ValidatorDisplay.Dynamic;
			base.Render(writer);
		}
	}

	public class CustomValidator : System.Web.UI.WebControls.CustomValidator
	{
		private bool m_OverwriteDisplayType;
		public ValidatorDisplay Display
		{
			get { return base.Display; }
			set { m_OverwriteDisplayType = true; base.Display = value; }
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!CssClass.Contains("validator"))
				CssClass += " validator";
			if (!CssClass.Contains("customValidator"))
				CssClass += " customValidator";
			CssClass = CssClass.Trim();
			if (!m_OverwriteDisplayType)
				base.Display = ValidatorDisplay.Dynamic;
			base.Render(writer);
		}
	}

	public class RangeValidator : System.Web.UI.WebControls.RangeValidator
	{
		private bool m_OverwriteDisplayType;
		public ValidatorDisplay Display
		{
			get { return base.Display; }
			set { m_OverwriteDisplayType = true; base.Display = value; }
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!CssClass.Contains("validator"))
				CssClass += " validator";
			if (!CssClass.Contains("rangeValidator"))
				CssClass += " rangeValidator";
			CssClass = CssClass.Trim();
			if (!m_OverwriteDisplayType)
				base.Display = ValidatorDisplay.Dynamic;
			base.Render(writer);
		}
	}

	public class RegularExpressionValidator : System.Web.UI.WebControls.RegularExpressionValidator
	{
		private bool m_OverwriteDisplayType;
		public ValidatorDisplay Display
		{
			get { return base.Display; }
			set { m_OverwriteDisplayType = true; base.Display = value; }
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!CssClass.Contains("validator"))
				CssClass += " validator";
			if (!CssClass.Contains("regexValidator"))
				CssClass += " regexValidator";
			CssClass = CssClass.Trim();
			if (!m_OverwriteDisplayType)
				base.Display = ValidatorDisplay.Dynamic;
			base.Render(writer);
		}
	}

	public class RequiredFieldValidator : System.Web.UI.WebControls.RequiredFieldValidator
	{
		private bool m_OverwriteDisplayType;
		public ValidatorDisplay Display
		{
			get { return base.Display; }
			set { m_OverwriteDisplayType = true; base.Display = value; }
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!CssClass.Contains("validator"))
				CssClass += " validator";
			if (!CssClass.Contains("requiredValidator"))
				CssClass += " requiredValidator";
			CssClass = CssClass.Trim();
			if (!m_OverwriteDisplayType)
				base.Display = ValidatorDisplay.Dynamic;
			base.Render(writer);
		}
	}	
}