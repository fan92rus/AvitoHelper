using AvitoHelper.Controllers;
using AvitoHelper.DataBase;
using AvitoHelper.Helpers;
using AvitoHelper.Services;
using AvitoHelper.Types;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        public void TestAvitoParser()
        {
            Order order = new Order()
            {
                id = 0,
                Link = "/rossiya/planshety_i_elektronnye_knigi?cd=1&pmax=2200&pmin=900&s=104&p=3&q=samsung+galaxy+tab+3",
            };


            var context_options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("testDb").Options;
            var mockConttext = new DatabaseContext(context_options);

            //AvitoParser parser = new AvitoParser(new EmailSender(), mockConttext);
            //var res = parser.Parse(order, 2);
            //Assert.Pass();
        }
        [Test]
        public void TestAuth()
        {
            //var context_options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("testDb").Options;

            //var context = new DatabaseContext(context_options);
            //AuthController controller = new AuthController(context);
            //var emailMock = new EmailSender();
            //controller.GetCode("fan92rus@mail.ru", emailMock);
            //Assert.IsTrue(context.Users.Find(true) != null);
        }

        public void TestParseAll()
        {
            var context_options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("testDb").Options;

            var context = new DatabaseContext(context_options);
            context.Users.Add(new User()
            {
                email = "fan92rus@mail.ru",
                Deep = 2,
                Limit = 5,
                Orders = new List<Order>()
                {
                    new Order()
                    {
                        IgnoreFirstIteration = true,
                        id = 0,
                        Link = "/rossiya/planshety_i_elektronnye_knigi",
                        products = new List<Product>(),
                    }
                }
            });
            context.SaveChanges();
            //AvitoParser parser = new AvitoParser(new EmailSender(), context);
            //parser.PaseAll();
        }
        [Test]
        public void CheckPaymentHash()
        {
            string GetHashTwo(Purchace purchace)
            {
                return "";
                //     return new Crypto().CalculateMD5Hash(purchace.outSumm.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + ":" + purchace.PayId + ":" + passTwo + ":type=" + purchace.type);
            }

            RoboKassaService service = new RoboKassaService();
            var hash = GetHashTwo(new Purchace()
            {
                Id = 3,
                PayId = 345,
                outSumm = (decimal)345.24,
                type = "subscribe"
            });
            //var result = service.CheckHash("345.24", "345", "subscribe", hash);
        }
        [Test]
        public void TestEmailParse()
        {
            EmailHelper helper = new EmailHelper();
            helper.GetAuthEmail("4454326dfsgf", "https://github.com/dotliquid/dotliquid/wiki/DotLiquid-for-Developers");
            List<Product> products = new List<Product>();
            products.Add(new Product()
            {
                Image = "https://13.img.avito.st/208x156/6294300213.jpg",
                Link = "https://www.avito.ru/barnaul/oborudovanie_dlya_biznesa/svarochnyy_stol_3d_20001000_mm_1824017886",
                Name = "Сварочный стол 3D 2000*1000 мм",
                Price = "35 000",
                Text = ""
            });
            products.Add(new Product()
            { 
              Image = "https://91.img.avito.st/208x156/7884963091.jpg",
              Link = "https://www.avito.ru/moskva/telefony/google_pixel_2_xl_64_gb_black_1829442853",
              Price = "8 000",
              Name = "Google Pixel 2 XL 64 gb black",
              Text = "Куплен в США, полностью оригинальный. Упала отвёртка на экран в районе динамика. Треснуло стекло, захрипел динамик. Все остальное исправно. Комплект на фото без наушников и зарядного устройства. Обмен не интересует."
            });
            var productsRes = helper.GetProductEmail(products);
        }
    }
}