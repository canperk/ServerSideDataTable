using System;

namespace DataTableServerSide.Attributes
{
    public class AutoCompleteAttribute : Attribute
    {
        public AutoCompleteAttribute()
        {
            PageSize = 15;
            InputLength = 0;
        }
        public int PageSize { get; set; }
        public int InputLength { get; set; }
        public string Url { get; set; }
    }
}
