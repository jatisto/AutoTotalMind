using System.Web.Mvc;

namespace AutoTotalMind.Models
{
    public class Subpage
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
    }
}