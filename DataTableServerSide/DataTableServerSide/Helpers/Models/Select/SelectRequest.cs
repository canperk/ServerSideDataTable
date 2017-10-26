using System.Collections.Generic;

namespace DataTableServerSide.Helpers
{
    public class SelectRequest
    {
        public SelectRequest()
        {
            Values = new List<string>();
        }
        public string SearchTerm { get; set; }
        public int PageSize { get; set; }
        public List<string> Values { get; set; }
    }
}
