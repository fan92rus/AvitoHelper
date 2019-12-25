using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AvitoHelper.DataBase;
using AvitoHelper.Helpers;
using AvitoHelper.Types;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;

namespace AvitoHelper.Services
{
    public class AvitoParser : BaseParser
    {

        RestClient client = new RestClient("https://www.avito.ru/");
        public AvitoParser(EmailSender emailer, EmailHelper helper, IServiceScopeFactory scopeFactory) : base(emailer, helper, scopeFactory)
        {
            this._baseUrl = "https://www.avito.ru";
        }

        public override List<Product> Parse(Order order, int pageCount, bool isAnalize)
        {
            List<Product> products = new List<Product>();

            for (int i = 1; i < pageCount; i++)
            {
                string Content = GetContent(order.Link, i);

                if (!string.IsNullOrEmpty(Content))
                {
                    if ((order.NeedWords == null && order.ignoreWords == null) || !isAnalize)
                    {
                        products.AddRange(ParseProductsFromPage(Content));
                    }
                    else
                    {
                        products.AddRange(ParseLinks(Content).Select(ParseProductByLink));
                    }
                }
            }
            return products;
        }
        private string GetContent(string Link, int page)
        {
            RestRequest req = new RestRequest(Link);
            req.AddParameter("p", page);
            var result = client.Execute(req);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return result.Content;
            }
            return null;
        }
        private Product ParseProductByLink(string link)
        {
            Thread.Sleep(1000);
            var productpage = client.Execute(new RestRequest(link));
            if (productpage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var product = ParceProduct(productpage.Content);
                product.Link = link;
                return product;
            }
            return null;
        }
        public IEnumerable<string> ParseLinks(string Content)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Content);
            var res = doc.DocumentNode.Descendants("a").Where(a => a.GetAttributeValue("class", "") == "snippet-link");
            return res.Select(r => r.Attributes["href"].Value.ToString());
        }
        public IEnumerable<Product> ParseProductsFromPage(string Content)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Content);
            var products = doc.DocumentNode.Descendants("div").Where(e => e.GetAttributeValue("class", "") == "item__line");
            return products.Select(r =>
            {
                var Link = r.Descendants("a").FirstOrDefault(a => a.GetAttributeValue("class", "") == "snippet-link");
                var price = r.Descendants("span").FirstOrDefault(a => a.GetAttributeValue("class", "") == "price");
                return new Product()
                {
                    Link = Link.Attributes["href"]?.Value,
                    Name = Link.InnerText,
                    Price = price?.InnerText ?? "",
                    Image = r.Descendants("img").FirstOrDefault()?.Attributes["href"]?.Value,
                };
            });

        }
        private Product ParceProduct(string Content)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Content);
            var name = doc.DocumentNode.Descendants("span").FirstOrDefault(s => s.GetAttributeValue("class", "") == "title-info-title-text")?.InnerText;
            var text = doc.DocumentNode.Descendants("div").FirstOrDefault(s => s.GetAttributeValue("class", "") == "item-description")?.InnerText;
            var img = doc.DocumentNode.Descendants("div").FirstOrDefault(s => s.GetAttributeValue("class", "") == "gallery-img-frame js-gallery-img-frame").GetAttributeValue("data-url", "");
            var price = doc.DocumentNode.Descendants("span").FirstOrDefault(s => s.GetAttributeValue("class", "") == "js-item-price")?.InnerText;

            return new Product()
            {
                Name = name,
                Text = text,
                Image = img,
                Price = price,
            };
        }
    }
}
