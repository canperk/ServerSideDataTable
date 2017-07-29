namespace DataTableServerSide.ViewModels
{
    public class SelectViewModel
    {
        public SelectViewModel(string id, string text)
        {
            Id = id;
            Text = text;
        }
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
