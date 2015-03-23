using System.Collections.Generic;

public class ListingItemWithAdditionalOutput<T> : ListingItemWithCount<T>
{
	public decimal TotalSales { get; set; }
}