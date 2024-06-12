namespace EccomerceApi.Model
{
    public class PagedResultModel<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int currentPage { get; set; }
        public int pageSize { get; set; }
    }

}
