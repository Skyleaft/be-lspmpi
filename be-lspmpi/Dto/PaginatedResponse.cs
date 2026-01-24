namespace be_lspmpi.Dto
{
    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PaginatedResponse(List<T> data, int totalCount, int page, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
        public PaginatedResponse()
        {

        }
    }
}