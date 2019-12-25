using AvitoHelper.DataBase;
using AvitoHelper.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvitoHelper.Filters
{
    public class AccessFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string Key = context.HttpContext.Request.Cookies["AcccessKey"];

            var _context = (DatabaseContext)context.HttpContext.RequestServices.GetService(typeof(DatabaseContext));

            User acc = _context.Users.FirstOrDefault(a => a.AccessKey == Key);

            if (acc == null)
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
