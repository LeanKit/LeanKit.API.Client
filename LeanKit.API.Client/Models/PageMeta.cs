namespace LeanKit.Models
{
    public class PageMeta
    {
        public int TotalRecords { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
    }
}