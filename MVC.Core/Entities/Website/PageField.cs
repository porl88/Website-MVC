namespace MVC.Core.Entities.Website
{
    public class PageField : BaseEntity<int>
    {
        public int PageId { get; set; }

        public string Type { get; set; }

        public string Label { get; set; }
    }
}
