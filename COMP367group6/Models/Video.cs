using System.ComponentModel.DataAnnotations;

namespace COMP367group6.Models
{
    public class Video
    {
        public int VidId { get; set; } // Primary Key

        [Required]
        public string Title { get; set; }
        public byte[] Thumbnail { get; set; }

        [Required]
        public string Url { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
        public User User { get; set; }
    }
}