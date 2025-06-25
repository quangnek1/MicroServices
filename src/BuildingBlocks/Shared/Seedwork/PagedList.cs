namespace Shared.Seedwork
{
	public class PagedList<T> : List<T>
	{
		public PagedList(IEnumerable<T> items, long totalItems, int pageNumber, int pageSize)
		{
			_metaData = new MetaData()
			{
				TotalItems = totalItems,
				CurrentPage = pageNumber,
				PageSize = pageSize,
				TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
			};
			AddRange(items);
		}

		private MetaData _metaData { get; }
		public MetaData GetMetaData() => _metaData;
	}
}
