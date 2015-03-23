namespace Classes.StateAndCountry
{
	public partial class Address
	{
		public string StateName
		{
			get { return this.State != null ? this.State.Name : string.Empty; }
		}

		public string StateAbb
		{
			get { return this.State != null ? this.State.Abb : string.Empty; }
		}

		public string FormattedAddress { get { return ((Address1 + " " + Address2).Trim() + "<br />" + City + ", " + StateAbb + " " + Zip).Trim();  } }
	}
}