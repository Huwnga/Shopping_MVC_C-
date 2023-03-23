using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingOnline.Models
{
    public class Categories
    {
        [Required]
        public int CategoryID { get; set; }
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
    }
}