namespace Classes.Newsletters
{
	public enum Action
	{
		Send = 1,
		Open = 2,
		ReturnReciept = 3,
		Click = 4,
		Forward = 5,
		HardBounce = 6,
		SoftBounce = 7,
		Unsubscribe = 8,
		SendFailed = 9
	}

	public enum NewsletterSendingTypeFormat
	{
		HtmlAndText,
		TextOnly,
		HtmlOnly,
		Multipart
	}
	
	public enum SubscribeUserReturnCode
	{
		Success = 0,
		Already_Subscribed,
		Missing_MailingList,
		MailingList_Full
	}

	public enum UnsubscribeUserReturnCode
	{
		Success = 0,
		Never_Subscribed,
		Already_Unsubscribed,
		Missing_MailingList
	}
}