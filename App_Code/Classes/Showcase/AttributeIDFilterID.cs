namespace Classes.Showcase
{
	public class AttributeIDFilterID
	{
		public int AttributeID { get; set; }
		public int FilterID { get; set; }

		public string GetForArrayOjObjects
		{
			get
			{
				return string.Format("'{0}':{1}", AttributeID, FilterID);
			}
		}

		public string GetForArray
		{
			get
			{
				return string.Format("[{0},{1}]", AttributeID, FilterID);
			}
		}
	}
}