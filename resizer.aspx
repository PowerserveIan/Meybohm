<%@ Page Trace="false" Language="C#" %>

<%@ OutputCache Duration="2678400" VaryByParam="*" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Drawing2D" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing.Imaging" %>
<%@ Import Namespace="BaseCode" %>
<script runat="server">
	/*
	Author: Karthik Giddu 
	Email: gidduk@vsnl.com
	Updated: 5-May-2002
	Language: VB.NET
	Framework Version: V1
	Create thumbnail 
	images on the fly which will double as image conversion (bmp to jpg/gif etc) utility

	rewritten & converted to c# by Merlyn@352 Media Group w/code from http://groups.google.com/groups?hl=en&lr=&ie=UTF-8&selm=%23iJRx%24J9CHA.2452%40TK2MSFTNGP10.phx.gbl&rnum=6
	
	Modified to support PNGs and trimming (a crop that favors keeping more of the image rather than keeping the original resolution) by wmoore at 352 Media Group.
	*/

	private const string FileNameQuery = "filename";
	private const string WidthQuery = "width";
	private const string HeightQuery = "height";
	private const string CropQuery = "crop";
	private const string TrimQuery = "trim";
	private const double MaxResize = 1;

	protected override void OnLoad(EventArgs e)
	{
		System.Drawing.Image original, thumb;

		bool cropRequested, trimRequested;
		int width, height, passedInWidth, passedInHeight;

		if (!Boolean.TryParse(Request.QueryString[CropQuery], out cropRequested))
			cropRequested = "1".Equals(Request.QueryString[CropQuery]);

		if (!Boolean.TryParse(Request.QueryString[TrimQuery], out trimRequested))
			trimRequested = "1".Equals(Request.QueryString[TrimQuery]);

		try
		{
			width = passedInWidth = Math.Max(1, int.Parse(Request.QueryString[WidthQuery]));
		}
		catch (Exception)
		{
			width = passedInWidth = -1;
		}

		try
		{
			height = passedInHeight = Math.Max(1, int.Parse(Request.QueryString[HeightQuery]));
		}
		catch (Exception)
		{
			height = passedInHeight = -1;
		}
		string fileName = string.Empty;

		try
		{
			if (Request.QueryString[FileNameQuery].StartsWith("http"))
			{
				fileName = Request.QueryString[FileNameQuery];
				using (System.Net.WebClient webclient = new System.Net.WebClient())
				{
					byte[] imageData = webclient.DownloadData(Request.QueryString[FileNameQuery]);
					MemoryStream stream = new MemoryStream(imageData);
					original = System.Drawing.Image.FromStream(stream);
					stream.Close();
				}
			}
			else
			{
				fileName = Server.MapPath(Request.QueryString[FileNameQuery]);
				original = System.Drawing.Image.FromFile(fileName);
			}
		}
		catch (Exception)
		{
			try
			{
				fileName = Server.MapPath("~/" + Globals.Settings.MissingImagePath);
				original = System.Drawing.Image.FromFile(fileName);
			}
			catch (Exception)
			{
				width = 100;
				height = 100;
				original = System.Drawing.Image.FromFile(GetErrorBitmap(width, height));
			}
		}

		using (original)
		{
			if (height > -1)
			{
				height = Convert.ToInt32(Math.Min((original.Height) * MaxResize, height));
			}

			if (width > -1)
			{
				width = Convert.ToInt32(Math.Min((original.Width) * MaxResize, width));
			}

			float originalRatio = original.Width / (float)original.Height;
			float thumbRatio = width / (float)height;
			bool bothSpecified = width != -1 && height != -1;

			if (trimRequested)
			{
				if (width == -1)
					width = passedInWidth = original.Width;
				if (height == -1)
					height = passedInHeight = original.Height;
				double trimMultiplier;

				if (thumbRatio <= originalRatio)
					trimMultiplier = height / (double)original.Height;
				else
					trimMultiplier = width / (double)original.Width;

				height = (int)Math.Ceiling(original.Height * trimMultiplier);
				width = (int)Math.Ceiling(original.Width * trimMultiplier);
			}
			else if (cropRequested)
			{
				if (width == -1)
					width = passedInWidth = original.Width;
				if (height == -1)
					height = passedInHeight = original.Height;

				//find which measurement is closest to the desired size
				int differenceInWidths = original.Width - passedInWidth;
				int differenceInHeights = original.Height - passedInHeight;

				double cropMultiplier = Math.Max(1, differenceInHeights > differenceInWidths ? Math.Floor((double)(original.Width / width)) : Math.Floor((double)(original.Height / height)));

				width = Convert.ToInt32(original.Width / cropMultiplier);
				height = Convert.ToInt32(original.Height / cropMultiplier);
			}
			else
			{
				if (width == -1 && height == -1)
				{
					width = original.Width;
					height = original.Height;
				}
				else if (width == -1 || bothSpecified && height < width)
				{
					width = Convert.ToInt32(originalRatio * height);
				}
				else if (height == -1 || bothSpecified)
				{
					height = Convert.ToInt32((width) / originalRatio);
				}
			}

			if (!trimRequested)
			{
				width = Math.Min(width, original.Width);
				height = Math.Min(height, original.Height);
			}

			using (thumb = new Bitmap(width, height))
			{
				Regex pngMatch = new Regex(".png$", RegexOptions.IgnoreCase);
				Regex gmatch = new Regex("gif$", RegexOptions.IgnoreCase);

				using (Graphics g = Graphics.FromImage(thumb))
				{
					g.InterpolationMode = InterpolationMode.HighQualityBicubic;
					g.SmoothingMode = SmoothingMode.HighQuality;
					g.PixelOffsetMode = PixelOffsetMode.HighQuality;
					g.CompositingQuality = CompositingQuality.HighQuality;
					g.DrawImage(original,
								new Rectangle(0, 0, width, height),
								new Rectangle(0, 0, original.Width, original.Height),
								GraphicsUnit.Pixel);

					if (Request.QueryString["wm"] == "true")
					{
						using (System.Drawing.Image watermark = System.Drawing.Image.FromFile(Server.MapPath("images/watermark.png")))
						{
							Rectangle drawRect = new Rectangle(0, 0, watermark.Size.Width, watermark.Size.Height);
							g.DrawImage(watermark,
										new Rectangle(thumb.Size.Width / 3, thumb.Size.Height / 3, watermark.Width / 2, watermark.Height / 2),
										drawRect,
										GraphicsUnit.Pixel);
						}
					}

					if (trimRequested)
					{
						int trimX = 0, trimY = 0;
						if (thumbRatio <= originalRatio)
							trimX = Math.Max(0, (width / 2) - (passedInWidth / 2));
						else
							trimY = Math.Max(0, (height / 2) - (passedInHeight / 2));

						Rectangle rect = new Rectangle(trimX, trimY, Math.Min(passedInWidth, width), Math.Min(passedInHeight, height));
						thumb = cropImage(thumb, rect);
					}
					else if (cropRequested)
					{
						int cropX = Math.Max(0, (width / 2) - (passedInWidth / 2));
						int cropY = Math.Max(0, (height / 2) - (passedInHeight / 2));

						Rectangle rect = new Rectangle(cropX, cropY, Math.Min(passedInWidth, width), Math.Min(passedInHeight, height));
						thumb = cropImage(thumb, rect);
					}

					if (pngMatch.IsMatch(fileName))
					{
						Response.ContentType = "image/png";
						// need a bidirectional stream to save the PNG initially, so buffer there, THEN write into the Response
						MemoryStream bidiPngStream = new MemoryStream(thumb.Height * thumb.Width * 3); //initial capacity guess based on 24-bit PNG
						thumb.Save(bidiPngStream, ImageFormat.Png); // defer to default PNG codec
						Response.OutputStream.Write(bidiPngStream.GetBuffer(), 0, (int)bidiPngStream.Length);
					}
					if (gmatch.IsMatch(fileName))
					{
						Response.ContentType = "image/gif";
						thumb.Save(Response.OutputStream, ImageFormat.Gif);
					}
					else
					{
						Response.ContentType = "image/jpeg";
						ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
						System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
						EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
						EncoderParameters myEncoderParameters = new EncoderParameters(1);
						myEncoderParameters.Param[0] = myEncoderParameter;
						thumb.Save(Response.OutputStream, myImageCodecInfo, myEncoderParameters);
					}
				}
			}
		}
		Response.AddCacheItemDependency("Resizer_" + fileName);
	}

	private static System.Drawing.Image cropImage(System.Drawing.Image img, Rectangle cropArea)
	{
		using (Bitmap bmpImage = new Bitmap(img))
		{
			Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
			return (bmpCrop);
		}
	}

	private static ImageCodecInfo GetEncoderInfo(String mimeType)
	{
		int j;
		ImageCodecInfo[] encoders;
		encoders = ImageCodecInfo.GetImageEncoders();
		for (j = 0; j < encoders.Length; ++j)
		{
			if (encoders[j].MimeType == mimeType)
				return encoders[j];
		}
		return null;
	}

	private string GetErrorBitmap(int width, int height)
	{
		return "~/resizer.aspx?filename=" + Globals.Settings.MissingImagePath + "&width=" + width + "&height=" + height;
	}

</script>
