namespace Classes.MLS
{
	public partial class NeighborhoodBuilder
	{
		protected override void ClearRelatedCacheItems()
		{
			if (Cache.IsEnabled)
				Cache.Purge("MLS_Builder_BuilderPageForFrontend_");
		}
	}
}