namespace AutoTotalMind.Models
{
    public class Image
    {
        public int ID { get; set; }
        public string ImageUrl { get; set; }
        public string Alt { get; set; }
        public int ProductID { get; set; }
        public int SubpageID { get; set; }
    }
}