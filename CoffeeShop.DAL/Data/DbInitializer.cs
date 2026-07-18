using CoffeeShop.Models.Entities.Catalog; // Namespace chứa Category, Product
using CoffeeShop.Models.Entities.Sales;

namespace CoffeeShop.DAL.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            Console.WriteLine("====== BẮT ĐẦU KHỞI ĐỘNG SEEDER ======");

            // 1. KIỂM TRA VÀ SEED CATEGORY & PRODUCT
            if (!context.Categories.Any() && !context.Products.Any())
            {
                Console.WriteLine("====== BƠM DỮ LIỆU MENU... ======");
                var catTraditional = new Category
                {
                    Name = "Cà phê truyền thống",
                    Description = "Đậm đà hương vị Việt",
                    DisplayOrder = 1,
                    IsActive = true
                };
                var catMachine = new Category
                {
                    Name = "Cà phê máy",
                    Description = "Phong cách Espresso Ý",
                    DisplayOrder = 2,
                    IsActive = true
                };
                var catModern = new Category
                {
                    Name = "Cà phê hiện đại",
                    Description = "Sáng tạo & Độc đáo",
                    DisplayOrder = 3,
                    IsActive = true
                };
                context.Categories.AddRange(catTraditional,catMachine,catModern);
                context.SaveChanges();
                string dummyImg = "https://images.unsplash.com/photo-1559525839-b184a4d698c7?w=500&auto=format&fit=crop&q=60";

                var products = new List<Product>
            {
                // --- NHÓM 1: TRUYỀN THỐNG ---
                new Product { Name = "Cà phê đen (Đá/Nóng)", Price = 25000, Image = dummyImg, CategoryId = catTraditional.Id },
                new Product { Name = "Cà phê sữa (Đá/Nóng)", Price = 30000, Image = dummyImg, CategoryId = catTraditional.Id },
                new Product { Name = "Bạc xỉu", Price = 35000, Image = dummyImg, CategoryId = catTraditional.Id },
                new Product { Name = "Cà phê trứng", Price = 45000, Image = dummyImg, CategoryId = catTraditional.Id },

                // --- NHÓM 2: MÁY Ý ---
                new Product { Name = "Espresso", Price = 35000, Image = dummyImg, CategoryId = catMachine.Id },
                new Product { Name = "Americano", Price = 40000, Image = dummyImg, CategoryId = catMachine.Id },
                new Product { Name = "Cappuccino", Price = 50000, Image = dummyImg, CategoryId = catMachine.Id },
                new Product { Name = "Latte", Price = 50000, Image = dummyImg, CategoryId = catMachine.Id },
                new Product { Name = "Mocha", Price = 55000, Image = dummyImg, CategoryId = catMachine.Id },

                // --- NHÓM 3: HIỆN ĐẠI ---
                new Product { Name = "Cà phê muối", Price = 45000, Image = dummyImg, CategoryId = catModern.Id },
                new Product { Name = "Cà phê dừa", Price = 50000, Image = dummyImg, CategoryId = catModern.Id },
                new Product { Name = "Cold Brew", Price = 55000, Image = dummyImg, CategoryId = catModern.Id }
            };
                context.Products.AddRange(products);
                context.SaveChanges();
                Console.WriteLine("====== BƠM DỮ LIỆU MENU THÀNH CÔNG! ======");
            }
            else
            {
                Console.WriteLine("====== MENU ĐÃ CÓ DỮ LIỆU, BỎ QUA ======");
            }

            // 2. KIỂM TRA VÀ SEED CUSTOMER ĐỘC LẬP
            if (!context.Customers.Any())
            {
                Console.WriteLine("====== BƠM DỮ LIỆU KHÁCH HÀNG... ======");
                var customer = new List<Customer>
            {
            // Set Id = 1 (tuỳ thuộc sếp có dùng Identity tự tăng không, nếu auto increment thì bỏ Id đi)
                new Customer { FullName = "SieuThatNghiep", PhoneNumber = "0123456789"}
            };
                context.Customers.AddRange(customer);
                context.SaveChanges();
                Console.WriteLine("====== BƠM DỮ LIỆU KHÁCH HÀNG THÀNH CÔNG! ======");
            }
            else
            {
                Console.WriteLine("====== KHÁCH HÀNG ĐÃ CÓ DỮ LIỆU, BỎ QUA ======");
            }
        }
    }
}
