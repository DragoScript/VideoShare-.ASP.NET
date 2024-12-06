using System.ComponentModel.DataAnnotations;
namespace COMP367group6.Models 
{
    public class User
    {
        public int Id { get; set; } // Primary Key

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
}