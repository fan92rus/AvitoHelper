using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvitoHelper.DataBase;
using AvitoHelper.Helpers;
using AvitoHelper.Services;
using AvitoHelper.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AvitoHelper.Controllers
{
    public class LoginModel
    {
        public string email { get; set; }
        public string pass { get; set; }
    }
    public class AuthController : CustomController
    {
        EmailSender _emailSender;
        ILogger<AuthController> _loger;
        EmailHelper _emailHelper;
        public AuthController(DatabaseContext context, EmailSender emailSender, ILogger<AuthController> loger, EmailHelper emailHelper) : base(context)
        {
            this._emailHelper = emailHelper;
            this._emailSender = emailSender;
            this._loger = loger;
            _loger.LogDebug("Created AuthController");
        }
        [HttpGet("Auth/GetCode")]
        public ActionResult GetCode(string email)
        {
            _loger.LogDebug("Get Code");
            var login = Environment.GetEnvironmentVariable("AUTH_EMAIL_LOGIN");
            var passEmail = Environment.GetEnvironmentVariable("AUTH_EMAIL_PASSWORD");
            var link = Environment.GetEnvironmentVariable("FRONTEND_DOMAIN");

            var user = _context.Users.FirstOrDefault(u => u.email == email);
            var passUser = new StringHelper().RandomPassword(6);
            if (user != null)
            {
                _loger.LogDebug("Avalible user");
                _context.Attach(user);
                user.PasswordHash = new Crypto().CalculateMD5Hash(passUser);
            }
            else
            {
                _loger.LogDebug("New User");
                user = new User()
                {
                    AccessKey = Guid.NewGuid().ToString(),
                    email = email,
                    PasswordHash = new Crypto().CalculateMD5Hash(passUser),
                    Limit = 15,
                };
                _context.Users.Add(user);
                Response.Cookies.Append("AcccessKey", user.AccessKey);
            }
            _context.SaveChanges();
            string activationURL = $"{link}/#/Auth/{user.AccessKey}";
            string text = _emailHelper.GetAuthEmail(passUser, activationURL);
            _emailSender.Execute(login, passEmail, email, "Поиск товаров на досках регистрация", text);

            return new JsonResult(true);
        }

        [HttpGet("Auth/SetKey")]
        public ActionResult SetKey(string key)
        {
            var user = _context.Users.FirstOrDefault(u => u.AccessKey == key);
            if (user != null)
            {
                Response.Cookies.Append("AcccessKey", key);
                return new JsonResult(true);
            }
            return new JsonResult(false);
        }
        [HttpPost("Auth/Login")]
        public ActionResult Login([FromBody] LoginModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.email == model.email);
            if (user != null)
            {
                if (user.PasswordHash == new Crypto().CalculateMD5Hash(model.pass))
                {
                    Response.Cookies.Append("AcccessKey", user.AccessKey);
                    return new JsonResult(true);
                }
            }
            return new JsonResult(false);
        }

    }
}