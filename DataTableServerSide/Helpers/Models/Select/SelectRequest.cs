namespace DataTableServerSide.Helpers
{
    public class SelectRequest
    {
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public string Url { get; set; }
    }
}
