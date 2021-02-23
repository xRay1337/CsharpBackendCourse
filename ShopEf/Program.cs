using ShopEf.Models;
using System;
using System.Linq;
using System.Threading;

namespace ShopEf
{
    internal class Program
    {
        internal static void Main()
        {
            using (var db = new ShopContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                AddData(db);

                // Найти самый часто покупаемый товар
                var topProduct = db.Products
                    .Select(p => new { p.Name, Count = p.Orders.Sum(o => o.Count) })
                    .OrderByDescending(p => p.Count)
                    .FirstOrDefault();

                Console.WriteLine($"Хит продаж: {topProduct.Name}");

                Console.WriteLine();

                // Найти сколько каждый клиент потратил денег за все время
                var spending = db.Customers
                    .Select(c => new { c.LastName, Amount = c.Orders.Sum(o => o.Count * o.Product.Price ?? 0) })
                    .OrderByDescending(c => c.Amount)
                    .ThenBy(c => c.LastName)
                    .ToArray();

                foreach (var s in spending)
                {
                    Console.WriteLine($"Сумма покупок: {s.Amount:00000000} р. {s.LastName}");
                }

                Console.WriteLine();

                // Вывести сколько товаров каждой категории купили
                var salesByCategory = db.Categories
                    .Select(c => new { c.Name, Count = c.CategoryProducts.Select(cp => cp.Product).SelectMany(p => p.Orders).Sum(o => o.Count) })
                    .OrderByDescending(c => c.Count)
                    .ThenBy(c => c.Name)
                    .ToArray();

                foreach (var s in salesByCategory)
                {
                    Console.WriteLine($"Кол-во продаж: {s.Count:00000000}\t{s.Name}");
                }
            }

            Console.ReadKey();
        }

        internal static void AddData(ShopContext context)
        {
            var processors = context.Categories.Add(new Category("Процессоры"));
            var videoCards = context.Categories.Add(new Category("Видеокарты"));
            var randomAccessMemory = context.Categories.Add(new Category("Оперативная память"));

            var processor1 = context.Products.Add(new Product("Intel Core i5 Coffee Lake", 11_999));
            var processor2 = context.Products.Add(new Product("Intel Xeon Platinum Skylake", 199_628));
            var processor3 = context.Products.Add(new Product("Intel Core i3-9100F", 6_290));
            var processor4 = context.Products.Add(new Product("AMD Ryzen Threadripper Colfax", 34_360));
            var processor5 = context.Products.Add(new Product("AMD Ryzen 5 2600", 11_290));

            var videoCard1 = context.Products.Add(new Product("GIGABYTE GeForce GTX 1660", 21_990));
            var videoCard2 = context.Products.Add(new Product("MSI GeForce RTX 2060 SUPER", 40_340));
            var videoCard3 = context.Products.Add(new Product("GIGABYTE GeForce GTX 1650", 15_870));
            var videoCard4 = context.Products.Add(new Product("Palit GeForce GTX 1650", 14_190));
            var videoCard5 = context.Products.Add(new Product("GeForce GTX 1050 Ti", 11_610));

            var randomAccessMemory1 = context.Products.Add(new Product("Samsung M378A1K43CB2-CTD", 2_650));
            var randomAccessMemory2 = context.Products.Add(new Product("HyperX Fury HX426C16FB3K2/16", 5_990));
            var randomAccessMemory3 = context.Products.Add(new Product("AMD Radeon R7 Performance R744G2400U1S-UO", 1_290));

            var customer1 = context.Customers.Add(new Customer("Злобин", "Виталий", "Андреевич", "89137373455", "zlovian@yandex.ru"));
            var customer2 = context.Customers.Add(new Customer("Григорьев", "Игорь", "Дмитриевич", "89529373625", "grigid@yandex.ru"));
            var customer3 = context.Customers.Add(new Customer("Зебницкий", "Илья", "Валерьевич", "89065473638", "zebniciv@yandex.ru"));

            context.SaveChanges();

            context.CategoryProducts.Add(new CategoryProduct(processors.Entity, processor1.Entity));
            context.CategoryProducts.Add(new CategoryProduct(processors.Entity, processor2.Entity));
            context.CategoryProducts.Add(new CategoryProduct(processors.Entity, processor3.Entity));
            context.CategoryProducts.Add(new CategoryProduct(processors.Entity, processor4.Entity));
            context.CategoryProducts.Add(new CategoryProduct(processors.Entity, processor5.Entity));
            context.CategoryProducts.Add(new CategoryProduct(videoCards.Entity, videoCard1.Entity));
            context.CategoryProducts.Add(new CategoryProduct(videoCards.Entity, videoCard2.Entity));
            context.CategoryProducts.Add(new CategoryProduct(videoCards.Entity, videoCard3.Entity));
            context.CategoryProducts.Add(new CategoryProduct(videoCards.Entity, videoCard4.Entity));
            context.CategoryProducts.Add(new CategoryProduct(videoCards.Entity, videoCard5.Entity));
            context.CategoryProducts.Add(new CategoryProduct(randomAccessMemory.Entity, randomAccessMemory1.Entity));
            context.CategoryProducts.Add(new CategoryProduct(randomAccessMemory.Entity, randomAccessMemory2.Entity));
            context.CategoryProducts.Add(new CategoryProduct(randomAccessMemory.Entity, randomAccessMemory3.Entity));

            context.Orders.Add(new Order(customer1.Entity.Id, processor1.Entity.Id, 1));
            context.Orders.Add(new Order(customer1.Entity.Id, videoCard1.Entity.Id, 1));
            context.Orders.Add(new Order(customer1.Entity.Id, randomAccessMemory1.Entity.Id, 1));

            Thread.Sleep(500);

            context.Orders.Add(new Order(customer2.Entity.Id, processor2.Entity.Id, 1));
            context.Orders.Add(new Order(customer2.Entity.Id, videoCard2.Entity.Id, 1));
            context.Orders.Add(new Order(customer2.Entity.Id, randomAccessMemory2.Entity.Id, 3));

            Thread.Sleep(500);

            context.Orders.Add(new Order(customer3.Entity.Id, processor3.Entity.Id, 1));
            context.Orders.Add(new Order(customer3.Entity.Id, videoCard3.Entity.Id, 1));
            context.Orders.Add(new Order(customer3.Entity.Id, randomAccessMemory3.Entity.Id, 1));

            context.SaveChanges();
        }
    }
}