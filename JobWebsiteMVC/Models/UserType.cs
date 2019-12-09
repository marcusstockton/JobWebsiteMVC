using System;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.Models
{
    public class UserType
    {
        public Guid Id { get; set; }
        
        [MaxLength(50)]
        public string Description { get; set; }
    }
}