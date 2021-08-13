﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Data
{
    public class Product
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string SimpleDescription { get; set; }

        public string Description { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Price { get; set; }
        public string  Pictures { get; set; }
        [Display(Name ="Category Type")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public  Category Category { get; set; }


        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public ApplicationType ApplicationType { get; set; }
    }
}
