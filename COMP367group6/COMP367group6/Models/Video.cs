namespace COMP367group6.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public User User { get; set; }
    }
}