namespace Classes.Contacts
{
	public enum ContactMethods
	{
		WorkPhone = 1,
		HomePhone = 2,
		CellPhone = 3,
		Email = 4
	}
	
	public enum ContactStatuses
	{
		Unread = 1,
		Read = 2,
		Answered = 3
	}

	public enum ContactTimes
	{
		Morning = 1,
		Afternoon = 2,
		Evening = 3,
		AnyTime = 4
	}

	public enum ContactTypes
	{
		ContactUs = 1,
		HomeValuationRequest = 2,
		MaintenanceRequest = 3,
		PropertyInformation = 4,
		Agent = 5
	}
}