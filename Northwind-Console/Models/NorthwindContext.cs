using System.Data.Entity;

namespace NorthwindConsole.Models
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext() : base("name=NorthwindContext") { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public void AddCategory(Category category)
        {
            this.Categories.Add(category);
            this.SaveChanges();
        }

        public void AddProduct(Product product)
        {
            this.Products.Add(product);
            this.SaveChanges();
        }

        public void EditProduct(Product UpdatedProduct)
        {
            Product product = this.Products.Find(UpdatedProduct.ProductID);
            product.ProductID = UpdatedProduct.ProductID;
            product.ProductName = UpdatedProduct.ProductName;
            product.QuantityPerUnit = UpdatedProduct.QuantityPerUnit;
            product.UnitPrice = UpdatedProduct.UnitPrice;
            product.UnitsInStock = UpdatedProduct.UnitsInStock;
            product.UnitsOnOrder = UpdatedProduct.UnitsOnOrder;
            product.ReorderLevel = UpdatedProduct.ReorderLevel;
            product.Discontinued = UpdatedProduct.Discontinued;
            product.CategoryId = UpdatedProduct.CategoryId;
            product.SupplierId = UpdatedProduct.SupplierId;
            product.Category = UpdatedProduct.Category;
            product.Supplier = UpdatedProduct.Supplier;
            this.SaveChanges();
        }

        public void EditCategory(Category UpdatedCategory)
        {
            Category category = this.Categories.Find(UpdatedCategory.CategoryId);
            category.CategoryId = UpdatedCategory.CategoryId;
            category.CategoryName = UpdatedCategory.CategoryName;
            category.Description = UpdatedCategory.Description;
            category.Products = UpdatedCategory.Products;
            this.SaveChanges();
        }

        public void DeleteCategory(Category category)
        {
            this.Categories.Remove(category);
            this.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            this.Products.Remove(product);
            this.SaveChanges();
        }
    }
}
