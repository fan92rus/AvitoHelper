using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvitoHelper.DataBase;
using AvitoHelper.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvitoHelper.Controllers
{
    public class CustomController : ControllerBase
    {
        protected readonly DatabaseContext _context;

        public CustomController(DatabaseContext context)
        {
            _context = context;
        }
        public User Account => Key != null ? _context.Users.FirstOrDefault(u => u.AccessKey == Key) : null;
        public string Key => HttpContext?.Request?.Cookies?["AcccessKey"];

    }
}
