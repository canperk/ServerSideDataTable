using DataTableServerSide.Attributes;

namespace DataTableServerSide.ViewModels
{
    public abstract class ViewModelBase
    {
        [HiddenColumn]
        public bool IsNew { get; set; }
    }
}