using System.Collections.Generic;

namespace NorthwindConsole.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual List<Product> Product { get; set; }
    }
}
