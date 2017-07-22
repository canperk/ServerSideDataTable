using System.Collections.Generic;

namespace DataTableServerSide.Helpers
{
    public class DTParameters
    {
        public DTParameters()
        {
            Columns = new List<DTColumn>();
            Order = new List<DTOrder>();
        }
        public int Draw { get; set; }
        public List<DTColumn> Columns { get; set; }
        public List<DTOrder> Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DTSearch Search { get; set; }
        public string SortOrder
        {
            get
            {
                return Columns != null && Order != null && Order.Count > 0
                    ? (Columns[Order[0].Column].Data + (Order[0].Dir == DTOrderDir.DESC ? " " + Order[0].Dir : string.Empty))
                    : null;
            }
        }
    }
}
