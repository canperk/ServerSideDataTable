namespace DataTableServerSide.Helpers
{
    public abstract class DTRow
    {
        public virtual string DT_RowId
        {
            get { return null; }
        }

        public virtual string DT_RowClass { get { return null; } }

        public virtual object DT_RowData { get { return null; } }
    }
}
