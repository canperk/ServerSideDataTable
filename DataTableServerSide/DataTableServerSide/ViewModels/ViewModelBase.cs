using DataTableServerSide.Attributes;
using DataTableServerSide.Enums;

namespace DataTableServerSide.ViewModels
{
    public abstract class ViewModelBase
    {
        [HiddenColumn]
        public bool IsNew { get; set; }

        [HiddenColumn]
        public ObjectState ObjectState { get; set; }
    }
}