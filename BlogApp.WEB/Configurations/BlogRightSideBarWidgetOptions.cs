using BlogApp.Core.Enums.ComplexTypes;

namespace BlogApp.WEB.Configurations
{
    public class BlogRightSideBarWidgetOptions
    {
        public string Header { get; set; }
        public int TakeSize { get; set; }
        public int CategoryId { get; set; }
        public FilterBy FilterBy { get; set; }
        public OrderBy OrderBy { get; set; }
        public bool IsAscending { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int MaxViewCount { get; set; }
        public int MinViewCount { get; set; }
        public int MaxCommentCount { get; set; }
        public int MinCommentCount { get; set; }
    }
}
