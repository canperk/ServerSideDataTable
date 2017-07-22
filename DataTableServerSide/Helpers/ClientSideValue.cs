namespace DataTableServerSide.Helpers
{
    public class ClientSideValue
    {
        public ClientSideValue(string name, object value)
        {
            PropertyName = name;
            Value = value;
        }
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
}
