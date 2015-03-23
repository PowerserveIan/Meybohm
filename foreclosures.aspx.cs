public partial class foreclosures : BasePage
{
	protected string Zipcode
	{
		get
		{
			if (((microsite)Master).CurrentMicrosite.Name == "Aiken")
				return "29801";
			return "30904";
		}
	}

	public override void SetComponentInformation()
	{
	}
}