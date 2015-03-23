using System.Web.UI.WebControls;

namespace TFT.WebControls
{
	/// <summary>
	/// Added ResizerWidth and ResizerHeight properties so that image is not stretched
	/// </summary>
	public class Image : System.Web.UI.WebControls.Image
	{
		public override Unit BorderWidth
		{
			get
			{
				if (base.BorderWidth.IsEmpty)
					return Unit.Pixel(0);
				return base.BorderWidth;
			}
			set { base.BorderWidth = value; }
		}

		public int ResizerWidth { get; set; }
		public int ResizerHeight { get; set; }
	}
}