using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NLog;
using NorthwindConsole.Models;

//TODO: 
/*
 * 1)DONE-Add product to product table
 * 2)DONE-edit product in product table
 * 3)DONE-Display all products, active products, discontinued products
 * 4)DONE-Display a product by id
 * DONE-Use Nlog
 * 
 * 5)DONE-Add category to category table
 * 6)DONE-edit category in category table
 * 7)DONE-Display all categories, name and description
 * 8)DONE-Display all categories and active products
 * 9)DONE-display single category and its active products
 * 
 * DONE-delete a product
 * DONE-delete a category
 * DONE-Use data annotations, handle user error with Nlog
 * 
 */

namespace NorthwindConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            logger.Info("Program started");
            try
            {
                string choice;
                do
                {
                    Console.WriteLine("1) Add Product");
                    Console.WriteLine("2) Edit Product");
                    Console.WriteLine("3) Display All Products");
                    Console.WriteLine("4) Display Specific Product");
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("5) Add Category");
                    Console.WriteLine("6) Edit Category");
                    Console.WriteLine("7) Display Categories");
                    Console.WriteLine("8) Display all Categories and their related products");
                    Console.WriteLine("9) Display Category and related products");
                    Console.WriteLine("---------------------------");
                    Console.WriteLine("10) Delete Product");
                    Console.WriteLine("11) Delete Category");
                    
                    Console.WriteLine("\"q\" to quit");
                    choice = Console.ReadLine();
                    Console.Clear();
                    logger.Info($"Option {choice} selected");
                    if (choice == "1")
                    {
                        Product product = new Product();
                        Console.WriteLine("Enter Product Name:");
                        product.ProductName = Console.ReadLine();
                        Console.WriteLine("Enter the Quantity per Unit:");
                        product.QuantityPerUnit = Console.ReadLine();
                        Console.WriteLine("Enter the Unit Price:");
                        product.UnitPrice = decimal.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the Units in Stock:");
                        product.UnitsInStock = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the Units on Order:");
                        product.UnitsOnOrder = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the Reorder level:");
                        product.ReorderLevel = Int16.Parse(Console.ReadLine());
                        Console.WriteLine("Discontinued (true/false):");
                        product.Discontinued = bool.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the Category ID:");
                        product.CategoryId = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter the Supplier ID:");
                        product.SupplierId = int.Parse(Console.ReadLine());
                        ValidationContext context = new ValidationContext(product, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(product, context, results, true);
                        if (isValid)
                        {
                            var db = new NorthwindContext();
                            // check for unique name
                            if (db.Products.Any(p => p.ProductName == product.ProductName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                db.AddProduct(product);
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "2")
                    {
                        // edit product
                        Console.WriteLine("Choose the product to edit:");
                        var db = new NorthwindContext();
                        var product = GetProduct(db);
                        Product UpdatedProduct = InputProduct(db);
                        if (UpdatedProduct != null)
                        {
                            UpdatedProduct.ProductID = product.ProductID;
                            db.EditProduct(UpdatedProduct);
                            logger.Info("Product (id: {productid}) updated", UpdatedProduct.ProductID);
                        }
                    }
                    else if (choice == "3")
                    {
                        var db = new NorthwindContext();
                        Console.Clear();
                        Console.WriteLine("1) All Products");
                        Console.WriteLine("2) Active Products");
                        Console.WriteLine("3) Discontinued Products");
                        var option = Console.ReadLine();
                        if (option == "1")
                        {
                            var query = db.Products.OrderBy(p => p.ProductName);
                            Console.WriteLine($"{query.Count()} records returned");
                            Console.WriteLine("All Products");
                            foreach (var item in query)
                            {
                                if (item.Discontinued == true)
                                {
                                    Console.WriteLine($"{item.ProductName} - Discontinued");
                                }
                                else
                                {
                                    Console.WriteLine($"{item.ProductName}");
                                }
                            }
                        }
                        else if (option == "2")
                        {
                            var query = db.Products.Where(p => p.Discontinued == false).OrderBy(p => p.ProductName);
                            Console.WriteLine($"{query.Count()} records returned");
                            Console.WriteLine("Active Products");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.ProductName}");
                            }
                        }
                        else if (option == "3")
                        {
                            var query = db.Products.Where(p => p.Discontinued == true).OrderBy(p => p.ProductName);
                            Console.WriteLine($"{query.Count()} records returned");
                            Console.WriteLine("Discontinued Products");
                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.ProductName}");
                            }
                        }
                    }
                    else if (choice == "4")
                    {
                        var db = new NorthwindContext();
                        var query = db.Products.OrderBy(p => p.ProductID);

                        Console.WriteLine("Select the Product whose details you want to display:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.ProductID}) {item.ProductName}");
                        }
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"ProductID {id} selected");
                        Product product = db.Products.FirstOrDefault(p => p.ProductID == id);
                        Console.WriteLine($"{product.ProductID} - {product.ProductName}");
                        Console.WriteLine($"Supplier ID: - {product.SupplierId}");
                        Console.WriteLine($"Category ID: - {product.CategoryId}");
                        Console.WriteLine($"Quantity per Unit: - {product.QuantityPerUnit}");
                        Console.WriteLine($"Unit Price: - {product.UnitPrice}");
                        Console.WriteLine($"Stock: - {product.UnitsInStock}");
                        Console.WriteLine($"On Order: - {product.UnitsOnOrder}");
                        Console.WriteLine($"Reorder Level: - {product.ReorderLevel}");
                        Console.WriteLine($"Discontinued: - {product.Discontinued}");
                    }
                    else if (choice == "5")
                    {
                        Category category = new Category();
                        Console.WriteLine("Enter Category Name:");
                        category.CategoryName = Console.ReadLine();
                        Console.WriteLine("Enter the Category Description:");
                        category.Description = Console.ReadLine();
                        ValidationContext context = new ValidationContext(category, null, null);
                        List<ValidationResult> results = new List<ValidationResult>();

                        var isValid = Validator.TryValidateObject(category, context, results, true);
                        if (isValid)
                        {
                            var db = new NorthwindContext();
                            // check for unique name
                            if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                            {
                                // generate validation error
                                isValid = false;
                                results.Add(new ValidationResult("Name exists", new string[] { "CategoryName" }));
                            }
                            else
                            {
                                logger.Info("Validation passed");
                                db.AddCategory(category);
                            }
                        }
                        if (!isValid)
                        {
                            foreach (var result in results)
                            {
                                logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                            }
                        }
                    }
                    else if (choice == "6")
                    {
                        // edit category
                        Console.WriteLine("Choose the category to edit:");
                        var db = new NorthwindContext();
                        var category = GetCategory(db);
                        Category UpdatedCategory = InputCategory(db);
                        if (UpdatedCategory != null)
                        {
                            UpdatedCategory.CategoryId = category.CategoryId;
                            db.EditCategory(UpdatedCategory);
                            logger.Info("Category (id: {categoryid}) updated", UpdatedCategory.CategoryId);
                        }
                    }
                    else if (choice == "7")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.OrderBy(p => p.CategoryName);

                        Console.WriteLine($"{query.Count()} records returned");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName} - {item.Description}");
                        }
                    }
                    else if (choice == "8")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryName}");
                            foreach (Product p in item.Products.Where(ap => ap.Discontinued == false))
                            {
                                Console.WriteLine($"\t{p.ProductName}");
                            }
                        }
                    }
                    else if (choice == "9")
                    {
                        var db = new NorthwindContext();
                        var query = db.Categories.Include("Products").OrderBy(p => p.CategoryId);

                        Console.WriteLine("Select the category whose products you want to display:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.CategoryId}) {item.CategoryName}");
                        }
                        int id = int.Parse(Console.ReadLine());
                        Console.Clear();
                        logger.Info($"CategoryId {id} selected");
                        Category category = db.Categories.FirstOrDefault(c => c.CategoryId == id);
                        Console.WriteLine($"{category.CategoryName} - {category.Description}");
                        foreach (Product p in category.Products.Where(ap => ap.Discontinued == false))
                        {
                            Console.WriteLine(p.ProductName);
                        }
                    }
                    else if (choice == "10")
                    {
                        // delete product
                        Console.WriteLine("Choose the product to delete:");
                        var db = new NorthwindContext();
                        var product = GetProduct(db);
                        if (product != null)
                        {
                            db.DeleteProduct(product);
                            logger.Info("Product (id: {productid}) deleted", product.ProductID);
                        }
                    }
                    else if (choice == "11")
                    {
                        // delete category
                        Console.WriteLine("Choose the category to delete:");
                        var db = new NorthwindContext();
                        var category = GetCategory(db);
                        if (category != null)
                        {
                            db.DeleteCategory(category);
                            logger.Info("Category (id: {categoryid}) deleted", category.CategoryId);
                        }
                    }
                    
                    Console.WriteLine();

                } while (choice.ToLower() != "q");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }

        public static Product InputProduct(NorthwindContext db)
        {
            Product product = new Product();
            Console.WriteLine("Enter Product Name:");
            product.ProductName = Console.ReadLine();
            Console.WriteLine("Enter the Quantity per Unit:");
            product.QuantityPerUnit = Console.ReadLine();
            Console.WriteLine("Enter the Unit Price:");
            product.UnitPrice = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Units in Stock:");
            product.UnitsInStock = Int16.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Units on Order:");
            product.UnitsOnOrder = Int16.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Reorder level:");
            product.ReorderLevel = Int16.Parse(Console.ReadLine());
            Console.WriteLine("Discontinued (true/false):");
            product.Discontinued = bool.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Category ID:");
            product.CategoryId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the Supplier ID:");
            product.SupplierId = int.Parse(Console.ReadLine());

            ValidationContext context = new ValidationContext(product, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(product, context, results, true);
            if (isValid)
            {
                return product;
            }
            else
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }

        public static Product GetProduct(NorthwindContext db)
        {
            // display all products
            // force eager loading of products
            var products = db.Products.OrderBy(p => p.ProductID);
            foreach (Product p in products)
            {
                Console.WriteLine($"{p.ProductID} - {p.ProductName}");
            }
            if (int.TryParse(Console.ReadLine(), out int ProductId))
            {
                Product product = db.Products.FirstOrDefault(p => p.ProductID == ProductId);
                if (product != null)
                {
                    return product;
                }
            }
            logger.Error("Invalid Product Id");
            return null;
        }

        public static Category GetCategory(NorthwindContext db)
        {
            // display all categories
            var categories = db.Categories.OrderBy(c => c.CategoryId);
            foreach (Category c in categories)
            {
                Console.WriteLine($"{c.CategoryId} - {c.CategoryName}");
            }
            if (int.TryParse(Console.ReadLine(), out int CategoryId))
            {
                Category category = db.Categories.FirstOrDefault(c => c.CategoryId == CategoryId);
                if (category != null)
                {
                    return category;
                }
            }
            logger.Error("Invalid Category Id");
            return null;
        }

        public static Category InputCategory(NorthwindContext db)
        {
            Category category = new Category();
            Console.WriteLine("Enter Category Name:");
            category.CategoryName = Console.ReadLine();
            Console.WriteLine("Enter the Category Description:");
            category.Description = Console.ReadLine();

            ValidationContext context = new ValidationContext(category, null, null);
            List<ValidationResult> results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(category, context, results, true);
            if (isValid)
            {
                return category;
            }
            else
            {
                foreach (var result in results)
                {
                    logger.Error($"{result.MemberNames.First()} : {result.ErrorMessage}");
                }
            }
            return null;
        }
    }
}
