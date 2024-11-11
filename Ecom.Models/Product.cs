using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecom.Models
{
    public class Product
    {
        /**
         * [TT]: EF data annotation attribute for primary key.
         */
        [Key]
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string? Description { get; set; }
        
        public string ISBN { get; set; }
        
        public string Author{ get; set; }
        
        [DisplayName("List price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [DisplayName("Price for 1-50")]
        [Range(1, 1000)]
        public double Price{ get; set; }

        [DisplayName("Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [DisplayName("Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever] // [TT]: Using [ValidateNever] here instead of "?" because Category property should never be null, but I still don't want ModelState to validate it.
        public Category Category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
