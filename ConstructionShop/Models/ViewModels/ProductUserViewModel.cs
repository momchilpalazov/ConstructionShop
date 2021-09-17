using ConstructionShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConstructionShop.Models.ViewModels
{
    public class ProductUserViewModel
    {

        public ProductUserViewModel()
        {
            ProductsList = new List<Product>();
        }

        public ApplicationUser ApplicationUser { get; set; }

        public IList<Product> ProductsList { get; set; }
    }
}
