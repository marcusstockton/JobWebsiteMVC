using System;
using System.ComponentModel.DataAnnotations;

namespace JobWebsiteMVC.Models
{
    public abstract class Base
    {
        [Key]
        public Guid Id { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? UpdatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}