using System;
using System.Web;
using System.Web.UI;
using BaseCode;

public partial class Controls_BaseControls_RichTextEditor : System.Web.UI.UserControl
{
	private int m_NumberOfFoldersToRoot = 2;

	public string FieldName { get; set; }

	public bool HideEditorInitially { get; set; }

	public int? MaxLength { get; set; }

	public int NumberOfFoldersToRoot { get { return m_NumberOfFoldersToRoot; } set { m_NumberOfFoldersToRoot = value; } }

	public bool Required { get; set; }

	public string EditorHTML
	{
		get
		{
			string returnText = Helpers.ReplaceAbsolutePathWithRoot(HttpUtility.HtmlDecode(uxEditorTextBox.Text));
			uxEditorText.Text = uxEditorTextBox.Text = Helpers.ReplaceRootWithRelativePath(returnText, NumberOfFoldersToRoot);
			return returnText;
		}
		set
		{
			uxEditorText.Text = uxEditorTextBox.Text = Helpers.ReplaceRootWithRelativePath(value, NumberOfFoldersToRoot);
		}
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);		
		Helpers.GetJSCode(uxJavaScripts);
		if (HideEditorInitially)
			uxEditorTextBox.CssClass += " hidden";

		uxEditorText.Visible =
		uxToggleEditor.Visible = HideEditorInitially;

		uxEditorTextBoxRFV.ErrorMessage = FieldName + " is required.";
		uxEditorTextBoxRFV.Visible = uxEditorTextBoxRFV.Enabled = Required;
		uxEditorTextBoxREV.Visible = uxEditorTextBoxREV.Enabled = MaxLength.HasValue;
		if (MaxLength.HasValue)
		{
			uxEditorTextBoxREV.ValidationExpression = @"^[\s\S]{0," + MaxLength + "}$";
			uxEditorTextBoxREV.ErrorMessage = FieldName + " is too long.  It must be " + MaxLength + " characters or less.";
		}
		Page.ClientScript.RegisterStartupScript(Page.GetType(), "TinyMCECleanup", @"$(function(){$('input[type=submit]').attr('onclick', 'if (editorLoaded)tinyMCE.triggerSave(false,true);' + $('input[type=submit]').attr('onclick'));});", true);
	}
}