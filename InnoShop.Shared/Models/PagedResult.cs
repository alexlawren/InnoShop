namespace InnoShop.Shared.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public PagedResult(PagedList<T> pagedList)
        {
            Items = pagedList;
            TotalCount = pagedList.TotalCount;
            PageSize = pagedList.PageSize;
            CurrentPage = pagedList.CurrentPage;
            TotalPages = pagedList.TotalPages;
            HasNext = pagedList.HasNext;
            HasPrevious = pagedList.HasPrevious;
        }
        public PagedResult() { }

        public PagedResult(List<T> items, int totalCount, int pageSize, int currentPage)
        {
            Items = items;
            TotalCount = totalCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            HasNext = currentPage < TotalPages;
            HasPrevious = currentPage > 1;
        }
    }
}