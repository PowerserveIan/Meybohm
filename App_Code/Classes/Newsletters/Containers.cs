namespace Classes.Newsletters
{
	public class UrlClickCountContainer
	{
		public string Url { get; set; }
		public int ClickCount { get; set; }

		public UrlClickCountContainer(string url, int clickCount)
		{
			Url = url;
			ClickCount = clickCount >= 0 ? clickCount: 0;
		}
		public UrlClickCountContainer(string url, int? clickCount)
		{
			Url = url;
			ClickCount = clickCount >= 0 ? clickCount.Value : 0;
		}
	}

	public class SubscriberEmailWithFormatContainer
	{
		public string Email { get; set; }
		public string Format { get; set; }

		public SubscriberEmailWithFormatContainer(string email, string format)
		{
			Email = email;
			Format = format;
		}
	}
}