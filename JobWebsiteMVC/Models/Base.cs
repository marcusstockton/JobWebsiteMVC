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

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTimeOffset CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTimeOffset? UpdatedDate { get; set; }

        public bool IsActive { get; set; }
    }
}