using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AvitoHelper.Types;
using DotLiquid;

namespace AvitoHelper.Helpers
{
    public class EmailHelper
    {
        public string GetAuthEmail(string pass, string AuthLink)
        {
            string email = File.ReadAllText("static/auth.html");
            Template template = Template.Parse(email);
            var res = template.Render(Hash.FromAnonymousObject(new { password = pass, link = AuthLink, Date = DateTimeOffset.Now }));
            return res;
        }
        public string GetProductEmail(List<Product> products)
        {
            string email = File.ReadAllText("static/look.html");
            Template template = Template.Parse(email);
            Template.RegisterSafeType(typeof(Product), typeof(Product).GetProperties().Select(x => x.Name).ToArray());
            var res = template.Render(Hash.FromAnonymousObject(new { Products = products }));
            return res;
        }
    }
}
