using System.Collections.Generic;

public class ListingItemWithCount<T>
{
	public List<T> Items { get; set; }
	public int TotalCount { get; set; }
}